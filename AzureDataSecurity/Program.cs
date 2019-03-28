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

                    //this will allow us to grab the connection string
                    config.AddJsonFile("appsettings.json");

                    //build the configuration, then we can pull items from it.
                    var builtConfig = config.Build();

                    //set up keyvault
                    //var tokenProvider = new AzureServiceTokenProvider();
                    //var keyvaultClient = new KeyVaultClient((authority, resource, scope) =>
                    //    tokenProvider.KeyVaultTokenCallback(authority, resource, scope));
                    //config.AddAzureKeyVault(
                    //    builtConfig["Environment:KeyvaultEndpoint"],
                    //    keyvaultClient,
                    //    new DefaultKeyVaultSecretManager());

                })
                .UseStartup<Startup>()
                .Build();
    }
}
