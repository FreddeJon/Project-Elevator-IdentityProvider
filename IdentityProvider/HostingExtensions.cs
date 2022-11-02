using Azure.Identity;
using IdentityProvider.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityProvider
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVault:RootUri"]), new DefaultAzureCredential());

            var connectionString = builder.Configuration["IdentityServerSqlConnectionString"] ?? throw new ArgumentNullException();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddRazorPages();

            builder.Services.AddIdentityServer(options =>
                {
                    // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                    options.EmitStaticAudienceClaim = true;
                    options.LicenseKey = builder.Configuration["IdentityServerKey"];
                })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<IdentityUser>();

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
                                           | ForwardedHeaders.XForwardedProto;
            });


            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {

            app.UseForwardedHeaders();

            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();
            app.MapRazorPages().RequireAuthorization();

            return app;
        }
    }
}