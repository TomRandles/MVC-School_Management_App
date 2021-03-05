using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicesLib.Domain.Utilities;
using ServicesLib.Services.Database;
using ServicesLib.Services.Repository.Generic;
using System;

namespace School_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
                    ErrorProcessing.ProcessException("Program - db update concurrency error: ",
                                            e,
                                            ErrorType.Critical);

                    // Log.Information("Exiting application ...");
                    // Log.CloseAndFlush();
                    host.StopAsync();
                }
                catch (DbUpdateException e)
                {
                    ErrorProcessing.ProcessException("Program - db update error: ",
                                                     e, 
                                                     ErrorType.Critical);
                    // Log.Information("Exiting application ...");
                    // Log.CloseAndFlush();
                    host.StopAsync();
                }
                catch (InvalidOperationException e)
                {
                    ErrorProcessing.ProcessException("Program - db invalid operation error: ",
                                                     e,
                                                     ErrorType.Critical);

                    // Log.Information("Exiting application ...");
                    // Log.CloseAndFlush();
                    host.StopAsync();
                }
                catch (Exception e)
                {
                    ErrorProcessing.ProcessException("Program - general error: ",
                                            e,
                                            ErrorType.Critical);

                    //Log.Information("Exiting application ...");
                    //Log.CloseAndFlush();
                    host.StopAsync();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
