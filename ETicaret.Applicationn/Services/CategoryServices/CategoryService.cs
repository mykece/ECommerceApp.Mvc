using ETicaret.Applicationn.DTOs.CategoryDTOs;
using ETicaret.Applicationn.DTOs.CategorySizeTypeDTOs;
using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Applicationn.DTOs.ProductDTOs;
using ETicaret.Applicationn.DTOs.SizeTypeDTOs;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Enums;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using ETicaret.Infrastructure.Repositories.CategoryRepositories;
using ETicaret.Infrastructure.Repositories.CategorySizeTypeProductRepositories;
using ETicaret.Infrastructure.Repositories.CategorySizeTypeRepositories;
using ETicaret.Infrastructure.Repositories.ProductRepositories;
using ETicaret.Infrastructure.Repositories.SizeTypeRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategorySizeTypeRepository _categorySizeTypeRepository;
        private readonly ISizeTypeRepository _sizeTypeRepository;
        private readonly ICategorySizeTypeProductRepository _categorySizeTypeProductRepository;
        private readonly IProductRepository _productRepository;
        public CategoryService(ICategoryRepository categoryRepository, ICategorySizeTypeRepository categorySizeTypeRepository, ISizeTypeRepository sizeTypeRepository, ICategorySizeTypeProductRepository categorySizeTypeProductRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _categorySizeTypeRepository = categorySizeTypeRepository;
            _sizeTypeRepository = sizeTypeRepository;
            _categorySizeTypeProductRepository = categorySizeTypeProductRepository;
            _productRepository = productRepository;
        }
        //Multi-Select için create değişikliği (foreach eklendi)
        public async Task<IDataResult<CategoryDTO>> CreateAsync(CategoryCreateDTO categoryCreateDTO)
        {
            DataResult<CategoryDTO> result = new ErrorDataResult<CategoryDTO>();

            var strategy = await _categoryRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _categoryRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var category = await _categoryRepository.AddAsync(categoryCreateDTO.Adapt<Category>());
                    if (category == null)
                    {
                        result = new ErrorDataResult<CategoryDTO>("Category eklenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }

                    foreach (var sizeTypeId in categoryCreateDTO.SizeTypeIds)
                    {
                        var categorySizeType = new CategorySizeTypeCreateDTO()
                        {
                            CategoryId = category.Id,
                            SizeTypeId = sizeTypeId
                        };

                        await _categorySizeTypeRepository.AddAsync(categorySizeType.Adapt<CategorySizeType>());
                    }
                    await _categorySizeTypeRepository.SaveChangesAsync();

                    result = new SuccessDataResult<CategoryDTO>("Category ekleme başarılı");
                    transactionScope.Commit();


                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<CategoryDTO>("Category could not be added!" + ex.Message);
                    transactionScope.Rollback();

                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }

        //public async Task<IDataResult<CategoryDTO>> CreateAsync(CategoryCreateDTO categoryCreateDTO)
        //{
        //    DataResult<CategoryDTO> result = new ErrorDataResult<CategoryDTO>();

        //    var strategy = await _categoryRepository.CreateExecutionStrategy();
        //    await strategy.ExecuteAsync(async () =>
        //    {
        //        var transactionScope = await _categoryRepository.BeginTransactionAsync().ConfigureAwait(false);
        //        try
        //        {
        //            var category = await _categoryRepository.AddAsync(categoryCreateDTO.Adapt<Category>());
        //            if (category == null)
        //            {
        //                result = new ErrorDataResult<CategoryDTO>("Category eklenirken bir hata oluştu.");
        //                transactionScope.Rollback();
        //                return;
        //            }
        //            // burdan devam edilecek

        //            var categorySizeType = new CategorySizeTypeCreateDTO()
        //            {
        //                CategoryId = category.Id,
        //                SizeTypeId = categoryCreateDTO.SizeTypeId
        //            };


        //            await _categorySizeTypeRepository.AddAsync(categorySizeType.Adapt<CategorySizeType>());
        //            await _categorySizeTypeRepository.SaveChangesAsync();

        //            result = new SuccessDataResult<CategoryDTO>("Category ekleme başarılı");
        //            transactionScope.Commit();

        //        }
        //        catch (Exception ex)
        //        {
        //            result = new ErrorDataResult<CategoryDTO>("Category could not be added!" + ex.Message);
        //            transactionScope.Rollback();

        //        }
        //        finally
        //        {
        //            transactionScope.Dispose();
        //        }
        //    });
        //    return result;
        //}

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DataResult<CategoryDTO> result = new ErrorDataResult<CategoryDTO>();

            var strategy = await _categoryRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _categoryRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var category = await _categoryRepository.GetByIdAsync(id);
                    if (category == null)
                    {
                        result = new ErrorDataResult<CategoryDTO>("Silinecek Category Bulunamadı!");
                        transactionScope.Rollback();
                        return;
                    }
                    await _categoryRepository.DeleteAsync(category);

                    var categorySizeType = await _categorySizeTypeRepository.GetAllAsync(x => x.CategoryId == id);
                    foreach (var item in categorySizeType)
                    {
                        await _categorySizeTypeRepository.DeleteAsync(item);
                    }
                    await _categorySizeTypeRepository.SaveChangesAsync();
                    result = new SuccessDataResult<CategoryDTO>("Category Silme başarılı");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<CategoryDTO>("Category could not be deleted!" + ex.Message);
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;

        }

        //public async Task<IDataResult<List<CategoryListDTO>>> GetAllAsync()
        //{
        //    var categories = await _categoryRepository.GetAllAsync();
        //    var categoryListDTOs = categories.Adapt<List<CategoryListDTO>>();
        //    if (categories.Count() <= 0)
        //    {
        //        return new ErrorDataResult<List<CategoryListDTO>>(categoryListDTOs, "Categories could not be found!");

        //    }
        //    return new SuccessDataResult<List<CategoryListDTO>>(categoryListDTOs, "Categories listed succesfully! ");

        //}

        //Multi-Select için GetAll değişikliği (foreach eklendi)
        public async Task<IDataResult<List<CategoryListDTO>>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryListDTOs = new List<CategoryListDTO>();

            foreach (var category in categories) 
            {
                var categoryDTO = category.Adapt<CategoryListDTO>();

                var sizeTypes = await _categorySizeTypeRepository.GetAllAsync(x => x.CategoryId == category.Id);
                categoryDTO.SizeTypeIds = sizeTypes.Select(x => x.SizeTypeId).ToList();

                var sizeTypeNames = new List<string>();
                foreach (var sizeTypeId in categoryDTO.SizeTypeIds)
                {
                    var sizeType = await _sizeTypeRepository.GetByIdAsync(sizeTypeId);
                    if (sizeType != null)
                    {
                        sizeTypeNames.Add(sizeType.SizeTypeName);
                    }
                }

                categoryDTO.SizeTypeNames = sizeTypeNames;

                categoryListDTOs.Add(categoryDTO);
            }

            if (!categoryListDTOs.Any())
            {
                return new ErrorDataResult<List<CategoryListDTO>>(categoryListDTOs, "Categories could not be found!");
            }

            return new SuccessDataResult<List<CategoryListDTO>>(categoryListDTOs, "Categories listed successfully!");
        }

        public async Task<IDataResult<List<CategorySizeTypeProductListDTO>>> GetAllProductByCategoryId(Guid id)
        {
            var categorySizeTypes = await _categorySizeTypeRepository.GetAllAsync(x=>x.CategoryId == id);
            if (categorySizeTypes == null)
            {
                return new ErrorDataResult<List<CategorySizeTypeProductListDTO>>(new List<CategorySizeTypeProductListDTO>(), "Kategoriye ait ürün bulunamadı.");
            }
            var productList = new List<CategorySizeTypeProduct>();
            foreach(var item in categorySizeTypes)
            {
                var products = await _categorySizeTypeProductRepository.GetAllAsync(x => x.CategorySizeTypeId == item.Id);
                if(products != null)
                {
                    productList.AddRange(products);
                }
                continue;
            }
            if(productList == null)
            {
                return new ErrorDataResult<List<CategorySizeTypeProductListDTO>>(new List<CategorySizeTypeProductListDTO>(),"Kategoriye ait ürün bulunamadı.");
            }
            return new SuccessDataResult<List<CategorySizeTypeProductListDTO>>(productList.Adapt<List<CategorySizeTypeProductListDTO>>(),"Kategoriye ait ürünler listelendi!");

        }

        public async Task<IDataResult<CategoryDTO>> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
            {
                return new ErrorDataResult<CategoryDTO>("Category could not be found!");
            }
            var categoryDTO = category.Adapt<CategoryDTO>();
            return new SuccessDataResult<CategoryDTO>(categoryDTO, "Category found successfully!");
        }

        public async Task<IDataResult<List<CategoryListDTO>>> GetCategoriesByProductGenderAsync(Gender gender)
        {
            // Tüm ürünler
            var manProducts = await _productRepository.GetAllAsync(p => p.Gender == Gender.Man);
            var womanProducts = await _productRepository.GetAllAsync(p => p.Gender == Gender.Woman);
            var unisexProducts = await _productRepository.GetAllAsync(p => p.Gender == Gender.Unisex);

            // Ürünleri birleştir
            var products = manProducts.Concat(womanProducts).Concat(unisexProducts).ToList();

            if (products == null || !products.Any())
            {
                return new ErrorDataResult<List<CategoryListDTO>>("Ürün bulunamadı.");
            }

            // Filtrelenen ürünlerin kategorilerini al
            var categoryIds = products
                .SelectMany(p => p.CategorySizeTypeProducts.Select(cstp => cstp.CategorySizeType.CategoryId))
                .Distinct()
                .ToList();

            var categories = await _categoryRepository.GetAllAsync(c => categoryIds.Contains(c.Id));
            if (categories == null || !categories.Any())
            {
                return new ErrorDataResult<List<CategoryListDTO>>("Kategori bulunamadı.");
            }

            var categoryListDTOs = categories.Adapt<List<CategoryListDTO>>();
            foreach (var category in categoryListDTOs.ToList())
            {
                // İlgili cinsiyete sahip ürünleri kategoriye atama
                var relatedProducts = products
                    .Where(p => p.CategorySizeTypeProducts.Any(cstp => cstp.CategorySizeType.CategoryId == category.Id))
                    .ToList();

                if (relatedProducts.Any())
                {
                    category.Gender = relatedProducts.First().Gender;
                }
                else
                {
                    categoryListDTOs.Remove(category);
                }
            }

            return new SuccessDataResult<List<CategoryListDTO>>(categoryListDTOs, "Kategoriler başarıyla getirildi.");
        }

        public async Task<IDataResult<CategoryDTO>> GetCategoryByCategorySizeTypeId(Guid id)
        {
            var categorySizeTypeResult = await _categorySizeTypeRepository.GetByIdAsync(id);
            var category = await _categoryRepository.GetByIdAsync(categorySizeTypeResult.CategoryId);
            if (category is null) 
            {
                return new ErrorDataResult<CategoryDTO>("Kategori bulunamadı");
            }
            return new SuccessDataResult<CategoryDTO>(category.Adapt<CategoryDTO>(), "Kategori başarıyla bulundu.");
        }

        //Categoriye  ait Product Sayısı
        public async Task<int> GetProductCountByCategoryIdAsync(Guid categoryId)
        {
            return await _productRepository.CountAsync(p => p.CategorySizeTypeProducts.Any(c => c.CategorySizeType.CategoryId == categoryId));

        }

        public async Task<IDataResult<CategoryDTO>> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
            DataResult<CategoryDTO> result = new ErrorDataResult<CategoryDTO>();

            var strategy = await _categoryRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _categoryRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var category = await _categoryRepository.GetByIdAsync(categoryUpdateDTO.Id);
                    if (category == null)
                    {
                        result = new ErrorDataResult<CategoryDTO>("Güncellenecek Category Bulunamadı!");
                        transactionScope.Rollback();
                        return;
                    }
                    category.Description = categoryUpdateDTO.Description;
                    category.Name = categoryUpdateDTO.Name;
                    category.Image = categoryUpdateDTO.Image;
                    await _categoryRepository.UpdateAsync(category);

                    var existingCategorySizeTypes = await _categorySizeTypeRepository.GetAllAsync(x => x.CategoryId == categoryUpdateDTO.Id);
                    foreach (var item in existingCategorySizeTypes)
                    {
                        await _categorySizeTypeRepository.DeleteAsync(item);
                    }

                    foreach (var sizeTypeId in categoryUpdateDTO.SizeTypeIds)
                    {
                        var categorySizeType = new CategorySizeType()
                        {
                            CategoryId = category.Id,
                            SizeTypeId = sizeTypeId
                        };
                        await _categorySizeTypeRepository.AddAsync(categorySizeType);
                    }

                    await _categorySizeTypeRepository.SaveChangesAsync();

                    result = new SuccessDataResult<CategoryDTO>("Category Güncelleme başarılı");
                    transactionScope.Commit();


                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<CategoryDTO>("Category could not be updated!" + ex.Message);
                    transactionScope.Rollback();

                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }

        //public async Task<IDataResult<CategoryDTO>> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO)
        //{
        //    DataResult<CategoryDTO> result = new ErrorDataResult<CategoryDTO>();

        //    var strategy = await _categoryRepository.CreateExecutionStrategy();
        //    await strategy.ExecuteAsync(async () =>
        //    {
        //        var transactionScope = await _categoryRepository.BeginTransactionAsync().ConfigureAwait(false);
        //        try
        //        {

        //            // MApster ile adaptleyince Update edildiğinde Verinin Createdby'ı null geliyor ve güncelleme yapmıyor.
        //            var category = await _categoryRepository.GetByIdAsync(categoryUpdateDTO.Id);
        //            category.Description = categoryUpdateDTO.Description;
        //            category.Name = categoryUpdateDTO.Name;
        //            await _categoryRepository.UpdateAsync(category);
        //            if (category == null)
        //            {
        //                result = new ErrorDataResult<CategoryDTO>("Güncellenecek Category Bulunamadı!");
        //                transactionScope.Rollback();
        //                return;
        //            }
        //            var categorySizeTypes = await _categorySizeTypeRepository.GetAllAsync(x => x.CategoryId == categoryUpdateDTO.Id);
        //            foreach (var item in categorySizeTypes)
        //            {

        //                item.SizeTypeId = categoryUpdateDTO.SizeTypeId;
        //                await _categorySizeTypeRepository.UpdateAsync(item);
        //                await _categorySizeTypeRepository.SaveChangesAsync();
        //            }
        //            result = new SuccessDataResult<CategoryDTO>("Category Güncelleme başarılı");
        //            transactionScope.Commit();

        //        }
        //        catch (Exception ex)
        //        {
        //            result = new ErrorDataResult<CategoryDTO>("Category could not be updated!" + ex.Message);
        //            transactionScope.Rollback();

        //        }
        //        finally
        //        {
        //            transactionScope.Dispose();
        //        }
        //    });
        //    return result;
        //}

       

    }
}
