using ETicaret.Applicationn.Services.CategoryServices;
using ETicaret.Domain.Enums;
using ETicaret.UI.Areas.Admin.Models.CategoryVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.UI.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {

        private readonly ICategoryService _categoryService;

        public CategoryViewComponent(ICategoryService categoryService)
        {
            
            _categoryService = categoryService;
        }



        //public async Task<IViewComponentResult> InvokeAsync()
        //{

        //    var categoriesDto = (await _categoryService.GetAllAsync()).Data;
        //    var campaignListVMs = categoriesDto.Adapt<List<CategoryListVM>>();


        //    return View(campaignListVMs);
        //}
        public async Task<IViewComponentResult> InvokeAsync(Gender gender)
        {
            var categoriesDto = (await _categoryService.GetCategoriesByProductGenderAsync(gender)).Data;
            var categoryListVMs = categoriesDto.Adapt<List<CategoryListVM>>();
            return View(categoryListVMs);
        }
    }
}
