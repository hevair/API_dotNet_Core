using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Api.CrossCutting.DependencyInjection;
using Api.Domain.Interface.Services.User;
using Api.Domain.Interface;
using Api.Data.Repository;
using Api.Data.Context;
using Microsoft.EntityFrameworkCore;
using Api.Domain.Security;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace application
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
            // Add framework services.
            services.AddDbContext<MyContext>(options =>
            options.UseMySql(Configuration.GetValue<string>("ConnectionStrings:MySqlDB")
                        ,ServerVersion.AutoDetect(Configuration.GetValue<string>("ConnectionStrings:MySqlDB"))));

            ConfigureService.ConfigureDependenciesService(services);
             
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigutations"))
                .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);
            
            services.AddAuthentication(authOptions =>{
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions => {
                var paramsvalidation =  bearerOptions.TokenValidationParameters;
                paramsvalidation.IssuerSigningKey = signingConfigurations.Key;
                paramsvalidation.ValidAudience = tokenConfigurations.Audience;
                paramsvalidation.ValidIssuer = tokenConfigurations.Issuer;
                paramsvalidation.ValidateIssuerSigningKey = true;
                paramsvalidation.ValidateLifetime = true;
                paramsvalidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>{
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "application", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
                    Description = "Entre o token JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                    
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "application v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
