using ETicaret.Infrastructure.AppContext;
using ETicaret.Infrastructure.Repositories.AdminRepositories;
using ETicaret.Infrastructure.Repositories.CampaignRepositories;
using ETicaret.Infrastructure.Repositories.CategoryRepositories;
using ETicaret.Infrastructure.Repositories.CategorySizeTypeProductRepositories;
using ETicaret.Infrastructure.Repositories.CategorySizeTypeRepositories;
using ETicaret.Infrastructure.Repositories.CustomerAddressRepositories;
using ETicaret.Infrastructure.Repositories.CustomerRepositories;
using ETicaret.Infrastructure.Repositories.OrderDetailRepositories;
using ETicaret.Infrastructure.Repositories.OrderRepositories;
using ETicaret.Infrastructure.Repositories.ProductCampaignLogRepositories;
using ETicaret.Infrastructure.Repositories.ProductRepositories;
using ETicaret.Infrastructure.Repositories.SizeRepositories;
using ETicaret.Infrastructure.Repositories.SizeTypeRepositories;
using ETicaret.UI.Seeds;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ETicaret.Infrastructure.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(configuration.GetConnectionString(AppDbContext.DevConnectionString));
            });

            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISizeTypeRepository, SizeTypeRepository>();
            services.AddScoped<ICategorySizeTypeRepository, CategorySizeTypeRepository>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<ICategorySizeTypeProductRepository, CategorySizeTypeProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IProductCampaignLogRepository, ProductCampaignLogRepository>();

           // Hangfire konfigürasyonu ve servisi eklendi (uygulamanıza ekler ve Hangfire'ın kullanacağı depolama alanını belirleme)
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(configuration.GetConnectionString(AppDbContext.DevConnectionString));
            });
          
            services.AddHangfireServer();

            //Veritabanına başlangıç verilerini eklemek için AdminSeed sınıfını kullanır. 
            AdminSeed.SeedAsync(configuration).GetAwaiter().GetResult();
            return services;
        }
    }
}
