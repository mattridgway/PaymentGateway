using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace Stark.PaymentGateway
{

    public static class IServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    "v1.0",
                    new OpenApiInfo
                    {
                        Title = "Stark Payment Gateway API v1.0",
                        Version = "v1.0"
                    });

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token. You should include the word 'bearer' followed by a space and then the token itself.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme },
                            Scheme = "OAuth2",
                            In = ParameterLocation.Header
                        },
                        new string[] { }
                    }
                });
                config.IgnoreObsoleteActions();
                config.IgnoreObsoleteProperties();
                config.IncludeXmlComments(
                    Path.Combine(
                        AppContext.BaseDirectory,
                        "paymentgateway-documentation.xml"));

            });

            return services;
        }

        public static IServiceCollection AddApiSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration.GetValue<string>("Authentication:JWT:Authority");
                    options.RequireHttpsMetadata = true;
                    options.Audience = "paymentgateway";
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ProcessPayment", policy => policy.RequireClaim("scope", "paymentgateway.process"));
                options.AddPolicy("RetrieveDetails", policy => policy.RequireClaim("scope", "paymentgateway.retrieve"));
            });

            return services;
        }
    }
}
