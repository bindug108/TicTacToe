using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Services;
using TicTacToe.Extensions;
using Microsoft.AspNetCore.Routing;
using TicTacToe.Models;
using Microsoft.AspNetCore.Rewrite;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using TicTacToe.Options;
using Microsoft.AspNetCore.Mvc.Razor;

namespace TicTacToe
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public IHostingEnvironment _hostingEnvironment { get; }


        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = "Resources");
            //services.AddDirectoryBrowser();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IGameInvitationService, GameInvitationService>();
            services.Configure<EmailServiceOptions>(_configuration.GetSection("Email"));
            //services.AddSingleton<IEmailService, EmailService>();
            services.AddEmailService(_hostingEnvironment, _configuration);
            services.AddRouting();
            services.AddSession(o => o.IdleTimeout = TimeSpan.FromMinutes(30));
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);
        }

        public void ConfigureStagingServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
                app.UseExceptionHandler("/Home/Error");

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.UseStaticFiles();
            app.UseSession();

            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("CreateUser", context =>
            {
                var firstName = context.Request.Query["firstName"];
                var lastName = context.Request.Query["lastName"];
                var email = context.Request.Query["email"];
                var password = context.Request.Query["password"];

                var userService = context.RequestServices.GetService<IUserService>();
                userService.RegisterUser(new UserModel
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password
                });

                return context.Response.WriteAsync($"User {firstName} {lastName} has been successfully created");
            });

            var newUserRoutes = routeBuilder.Build();
            app.UseRouter(newUserRoutes);

            var options = new RewriteOptions();
            options.AddRewrite("/NewUser", "/UserRegistration/Index", false);
            app.UseRewriter();

            app.UseWebSockets();
            app.UseCommunicationMiddleware();

            var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-GB"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            localizationOptions.RequestCultureProviders.Clear();
            localizationOptions.RequestCultureProviders.Add(new CultureProviderResolverService());

            app.UseRequestLocalization(localizationOptions);

            //app.UseDirectoryBrowser();
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseStatusCodePages("text/plain", "HTTP Error - Status Code: {0}");
        }
    }
}
