using ETicaret.Applicationn.DTOs.SizeDTOs;
using ETicaret.Applicationn.Services.SizeServices;
using ETicaret.Applicationn.Services.SizeTypeServices;
using ETicaret.UI.Areas.Admin.Models.SizeVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class SizeController : AdminBaseController
    {
        private readonly ISizeServices _sizeService;
        private readonly ISizeTypeService _sizeTypeService;
        private readonly IStringLocalizer<ModelResource> _stringLocalizer;


        public SizeController(ISizeServices sizeService, ISizeTypeService sizeTypeService, IStringLocalizer<ModelResource> stringLocalizer)
        {
            _sizeService = sizeService;
            _sizeTypeService = sizeTypeService;
            _stringLocalizer = stringLocalizer;
        }


        public async Task<IActionResult> Index()
        {
            var result = await _sizeService.GetAllAsync();
            var sizeListVMs = result.Data.Adapt<List<SizeListVM>>();
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Size could not be found!"]);

                return View(sizeListVMs);
            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Size listed succesfully!"]);

            return View(sizeListVMs);
        }

        public async Task<IActionResult> Create()
        {
            var sizeCreateVM = new SizeCreateVM();
            sizeCreateVM.SizeTypes = await GetSizeTypes();
            //return View(sizeCreateVM);
            return PartialView("_SizeCreatePartial", sizeCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SizeCreateVM sizeCreateVM)
        {
            sizeCreateVM.SizeTypes = await GetSizeTypes();
            var result = await _sizeService.CreateAsync(sizeCreateVM.Adapt<SizeCreateDTO>());
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Size already added!"]);

                return View(sizeCreateVM);
            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Size added successfully!"]);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _sizeService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["No data found to update!"]);
                RedirectToAction("Index");

            }
            var sizeUpdateVM = result.Data.Adapt<SizeUpdateVM>();
            sizeUpdateVM.SizeTypes = await GetSizeTypes();
            //return View(sizeUpdateVM);
            return PartialView("_SizeUpdatePartial", sizeUpdateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SizeUpdateVM sizeUpdateVM)
        {
            sizeUpdateVM.SizeTypes = await GetSizeTypes();
            if (!ModelState.IsValid)
            {
                return View(sizeUpdateVM);
            }
            var result = await _sizeService.UpdateAsync(sizeUpdateVM.Adapt<SizeUpdateDTO>());
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Size could not be updated!"]);

            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Size updated successfully!"]);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _sizeService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Delete failed!"]);
                return Json(new { success = false, message = result.Messages });
            }
            NotifySucces(_stringLocalizer["Successfully deleted!"]);
            return Json(new { success = true });
        }

        private async Task<SelectList> GetSizeTypes(Guid? sizeTypeId = null)
        {
            var sizeTypes = (await _sizeTypeService.GetAllAsync()).Data;
            return new SelectList(sizeTypes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.SizeTypeName,
                Selected = x.Id == (sizeTypeId != null ? sizeTypeId.Value : sizeTypeId)
            }).OrderBy(x => x.Text), "Value", "Text");
        }


    }
}
