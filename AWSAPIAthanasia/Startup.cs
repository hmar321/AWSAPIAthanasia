using Microsoft.OpenApi.Models;
using NSwag.Generation.Processors.Security;
using NSwag;
using ApiAthanasia.Repositories;
using ApiAthanasia.Data;
using Microsoft.EntityFrameworkCore;
using AWSAPIAthanasia.Models.Util;
using Newtonsoft.Json;
using AWSAPIAthanasia.Helpers;
using ApiAthanasia.Helpers;

namespace AWSAPIAthanasia;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin", x => x.AllowAnyOrigin());
        });
        services.AddOpenApiDocument(document =>
        {
            document.Title = "Api Athanasia";
            document.Description = "Api Athanasia.  Proyecto Azure 2024";
            document.Version = "v1";
            document.AddSecurity("JWT", Enumerable.Empty<string>(),
                new NSwag.OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Copia y pega el Token añadiendole la palabra bearer en el campo 'Value:' así: \"Bearer TuToken\"."
                }
            );
            document.OperationProcessors.Add(
            new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddTransient<IRepositoryAthanasia, RepositoryAthanasia>();
        string jsonSecrets = HelperSecretManager.GetSecretsAsync().GetAwaiter().GetResult();
        KeysModel keysModel = JsonConvert.DeserializeObject<KeysModel>(jsonSecrets);
        HelperActionServicesOAuth helper = new HelperActionServicesOAuth(keysModel);
        services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());
        services.AddTransient<HelperActionServicesOAuth>(x => helper);
        services.AddSingleton<KeysModel>(x => keysModel);
        string connectionString = keysModel.MySqlAWS;
        services.AddDbContext<AthanasiaContext>(
            options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );


    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api Athanasia");
            options.RoutePrefix = "";
        });
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(x => x.AllowAnyOrigin());
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
            
        });
    }
}