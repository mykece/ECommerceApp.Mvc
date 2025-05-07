using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.HangFire;

public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        //sadece yetkili kullanıcıların erişimine izin veriyoruz
        var httpContext = context.GetHttpContext();

        // kullanıcının kimlik doğrulamasını ve belirli bir rolü kontrol etmek
        return httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole("Admin");
    }
}
