using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DevJobs.Api.Settings
{
    public static class SwaggerConfiguration
    {
        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title       = "Devjob Api Server",
                    Version     = "v1",
                    Description = "Devjob job tracker Developed by Asp.net Batch-8 (Team-1)",
                    Contact     = new OpenApiContact
                    {
                        Name    = "Dev Skill",
                        Email   = "info@devskill.com",
                        Url     = new Uri("https://www.devskill.com")
                    },
                    License     = new OpenApiLicense
                    {
                        Name    = "MIT License",
                        Url     = new Uri("https://opensource.org/licenses/MIT")
                    },  
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name        = "Authorization",
                    Type        = SecuritySchemeType.ApiKey,
                    In          = ParameterLocation.Header,
                    Scheme      = "Bearer",
                    BearerFormat= "Jwt",
                    Description = "JWT Authorazation header using the Bearer scheme",
                    Reference   = new OpenApiReference
                    {
                        Id      = "Bearer",
                        Type    = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>() }
                });

                options.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

                options.CustomSchemaIds(x => x.FullName);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                options.EnableAnnotations();
            });

            return builder;
        }
        
    }
}
