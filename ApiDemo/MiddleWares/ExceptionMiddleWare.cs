﻿using ApiDemo.Response_Module;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ApiDemo.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleWare(RequestDelegate next
            ,ILogger<ExceptionMiddleWare> logger
            ,IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }
        public async Task InvokeAsync( HttpContext httpContext)
        {
            try
            {
                await _next( httpContext);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex,ex.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _environment.IsDevelopment() 
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError);
                var options=new JsonSerializerOptions { PropertyNamingPolicy= JsonNamingPolicy.CamelCase };
                var json=JsonSerializer.Serialize(response, options);
                await httpContext.Response.WriteAsync(json);

            }
        }
    }
}
