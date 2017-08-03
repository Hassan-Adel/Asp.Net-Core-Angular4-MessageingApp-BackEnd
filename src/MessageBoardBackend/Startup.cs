using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MessageBoardBackend
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase());

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddCors(options => options.AddPolicy("Cors",
                builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                }));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            //In a production enviroment will have the secrit in a config file
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("the secret phrase"));

            //register Middelware
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {//this will cause it to perform some of the common Auth checks in the backgroud 
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters
                {
                    //the false should be true for a secure production envoroment (here for simplicity)
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateLifetime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            });

            //Cors : the Cors policy name that has been specified above
            app.UseCors("Cors");

            //Make sure Mvc is registered after the Jwty middelware
            app.UseMvc();

            SeedData(app.ApplicationServices.GetService<ApiContext>());
        }

        public void SeedData(ApiContext context)
        {
            context.Messages.Add(new Models.Message
            {
                Owner = "John",
                Text = "hello"
            });
            context.Messages.Add(new Models.Message
            {
                Owner = "Tim",
                Text = "Hi"
            });
            context.Users.Add(new Models.User
            {
                Email = "admin",
                Password = "admin",
                FirstName="Admin",
                Id="1"
            });

            context.SaveChanges();
        }

    }
}
