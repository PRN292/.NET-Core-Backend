using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StrangerDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using StrangerDetection.Services;
using StrangerDetection.Helpers;
using StrangerDetection.UnitOfWorks;

namespace StrangerDetection
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
          //  System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "Firebase Admin Key\\strangerdetection-firebase-adminsdk-ndswy-678c270870.json");
            Configuration = configuration;
           Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "Firebase Admin Key\\strangerdetection-firebase-adminsdk-ndswy-678c270870.json");
             FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
                
                
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();
            services.Configure<AppSetting>(Configuration.GetSection("AppSettings"));
            services.AddDbContext<StrangerDetectionContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IKnownPersonService, KnownPersonService> ();
            services.AddScoped<IEncodingService, EncodingService>();
            services.AddScoped<GRPCClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseRouting();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
