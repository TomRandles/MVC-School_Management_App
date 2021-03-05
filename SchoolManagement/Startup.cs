using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ServicesLib.Services.Database;
using ServicesLib.Domain.Models;
using ServicesLib.Services.Repositories;

namespace School_Management
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(options =>
            {
                // Make the session cookie essential
                options.Cookie.IsEssential = true;

                // This scale of timespan is necessary as the Observe Zone Razor Page is an always on screen
                options.IdleTimeout = TimeSpan.FromDays(1);
            });

            //Add http client services
            services.AddHttpClient();

            services.AddControllersWithViews();

            #region Configure JWT Token Authentication

            //Configure JWT Token Authentication
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    //Same Secret key will be used while creating the token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Application:Token").Value)),
                    ValidateIssuer = true,
                    //Usually, this is your application base URL
                    ValidIssuer = Configuration.GetSection("Application:DomainName").Value,
                    ValidateAudience = true,
                    //Here, we are creating and using JWT within the same application.
                    //In this case, base URL is fine.
                    //If the JWT is created using a web service, then this would be the consumer URL.
                    ValidAudience = Configuration.GetSection("Application:DomainName").Value,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            #endregion


            #region Activing Session
            // For Activing Session in .net core
            // services.AddMvc().AddSessionStateTempDataProvider();
            // services.AddSession();

            #endregion

            //#region Json Serilizing Problem Solution

            //services.AddMvc().AddJsonOptions(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //});

            //#endregion

            #region Database configuration
            //Local DB Register
            services.AddDbContext<SchoolDbContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("DbConnection")
            ));

            //Ensure threadsafe code - validation code especially
            services.AddDbContext<SchoolDbContext>(ServiceLifetime.Transient);
            #endregion

            #region Identity Configuration with EF

            /* services.AddDefaultIdentity<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SchoolDbContext>()
                // Add default token providers - part of automated password reset functionality
                .AddDefaultTokenProviders();
            */

            services.AddDefaultIdentity<AppUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<SchoolDbContext>()
                    // Add default token providers - part of automated password reset functionality
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
                option.Password.RequiredLength = 4;

                // Lockout settings.
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.AllowedForNewUsers = true;

                // User settings.
                option.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789&£$-._@+' ";
                option.User.RequireUniqueEmail = false;
            });

            #endregion

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                                                                   opt.TokenLifespan = TimeSpan.FromHours(2));

            #region Data access repositories
            services.TryAddTransient<IProgrammeRepository, ProgrammeRepository>();
            services.TryAddTransient<IModuleRepository, ModuleRepository>();
            services.TryAddTransient<IAssessmentRepository, AssessmentRepository>();
            services.TryAddTransient<ITeacherRepository, TeacherRepository>();
            services.TryAddTransient<IStudentRepository, StudentRepository>();
            services.TryAddTransient<IAssessmentResultRepository, AssessmentResultRepository>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            // Add JWToken to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });

            app.UseRouting();

            //Add JWToken Authentication service
            app.UseAuthentication();

            // The call to app.UseAuthorization() must appear between app.UseRouting() and app.UseEndpoints.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
