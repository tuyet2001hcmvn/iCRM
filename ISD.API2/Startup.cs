using AutoMapper;
using ISD.API.EntityModels.Entities;
using ISD.API.Repositories;
using ISD.API.Repositories.Marketing;
using ISD.API.Repositories.Services.Marketing;
using ISD.API2.Filters;
using ISD.API2.Middlewares;
using ISD.API2.ServiceExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace ISD.API2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var origins = Configuration.GetValue<string>("AllowedOrigins").Split(";");
            var connectionString = Configuration.GetConnectionString("SqlDatabase");
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader();
                                  });
                options.AddPolicy("TrackingEmailPolicy",
                                   builder =>
                                   {
                                       builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                   });
            });
            services.AddControllers(config =>
            {
                config.Filters.Add(new ValidationActionFilter());
            }).AddNewtonsoftJson();
            services.AddCustomServices();
            services.AddRepositories();
            services.AddUnitOfwork();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddDbContext<ICRMDbContext>(options => options.UseSqlServer(connectionString));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ISDNET5API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();            
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ISDNET5API v1"));
            }
           
            app.UseExceptionMiddleware();
          //  app.UseLoggingRequestMiddleware();
            app.UseFileServer();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Upload", "Images")),
                RequestPath = new PathString("/Images")
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();            
            });
            
        }
    }
}
