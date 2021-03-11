using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ServicesLib.Domain.Utilities;
using ServicesLib.Services.Database;
using System;

namespace School_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Build a configuration system to read appsettings
            // Pre DI approach
            var configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .Build();

            // Create Log
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            
            var host = CreateHostBuilder(args).Build();

            var config = host.Services.GetRequiredService<IConfiguration>();

            //Misc setup 
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var adminUserEmail = config["AppSettings:AdminUserEmail"];

                    var services = scope.ServiceProvider;
                    var db = services.GetRequiredService<DbContextOptions<SchoolDbContext>>();

                    SeedAdminUser.InitializeAdminUser(services, "12345678", adminUserEmail).Wait();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Log.Fatal(e, "Db Update Concurrency exception");
                    Log.Information("Exiting application ...");
                    Log.CloseAndFlush();
                    host.StopAsync();
                }
                catch (DbUpdateException e)
                {
                    Log.Fatal(e, "Db update exception");
                    Log.Information("Exiting application ...");
                    Log.CloseAndFlush();
                    host.StopAsync();
                }
                catch (InvalidOperationException e)
                {
                    Log.Fatal(e, "Db Invalid Operation exception");
                    Log.Information("Exiting application ...");
                    Log.CloseAndFlush();
                    host.StopAsync();
                }
                catch (Exception e)
                {
                    Log.Fatal(e, "General exception");
                    Log.Information("Exiting application ...");
                    Log.CloseAndFlush();
                    host.StopAsync();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                //.ConfigureLogging((context, logging) =>
                //{
                //    logging.ClearProviders();
                //    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                //    logging.AddConsole();
                //    logging.AddDebug();
                //})
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}