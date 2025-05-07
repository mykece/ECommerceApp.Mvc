using ETicaret.Applicationn.BackgroundServices;
using ETicaret.Applicationn.Extentions;
using ETicaret.Applicationn.Services.HangFire;
using ETicaret.Infrastructure.Extentions;
using ETicaret.UI.Extentions;
using FluentValidation.AspNetCore;
using Hangfire;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


/*builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());*/ //Eklenen tüm validators'leri otomatik olarak sisteme ekler.

//Aþaðýdaki kodda DataAnnotations Validation devre dýþý býrakýlarak sadece fluent validation'da belirlediðimiz kurallar çalýþýyor.
//Bunu yapma sebebimiz, empty/null deðer girdiðimizde DataAnnotation's validasyonu devreye giriyor, fluent validation'da belirlediðim null olamaz kuralýný pas geçiyor.
//Dilenirse yukarýdaki kod aktif hale getirilip aþaðýdaki kod yoruma alýnarak, data annotation ve fluent validation kurallarý birlikte kullanýlabilir.
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv =>
    {
        fv.DisableDataAnnotationsValidation = true;
        fv.RegisterValidatorsFromAssemblyContaining<Program>(); //program.cs 'in olduðu tüm assembly'deki validator kurallarýný aktif hale getiriyor.
    });


builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddUIService();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


// Hangfire Dashboard ve Server eklendi. Hangfire Authorization  iþlemi ,sayfaya sadece admin ulaþabilir.
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new MyAuthorizationFilter() }
});
//app.UseHangfireDashboard();
app.UseHangfireServer();

// Hangfire görevlerini yapýlandýrma
/*var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();*/   //Hangfire'da tekrar eden iþleri planlamak için kullanýlan bir arayüzdür. 
//ETicaret.Applicationn.Extentions.DependencyInjection.ConfigureHangfireJobs(recurringJobManager, app.Services);



using (var scope = app.Services.CreateScope())
{
    var hangfireService = scope.ServiceProvider.GetRequiredService<HangFireService>();
    hangfireService.ConfigureHangfireJobs();
}









app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");

//app.MapControllerRoute(
//name: "default",
//pattern: "{area=Admin}/{controller=Account}/{action=Login}/{id?}");
app.MapDefaultControllerRoute();


// Hangfire görevlerini yapýlandýrma
//using (var scope = app.Services.CreateScope())
//{
//    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
//    ConfigureHangfireJobs(recurringJobManager, scope.ServiceProvider);
//}








app.Run();
//static void ConfigureHangfireJobs(IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
//{
//    var backgroundService = serviceProvider.GetRequiredService<CampaignBackgroundService>();
//    recurringJobManager.AddOrUpdate("deactivate-expired-campaigns",
//        () => backgroundService.DeactivateExpiredCampaigns(), Cron.Minutely);
//}
