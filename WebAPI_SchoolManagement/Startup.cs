using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServicesLib.Services.Database;
using ServicesLib.Services.Repositories;

namespace WebAPI_SchoolManagement
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI_SchoolManagement", Version = "v1" });
            });

            #region Database configuration
            //Local DB Register
            services.AddDbContext<SchoolDbContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("DbConnection")
            ));

            //Ensure threadsafe code - validation code especially
            services.AddDbContext<SchoolDbContext>(ServiceLifetime.Transient);
            #endregion


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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI_SchoolManagement v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
