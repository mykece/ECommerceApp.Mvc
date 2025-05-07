using ETicaret.Applicationn.BackgroundServices;
using ETicaret.Applicationn.Extentions;
using ETicaret.Applicationn.Services.HangFire;
using ETicaret.Infrastructure.Extentions;
using ETicaret.UI.Extentions;
using FluentValidation.AspNetCore;
using Hangfire;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


/*builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());*/ //Eklenen t�m validators'leri otomatik olarak sisteme ekler.

//A�a��daki kodda DataAnnotations Validation devre d��� b�rak�larak sadece fluent validation'da belirledi�imiz kurallar �al���yor.
//Bunu yapma sebebimiz, empty/null de�er girdi�imizde DataAnnotation's validasyonu devreye giriyor, fluent validation'da belirledi�im null olamaz kural�n� pas ge�iyor.
//Dilenirse yukar�daki kod aktif hale getirilip a�a��daki kod yoruma al�narak, data annotation ve fluent validation kurallar� birlikte kullan�labilir.
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv =>
    {
        fv.DisableDataAnnotationsValidation = true;
        fv.RegisterValidatorsFromAssemblyContaining<Program>(); //program.cs 'in oldu�u t�m assembly'deki validator kurallar�n� aktif hale getiriyor.
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


// Hangfire Dashboard ve Server eklendi. Hangfire Authorization  i�lemi ,sayfaya sadece admin ula�abilir.
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new MyAuthorizationFilter() }
});
//app.UseHangfireDashboard();
app.UseHangfireServer();

// Hangfire g�revlerini yap�land�rma
/*var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();*/   //Hangfire'da tekrar eden i�leri planlamak i�in kullan�lan bir aray�zd�r. 
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


// Hangfire g�revlerini yap�land�rma
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
