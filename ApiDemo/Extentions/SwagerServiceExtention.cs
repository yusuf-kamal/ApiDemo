using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiDemo.Extentions
{
    public static class SwagerServiceExtention
    {
        public static IServiceCollection AddSwagerDocumentation( this IServiceCollection services)
        {
           services.AddEndpointsApiExplorer();
           services.AddSwaggerGen(c=>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Demo", Version = "v1" });

               var securitySchema = new OpenApiSecurityScheme
               {
                   Description = "JWt  Auth Bearer Scheme",
                   Name = "Authorization",
                   In = ParameterLocation.Header,
                   Type = SecuritySchemeType.ApiKey,
                   Scheme = "bearer",
                   Reference = new OpenApiReference
                   {
                       Type = ReferenceType.SecurityScheme,

                       Id = "Bearer"
                   }
               };
               c.AddSecurityDefinition("Bearer", securitySchema);
               var securityRequerment = new OpenApiSecurityRequirement
               {
                   {
                       securitySchema,new string[] {} 
                   }
               };
               c.AddSecurityRequirement(securityRequerment);

           });
               return services;

        }
    }
}
