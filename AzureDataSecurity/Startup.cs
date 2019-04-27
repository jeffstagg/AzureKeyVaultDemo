using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureDataSecurity.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureDataSecurity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            //this line stays the same - you will still be using the Configuration object
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"])
            );

            var tokenProvider = new AzureServiceTokenProvider();
            var keyvaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                        tokenProvider.KeyVaultTokenCallback));

            var blobAccount = Configuration["KeyVaultDetails:BlobAccountName"];
            var blobContainer = Configuration["KeyVaultDetails:BlobContainerName"];
            var blobName = Configuration["KeyVaultDetails:BlockBlobName"];
            var sasToken = Configuration["KeyVaultDetails:BlobSASToken"];
            var keyIdentifier = Configuration["KeyVaultDetails:KeyIdentifier"];

            services.AddDataProtection()

                //we will store our keys in Azure blob storage
                .PersistKeysToAzureBlobStorage(
                    new Uri($"https://{blobAccount}.blob.core.windows.net/{blobContainer}/{blobName}{sasToken}"))

                //we will protect those keys with the Azure key vault
                .ProtectKeysWithAzureKeyVault(
                    keyvaultClient,
                    keyIdentifier);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
