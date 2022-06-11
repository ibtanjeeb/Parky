using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ParkyAPI
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
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();
            services.AddAutoMapper(typeof(ParkyMapping));
            services.AddApiVersioning(options => {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;





            }) ;
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ParkyOpenAPISpec",
                new Microsoft.OpenApi.Models.OpenApiInfo()
                {

                    Title = "ParkyAPI",
                    Version = "1",
                    Description = "National Parky API NP",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "threepointonefour1993@gmail.com",
                        Name = "Md. Tushon",
                        Url = new Uri("https://www.dotnetmastery.com/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                    
                 



                });
                //options.SwaggerDoc("ParkyOpenAPISpecTrails",
                //new Microsoft.OpenApi.Models.OpenApiInfo()
                //{

                //    Title = "ParkyAPI Tarils",
                //    Version = "1",
                //    Description = "National Parky API Tarils",
                //    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                //    {
                //        Email = "threepointonefour1993@gmail.com",
                //        Name = "Md. Tushon",
                //        Url = new Uri("https://www.dotnetmastery.com/")
                //    },
                //    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                //    {
                //        Name = "MIT License",
                //        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                //    }





                //});
                var xmlCommentfile = $"{ Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlfullCommentpath = Path.Combine(AppContext.BaseDirectory, xmlCommentfile);
                options.IncludeXmlComments(xmlfullCommentpath);



            });


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
                //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");
                options.RoutePrefix = "";

            }
            );


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
