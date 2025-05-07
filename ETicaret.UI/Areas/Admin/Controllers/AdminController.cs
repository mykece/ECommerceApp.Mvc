using ETicaret.Applicationn.DTOs.AdminDTOs;
using ETicaret.Applicationn.Services.AdminService;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.UI.Areas.Admin.Models.AdminVMs;
using ETicaret.UI.Extentions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class AdminController : AdminBaseController
    {
        private readonly IAdminService _adminService;
        private readonly IStringLocalizer<ModelResource> _stringLocalizer;

        public AdminController(IAdminService adminService, IStringLocalizer<ModelResource> stringLocalizer)
        {
            _adminService = adminService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _adminService.GetAllAsync();
            var adminVMs = result.Data.Adapt<List<AdminListVM>>();
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Admin could not be found!"]);
                return View(adminVMs);
            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Admin  listed succesfully!"]);
            return View(adminVMs);
        }

        public async Task<IActionResult> Create()
        {
            var model = new AdminCreateVM();
            return PartialView("_CreatePartial", model);

        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCreateVM adminCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(adminCreateVM);
            }

            var adminCreateDto = adminCreateVM.Adapt<AdminCreateDTO>();

            if (adminCreateVM.NewImage != null && adminCreateVM.NewImage.Length > 0)
            {
                adminCreateDto.Image = await adminCreateVM.NewImage.StringToByteArrayAsync();
            }

            var result = await _adminService.CreateAsync(adminCreateDto);

            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Admin already added!"]);

                return RedirectToAction("Index");
            }

            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Admin added successfully!"]);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _adminService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                
                return View("Error");
            }
            var adminUpdateVM = result.Data.Adapt<AdminUpdateVM>();

            // Mevcut resim veritabanından alınıyor ve ViewBag'e aktarılıyor
            //ViewBag.ExistingImage = result.Data.Image;
            adminUpdateVM.ExistingImage = result.Data.Image;

            return PartialView("_UpdatePartial", adminUpdateVM);
        }



        [HttpPost]
        public async Task<IActionResult> Update(AdminUpdateVM adminUpdateVM)
        {
            var admin = await _adminService.GetByIdAsync(adminUpdateVM.Id);
            if(!ModelState.IsValid)
            {
                return View(adminUpdateVM);
            }
            var adminDto = adminUpdateVM.Adapt<AdminUpdateDTO>();
            if (adminUpdateVM.NewImage != null && adminUpdateVM.NewImage.Length > 0)
            {
                adminDto.Image = await adminUpdateVM.NewImage.StringToByteArrayAsync();
            }
            else
            {
                // Mevcut resmi kullan
                adminDto.Image = admin.Data.Image;
            }

            var result = await _adminService.UpdateAsync(adminDto);
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Admin could not be updated!"]);

                return RedirectToAction("Index");
            }

            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Admin updated successfully!"]);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _adminService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Delete failed!"]);
                return Json(new { success = false, message = result.Messages });
            }
            NotifySucces(_stringLocalizer["Successfully deleted!"]);
            return Json(new { success = true });
        }
    }   
}
