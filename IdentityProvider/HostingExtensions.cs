using System.Security.Cryptography.X509Certificates;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using IdentityProvider.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Serilog;

namespace IdentityProvider
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            var credential = new DefaultAzureCredential();
            builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVault:RootUri"]), credential);

            var connectionString = builder.Configuration["IdentityServerSqlConnectionString"] ?? throw new ArgumentNullException();
            var licenceKey = builder.Configuration["IdentityServerKey"] ?? throw new ArgumentNullException();

            builder.Services.AddDataProtection()
                .PersistKeysToAzureBlobStorage(new Uri(builder.Configuration["DataProtection:Keys"]), credential)
                .ProtectKeysWithAzureKeyVault(new Uri(builder.Configuration["DataProtection:ProtectionKeyForKeys"]),
                    credential);

            var secretClient = new SecretClient(new Uri(builder.Configuration["KeyVault:RootUri"]), credential);
            var secretResponse = secretClient.GetSecret(builder.Configuration["KeyVault:CertificateName"]);

            Log.Logger.Error(secretResponse.Value.Value);

            var signingCertificate = new X509Certificate2(
                Convert.FromBase64String(secretResponse.Value.Value), 
                (string) null,
                X509KeyStorageFlags.MachineKeySet);

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
                    options.LicenseKey = licenceKey;
                    options.KeyManagement.Enabled = false;
                })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<IdentityUser>()
                .AddSigningCredential(signingCertificate);

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

            app.UseDeveloperExceptionPage();


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