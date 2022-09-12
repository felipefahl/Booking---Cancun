using Booking.Cancun.WebApi.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;


namespace Booking.Cancun.WebApi;

[ExcludeFromCodeCoverage]
public class Startup : IPecegePayStartup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDatabase(Configuration);

        //Redis
        services.AddRedis(Configuration);

        services.AddControllers()
            .AddJsonOptions(opts => { opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); })
            .ConfigureApiBehaviorOptions(setup =>
            {
                setup.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

        // Swagger
        services.AddSwagger(Configuration);

        // API Compression
        services.AddResponseCompression();

        services.AddDependencyInjection();

        services.AddCapLibrary(Configuration);

        services.AddEmailSender(Configuration);

        // Add framework services.
        services.AddMvc();

        services.AddVersioning();
    }



    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(WebApplication app, IWebHostEnvironment CurrentEnvironment,
        IApiVersionDescriptionProvider provider)
    {
        if (CurrentEnvironment.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseLog(Configuration);

        app.UseCustomException();
        app.UseHttpsRedirection();

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseCors(c =>
        {
            c.AllowAnyMethod();
            c.AllowAnyOrigin();
        });

        app.UseEndpoints(endpoints =>
                endpoints.MapControllers()
        );

        //app.UseHangfireSetup(Configuration);

        app.UseSwaggerSetup(provider);

        app.UpdateDatabase(CurrentEnvironment);
    }
}


public interface IPecegePayStartup
{
    IConfiguration Configuration { get; }
    void ConfigureServices(IServiceCollection services);

    void Configure(WebApplication app, IWebHostEnvironment CurrentEnvironment, IApiVersionDescriptionProvider provider);
}

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder webApplicationBuilder)
        where TStartup : IPecegePayStartup
    {
        var startup =
            Activator.CreateInstance(typeof(TStartup), webApplicationBuilder.Configuration) as IPecegePayStartup;

        if (startup == null) throw new ArgumentException("Invalid class Startup.cs");

        startup.ConfigureServices(webApplicationBuilder.Services);

        webApplicationBuilder.Logging.ClearProviders();
        webApplicationBuilder.Logging.SetMinimumLevel(LogLevel.Trace);
        webApplicationBuilder.Host
            .UseWindowsService()
            .UseSerilog();

        var app = webApplicationBuilder.Build();

        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        startup.Configure(app, app.Environment, apiVersionDescriptionProvider);

        app.Run();

        return webApplicationBuilder;
    }
}