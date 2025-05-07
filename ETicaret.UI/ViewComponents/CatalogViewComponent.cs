using ETicaret.Applicationn.Services.CategoryServices;
using ETicaret.Domain.Enums;
using ETicaret.UI.Models.CatalogVMs;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.UI.ViewComponents
{
    public class CatalogViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CatalogViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
       
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genders = new[] { Gender.Man, Gender.Woman, Gender.Unisex };
            var allCategories = new List<CatalogViewModel>();

            foreach (var gender in genders)
            {
                // Cinsiyete göre kategorileri getir
                var categoriesDto = await _categoryService.GetCategoriesByProductGenderAsync(gender);
                if (categoriesDto.IsSuccess && categoriesDto.Data != null)
                {
                    // Her cinsiyet için kategorileri ayrı ayrı ekliyoruz
                    var categoryVMs = categoriesDto.Data.Select(c => new CatalogViewModel
                    {
                        CategoryId = c.Id,
                        CategoryName = c.Name,
                        Gender = c.Gender.ToString(),
                        Image = c.Image
                    }).ToList();



                    allCategories.AddRange(categoryVMs);
                }
            }

            // Kategorileri bir kez eklemek için 
            var distinctCategories = allCategories
                .GroupBy(c => c.CategoryId)
                .Select(g => g.First())
                .ToList();

            return View(distinctCategories);
        }
    }
}

