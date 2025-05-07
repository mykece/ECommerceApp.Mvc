using ETicaret.Applicationn.DTOs.SizeTypeDTOs;
using ETicaret.Applicationn.Services.SizeTypeServices;
using ETicaret.UI.Areas.Admin.Models.SizeTypesVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class SizeTypeController : AdminBaseController
    {
        private readonly ISizeTypeService _sizeTypeService;
        private readonly IStringLocalizer<ModelResource> _stringLocalizer;

        public SizeTypeController(ISizeTypeService sizeTypeService, IStringLocalizer<ModelResource> stringLocalizer)
        {
            _sizeTypeService = sizeTypeService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _sizeTypeService.GetAllAsync();
            var sizeTypeListVMs = result.Data.Adapt<List<SizeTypeListVM>>();
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Size types could not be found!"]);

                return View(sizeTypeListVMs);
            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Size types listed succesfully!"]);

            return View(sizeTypeListVMs);
        }

        public async Task<IActionResult> Create()
        {
            //return View();
            var model = new SizeTypeCreateVM();
            return PartialView("_SizeTypeCreatePartial", model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SizeTypeCreateVM sizeTypeCreateVM)
        {
            var result = await _sizeTypeService.CreateAsync(sizeTypeCreateVM.Adapt<SizeTypeCreateDTO>());
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Size type already added!"]);

                return View(sizeTypeCreateVM);
            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Size Type added successfully!"]);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete (Guid id)
        {
            var result = await _sizeTypeService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Delete failed!"]);
                return Json(new { success = false, message = result.Messages });
            }
            NotifySucces(_stringLocalizer["Successfully deleted!"]);
            return Json(new { success = true });
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _sizeTypeService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["No data found to update!"]);
                RedirectToAction("Index");
            }
            var sizeTypeUpdateVM = result.Data.Adapt<SizeTypeUpdateVM>();
            //return View(sizeTypeUpdateVM);
            return PartialView("_SizeTypeUpdatePartial", sizeTypeUpdateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SizeTypeUpdateVM sizeTypeUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                NotifyError(_stringLocalizer["Please fill in all fields as requested!"]);
                return View(sizeTypeUpdateVM);
            }
            var result = await _sizeTypeService.UpdateAsync(sizeTypeUpdateVM.Adapt<SizeTypeUpdateDTO>());
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Size type could not be updated!"]);
            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Size type updated successfully!"]);
            return RedirectToAction("Index");
        }
    }
}
