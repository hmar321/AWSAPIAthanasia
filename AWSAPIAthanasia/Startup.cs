using ApiAthanasia.Data;
using ApiAthanasia.Helpers;
using ApiAthanasia.Repositories;
using Azure.Security.KeyVault.Secrets;
using NSwag.Generation.Processors.Security;
using NSwag;
using Microsoft.Extensions.Azure;
using Microsoft.EntityFrameworkCore;

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
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddAzureClients(factory =>
        {
            factory.AddSecretClient(this.Configuration.GetSection("KeyVault"));
        });
        SecretClient secretClient = services.BuildServiceProvider().GetService<SecretClient>();
        HelperActionServicesOAuth helper = new HelperActionServicesOAuth(secretClient);
        services.AddSingleton<HelperActionServicesOAuth>(helper);
        KeyVaultSecret secret = secretClient.GetSecret("SqlServerAzure");
        string connectionString = "server=localhost;port=3306;user id=root;password=Admin123@;database=athanasia";
        services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());
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
        services.AddTransient<HelperMails>();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddTransient<IRepositoryAthanasia, RepositoryAthanasia>();
        services.AddDbContext<AthanasiaContext>(
            options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
        }
        app.UseOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Api Athanasia");
            options.RoutePrefix = "";
        });

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hola");
            });
        });
    }
}