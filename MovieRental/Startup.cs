
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieRental.Core.Account;
using MovieRental.Core.Events;
using MovieRental.Core.Events.Common;
using MovieRental.Core.Interfaces;
using MovieRental.Core.Movie;
using MovieRental.Handlers;
using MovieRental.Middlewares;
using MovieRental.Persitence;
using MovieRental.Persitence.Repositories;
using MovieRental.Utils;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;

namespace MovieRental
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ConfigureDatabase(services);
            services.AddScoped<IRepository<PriceUpdateLogEntity>, Repository<PriceUpdateLogEntity>>();
            services.AddScoped<IRepository<PurchaseLogEntity>, Repository<PurchaseLogEntity>>();
            services.AddScoped<IRepository<RentalLogEntity>, Repository<RentalLogEntity>>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieLikeRepository, MovieLikeRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IDomainHandler<PriceUpdated>, PriceUpdatedHandler>();
            services.AddScoped<IDomainHandler<MovieBuyed>, MovieBuyedHandler>();
            services.AddScoped<IDomainHandler<RentalMovie>, RentalMovieHandler>();
            services.AddScoped<IEventDispatcher, NetCoreEventContainer>();
            services.AddHttpContextAccessor();
            services.AddSingleton<ITokenFactory, JwtFactory>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Api Byron Lemus", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header {token}",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
                c.DescribeAllEnumsAsStrings();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var signingKey = Convert.FromBase64String(Configuration["Jwt:SigningSecret"]);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc();
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("MovieConnection");
            if (!string.IsNullOrEmpty(connectionString))
                services.AddDbContext<MovieDbContext>(opt => opt.UseSqlServer(connectionString));
            else
                services.AddDbContext<MovieDbContext>(opt => opt.UseInMemoryDatabase("MovieDb"));

        }
    }
}
