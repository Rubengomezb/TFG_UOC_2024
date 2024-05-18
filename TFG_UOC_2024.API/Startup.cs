using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.CORE.Components;
using TFG_UOC_2024.CORE.Services.Interfaces;
using TFG_UOC_2024.CORE.Services.User;
using TFG_UOC_2024.API.Components;
using TFG_UOC_2024.CORE.Helpers;
using TFG_UOC_2024.CORE.Services.Core;
using TFG_UOC_2024.DB.Repository.Interfaces;
using TFG_UOC_2024.DB.Repository;
using TFG_UOC_2024.CORE.Managers.Interfaces;
using TFG_UOC_2024.CORE.Managers;
using TFG_UOC_2024.CORE.Services.Menu;
using TFG_UOC_2024.CORE.Clients;

namespace TFG_UOC_2024.API
{
    public class Startup
    {
        public static IWebHostEnvironment AppEnvironment { get; private set; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            AppEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Application Insights
            services.AddApplicationInsightsTelemetry(Configuration);

            // enable caching
            services.AddMemoryCache();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Sweet System API",
                        Version = "v1",
                        Description = "API for handling user/content data",
                        Contact = new OpenApiContact
                        {
                            Name = "Some Developer",
                            Email = string.Empty,
                            Url = new Uri("https://rainstormtech.com"),
                        },
                    });

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "TFG_UOC_2024.API.xml");
                c.IncludeXmlComments(filePath);
            });

            // define SQL Server connection string
            services.AddDbContext<ApplicationContext>(options =>
                options
                    .UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                        ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection")))
                    );

            // as of 3.1.1 the internal .net core JSON doens't handle referenceloophandling so we still need to use Newtonsoft
            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            // add identity services
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            // enable CORS
            services.AddCors(options =>
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                })
            );

            // add appsettings availability
            services.AddSingleton(Configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // automapper - tell it where to find the automapper profile
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddMvc();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            services.AddLogging(loggingBuilder =>
            {
                //  loggingBuilder.AddApplicationInsights(aiKey);
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();

            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            // configure jwt authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // add authorization
            services.AddAuthorization(options => { });

            // handle authorization policies dynamically
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            // configure DI for application services

            /* Authentication / users / roles */
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRecipeManager, RecipeManager>();
            services.AddScoped<IMenuManager, MenuManager>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IHttpRecipeClient, HttpRecipeClient>();
            services.AddScoped<IUserRepository>(provider => new UserRepository(provider.GetRequiredService<ApplicationContext>()));
            services.AddScoped<IContactRepository>(provider => new ContactRepository(provider.GetRequiredService<ApplicationContext>()));
            services.AddScoped<IUserRoleRepository>(provider => new UserRoleRepository(provider.GetRequiredService<ApplicationContext>()));
            services.AddScoped<ICategoryRepository>(provider => new CategoryRepository(provider.GetRequiredService<ApplicationContext>()));
            services.AddScoped<IIngredientRepository>(provider => new IngredientRepository(provider.GetRequiredService<ApplicationContext>()));
            services.AddScoped<IMenuRepository>(provider => new MenuRepository(provider.GetRequiredService<ApplicationContext>()));
            services.AddScoped<IRecipeRepository>(provider => new RecipeRepository(provider.GetRequiredService<ApplicationContext>()));
            services.AddScoped<IUserFavoriteRepository>(provider => new UserFavoriteRepository(provider.GetRequiredService<ApplicationContext>()));
            services.AddScoped<IUserRoleRepository>(provider => new UserRoleRepository(provider.GetRequiredService<ApplicationContext>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseHttpsRedirection();
            }
            else
            {
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sweet System V1");
            });

            // handle DB seeding
            SeedDb.Initialize(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

            // use the following if hosting files/images using StandardFileService 
            var cachePeriod = env.IsDevelopment() ? "600" : "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            }); // cuz we're hosting some images

            app.UseRouting();

            app.UseCors(
               options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
