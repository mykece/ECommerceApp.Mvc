using ETicaret.Applicationn.Services.AdminService;
using Microsoft.Extensions.DependencyInjection;
using ETicaret.Applicationn.Services.AccountServices;
using ETicaret.Applicationn.Services.ProductServices;
using ETicaret.Applicationn.Services.MailServices;
using ETicaret.Applicationn.Services.CategoryServices;
using ETicaret.Applicationn.Services.SizeTypeServices;
using ETicaret.Applicationn.Services.SizeServices;
using ETicaret.Applicationn.Services.CustomerServices;
using ETicaret.Applicationn.Services.CampaignServices;
using ETicaret.Applicationn.BackgroundServices;
using Hangfire;
using ETicaret.Applicationn.Services.CustomerAddressServices;
using ETicaret.Applicationn.Services.OrderServices;
using ETicaret.Applicationn.Services.HangFire;
using ETicaret.Applicationn.Services.ProductCampaignLogServices;
using ETicaret.Applicationn.Services.CharServices;
namespace ETicaret.Applicationn.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISizeTypeService, SizeTypeService>();
            services.AddScoped<ISizeServices, SizeService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<CampaignBackgroundService>();
            services.AddScoped<IProductCampaignLogService, ProductCampaignLogService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<HangFireService>();







            return services;
        }
        public static IServiceCollection ConfigureHangfireServices(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(connectionString);
                config.UseFilter(new MyAuthorizationFilter());
            });

            services.AddHangfireServer();

            return services;
        }


        //public static void ConfigureHangfireJobs(IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        //{
        //    var backgroundService = serviceProvider.GetRequiredService<CampaignBackgroundService>();
        //    recurringJobManager.AddOrUpdate("deactivate-expired-campaigns",
        //        () => backgroundService.DeactivateExpiredCampaigns(), Cron.Daily);
        //}
    }
}
