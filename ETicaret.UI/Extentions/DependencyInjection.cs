using ETicaret.Infrastructure.AppContext;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Identity;
using ETicaret.Domain.Entities;
using AspNetCoreHero.ToastNotification;

namespace ETicaret.UI.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUIService(this IServiceCollection services)
        {
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
             .AddDataAnnotationsLocalization();
            services.AddLocalization(opt => opt.ResourcesPath = "Resources");
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // services.AddSingleton<IHtmlLocalizer<ModelResource>, HtmlLocalizer<ModelResource>>();


            services.Configure<RequestLocalizationOptions>(opt =>
            {
                var supCultures = new List<CultureInfo>()
                {
                    new CultureInfo("tr"),
                    new CultureInfo("en")
                };
                opt.DefaultRequestCulture = new RequestCulture("en");
                opt.SupportedCultures = supCultures;
                opt.SupportedUICultures = supCultures;

            }
            );

            AddIdentityService(services);
            services.AddNotyf(config =>
            {
                config.HasRippleEffect = true;
                config.DurationInSeconds = 5;
                config.Position = NotyfPosition.BottomRight;
                config.IsDismissable = true;
            });


            return services;
        }



        private static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            return services;
        }
    }
}
