using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.UI.Controllers
{
    public class BaseController : Controller
    {
        protected INotyfService notyfService => HttpContext.RequestServices.GetService(typeof(INotyfService)) as INotyfService;
        protected void NotifySucces(string messages)
        {
            notyfService.Success(messages);
        }

        protected void NotifyError(string messages)
        {
            notyfService.Error(messages);
        }

    }
}
