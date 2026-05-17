using System;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace src
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nPOTE API initialized successfully. Listening at: http://localhost:3100\n");
            //Console.WriteLine("🔒 Secure Link:  https://localhost:3101");
            Console.ResetColor();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var bootstrapConfig = config.Build();
                    var keyVaultEndpoint = bootstrapConfig["AZURE_KEY_VAULT_ENDPOINT"];
                    
                    // Note: If you want to guard against future ghost environment variable crashes,
                    // add: !context.HostingEnvironment.IsDevelopment() here.
                    if (!string.IsNullOrWhiteSpace(keyVaultEndpoint))
                    {
                        var credential = new DefaultAzureCredential();
                        config.AddAzureKeyVault(new Uri(keyVaultEndpoint), credential);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}