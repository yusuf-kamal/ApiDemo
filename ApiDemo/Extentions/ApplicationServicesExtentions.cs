using ApiDemo.Helper;
using ApiDemo.Response_Module;
using Core.Entities.Interface;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApllicationServices(this IServiceCollection services)
        {
         services.AddScoped<IProductRepo, ProductRepo>();
         services.AddScoped<IUnitOFWork, UnitOFWork>();
         services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            services.AddScoped<IBasketRepo,BasketRepo>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentServices,PaymentServices>();
            services.AddScoped<IResponseCasheService, ResponseCasheService>();
         services.AddAutoMapper(typeof(MappingProfiles));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors=actionContext.ModelState.Where(e=>e.Value.Errors.Count>0)
                    .SelectMany(e=>e.Value.Errors)
                    .Select(e=>e.ErrorMessage).ToArray() ;
                    var ErrorResponse = new ApiValdiationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ErrorResponse);

                };
            });
            return services;
        }
    }
}
