using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.WebApi.Configurations;

[ExcludeFromCodeCoverage]
public static class SwaggerSetup
{
    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurando o serviço de documentação do Swagger
        services.AddSwaggerGen(c =>
        {
            c.CustomOperationIds(e => $"{e.HttpMethod}_{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.RouteValues["action"]}");

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Booking Cancun Api",
                Version = "v1.0.0",
                Description = "API to book at the last hotel in Cancun",
                Contact = new OpenApiContact
                {
                    Name = "Felipe Fahl",
                    Url = new Uri("https://felipefahl.dev/")
                }
            });

            c.EnableAnnotations();
        });
    }

    public static void UseSwaggerSetup(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // Geração de um endpoint do Swagger para cada versão descoberta
            foreach (var groupName in provider.ApiVersionDescriptions.Select(x => x.GroupName))
            {
                options.SwaggerEndpoint($"swagger/{groupName}/swagger.json",
                    groupName.ToUpperInvariant());

                options.RoutePrefix = string.Empty; // Set Swagger UI at apps root
            }
        });
    }
}