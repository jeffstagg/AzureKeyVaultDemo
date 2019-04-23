using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace AzureDataSecurity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile("appsettings.json");
                    
                    //build the configuration object, then grab the keyvault endpoint from it to set up keyvault
                    var builtDevConfig = config.Build();
                    var tokenProvider = new AzureServiceTokenProvider();
                    var keyvaultClient = new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(
                                tokenProvider.KeyVaultTokenCallback));

                    config.AddAzureKeyVault(
                        builtDevConfig["ConnectionStrings:KeyvaultEndpoint"],
                        keyvaultClient,
                        new DefaultKeyVaultSecretManager());
                })
                .UseStartup<Startup>()
                .Build();
    }
}
