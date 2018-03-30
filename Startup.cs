using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBasedMicroservice.Models;
using EventBasedMicroservice.Services;
using EventBasedMicroservice.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resilience.Http;

namespace EventBasedMicroservice
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
            services.AddMvc();
            services.Configure<AppSettings>(Configuration);
            string connectString = Configuration.GetConnectionString("DefaultConnString");
            //services.AddDbContext<MvcMusicStoreContext>(options => options.UseSqlServer(connectString));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<IBasketService, BasketService>();
            services.AddSingleton<IHttpClient, StandardHttpClient>();
            services.AddTransient<IIdentityParser<ApplicationUser>, IdentityParser>();

            var useLoadTest = Configuration.GetValue<bool>("UseLoadTest");
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            var callBackUrl = Configuration.GetValue<string>("CallBackUrl");
            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
           .AddCookie()
           .AddOpenIdConnect(options => {
               options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               options.Authority = identityUrl.ToString();
               options.SignedOutRedirectUri = callBackUrl.ToString();
               options.ClientId = useLoadTest ? "mvctest" : "mvc";
               options.ClientSecret = "secret";
               options.ResponseType = useLoadTest ? "code id_token token" : "code id_token";
               options.SaveTokens = true;
               options.GetClaimsFromUserInfoEndpoint = true;
               options.RequireHttpsMetadata = false;
               options.Scope.Add("openid");
               options.Scope.Add("profile");
               options.Scope.Add("orders");
               options.Scope.Add("basket");
               options.Scope.Add("marketing");
               options.Scope.Add("locations");
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Catalog}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "defaultError",
                    template: "{controller=Error}/{action=Error}");
            });
        }
    }
}
