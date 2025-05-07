using ETicaret.Applicationn.DTOs.AdminDTOs;
using ETicaret.Applicationn.DTOs.CategorySizeTypeDTOs;
using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Applicationn.DTOs.ProductDTOs;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Enums;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using ETicaret.Infrastructure.Repositories.CategorySizeTypeProductRepositories;
using ETicaret.Infrastructure.Repositories.CategorySizeTypeRepositories;
using ETicaret.Infrastructure.Repositories.ProductRepositories;
using ETicaret.Infrastructure.Repositories.SizeRepositories;
using ETicaret.Infrastructure.Repositories.SizeTypeRepositories;
using MailKit.Search;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Linq;
using System.Linq.Expressions;

namespace ETicaret.Applicationn.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategorySizeTypeProductRepository _categorySizeTypeProductRepository;
        private readonly ICategorySizeTypeRepository _categorySizeTypeRepostory;
        private readonly ISizeTypeRepository _sizeTypeRepository;
        private readonly ISizeRepository _sizeRepository;
        public ProductService(IProductRepository productRepository, ICategorySizeTypeProductRepository categorySizeTypeProductRepository, ICategorySizeTypeRepository categorySizeTypeRepostory, ISizeTypeRepository sizeTypeRepository, ISizeRepository sizeRepository)
        {
            _productRepository = productRepository;
            _categorySizeTypeProductRepository = categorySizeTypeProductRepository;
            _categorySizeTypeRepostory = categorySizeTypeRepostory;
            _sizeTypeRepository = sizeTypeRepository;
            _sizeRepository = sizeRepository;
        }

        public async Task<IDataResult<ProductDTO>> CreateAsync(ProductCreateDTO productCreateDTO)
        {

            DataResult<ProductDTO> result = new ErrorDataResult<ProductDTO>();

            var strategy = await _productRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _productRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var product = await _productRepository.AddAsync(productCreateDTO.Adapt<Product>());
                    if (product == null)
                    {
                        result = new ErrorDataResult<ProductDTO>("Product eklenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }


                    var categorySizeTypeProduductDTOs = productCreateDTO.CategorySizeTypeProductCreateDTOs;
                    if (categorySizeTypeProduductDTOs == null)
                    {
                        result = new ErrorDataResult<ProductDTO>("Product eklenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }
                    var categoryIds = productCreateDTO.CategoryIds;
                    if (categoryIds == null)
                    {
                        result = new ErrorDataResult<ProductDTO>("Product eklenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }
                    var size = await _sizeRepository.GetByIdAsync(productCreateDTO.CategorySizeTypeProductCreateDTOs[0].SizeId);
                    if (size == null)
                    {
                        result = new ErrorDataResult<ProductDTO>("Product eklenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }
                    var selectedCategorySizeTypes = new List<CategorySizeType>();
                    foreach (var item in categoryIds)
                    {
                        var categorySizeTypes = await _categorySizeTypeRepostory.GetAsync(x => x.SizeTypeId == size.SizeTypeId && x.CategoryId == item);
                        selectedCategorySizeTypes.Add(categorySizeTypes);
                    }
                    if (selectedCategorySizeTypes == null)
                    {
                        result = new ErrorDataResult<ProductDTO>("Product eklenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }

                    var newCategorySizeTypeProductList = productCreateDTO.CategorySizeTypeProductCreateDTOs
                        .GroupBy(x => x.SizeId)
                        .Select(g => g.First())
                        .ToList();

                    foreach (var item in newCategorySizeTypeProductList)
                    {
                        foreach (var categorySizeType in selectedCategorySizeTypes)
                        {
                            var newCategorySizeTypeProduct = new CategorySizeTypeProduct
                            {
                                CategorySizeTypeId = categorySizeType.Id,
                                ProductId = product.Id,
                                SizeId = item.SizeId,
                                Quantity = item.Quantity // or other properties if any
                            };

                            await _categorySizeTypeProductRepository.AddAsync(newCategorySizeTypeProduct);
                        }
                    }

                    await _productRepository.SaveChangesAsync();
                    await _categorySizeTypeProductRepository.SaveChangesAsync();
                    result = new SuccessDataResult<ProductDTO>(product.Adapt<ProductDTO>(), "Product başarı ile eklendi");
                    transactionScope.Commit();

                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<ProductDTO>("Product could not be added!" + ex.Message);
                    transactionScope.Rollback();

                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {

            DataResult<ProductDTO> result = new ErrorDataResult<ProductDTO>();

            var strategy = await _productRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _productRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var product = await _productRepository.GetByIdAsync(id);
                    if (product == null)
                    {
                        result = new ErrorDataResult<ProductDTO>("Silinecek Product Bulunamadı!");
                        transactionScope.Rollback();
                        return;
                    }
                    await _productRepository.DeleteAsync(product);
                    var categorySizeTypeProducts = await _categorySizeTypeProductRepository.GetAllAsync(x => x.ProductId == id);
                    foreach (var item in categorySizeTypeProducts)
                    {
                        await _categorySizeTypeProductRepository.DeleteAsync(item);
                    }
                    await _categorySizeTypeProductRepository.SaveChangesAsync();
                    result = new SuccessDataResult<ProductDTO>("Product Silme başarılı");
                    transactionScope.Commit();

                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<ProductDTO>("Product could not be deleted!" + ex.Message);
                    transactionScope.Rollback();

                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }

        public async Task<IDataResult<List<ProductListDTO>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync(x => x.Status != Status.Deleted);
            var productListDtos = products.Adapt<List<ProductListDTO>>();

            if (products.Count() <= 0)
            {
                return new ErrorDataResult<List<ProductListDTO>>(productListDtos, "Listenecek Ürün Bulunamadı");
            }

            foreach (var productDto in productListDtos)
            {
                var categorySizeTypeProducts = await _categorySizeTypeProductRepository.GetAllAsync(cstp => cstp.ProductId == productDto.Id && cstp.Status != Status.Deleted);
                double totalQuantity = 0;
                var quantityDetails = new List<ProductQuantityDetailDTO>();
                var categoryNames = new List<string>();


                foreach (var cstp in categorySizeTypeProducts)
                {
                    totalQuantity += cstp.Quantity;
                    var size = await _sizeRepository.GetByIdAsync(cstp.SizeId);
                    if (size != null)
                    {
                        quantityDetails.Add(new ProductQuantityDetailDTO
                        {
                            SizeName = size.SizeName, // Size entity'nizde Name property olduğunu varsayıyorum
                            Quantity = cstp.Quantity
                        });
                    }

                    //Category Name için eklendi 
                    var category = await _categorySizeTypeRepostory.GetAsync(x => x.Id == cstp.CategorySizeTypeId);
                    if (category != null && !categoryNames.Contains(category.Category.Name))
                    {
                        categoryNames.Add(category.Category.Name);
                    }

                }
                productDto.Quantity = totalQuantity;
                productDto.QuantityDetails = quantityDetails;
                productDto.CategoryNames = categoryNames;
                
            }

            return new SuccessDataResult<List<ProductListDTO>>(productListDtos, "Ürün Listeleme Başarılı");
        }


        public async Task<IDataResult<ProductDTO>> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return new ErrorDataResult<ProductDTO>("Gösterilecek Ürün Bulunamadı");
            }

            var productDto = product.Adapt<ProductDTO>();

            // Ara tablodan Quantity ve QuantityDetails doldurma
            var categorySizeTypeProducts = await _categorySizeTypeProductRepository.GetByProductIdAsync(product.Id);
            double totalQuantity = 0;
            var quantityDetails = new List<ProductQuantityDetailDTO>();

            foreach (var cstp in categorySizeTypeProducts)
            {
                totalQuantity += cstp.Quantity;
                var size = await _sizeRepository.GetByIdAsync(cstp.SizeId);
                if (size != null)
                {
                    quantityDetails.Add(new ProductQuantityDetailDTO
                    {
                        SizeName = size.SizeName,
                        Quantity = cstp.Quantity
                    });
                }
            }
            // Kategori ve boyut ID'lerini ayıkla
            var categoryIds = categorySizeTypeProducts.Select(cstp => cstp.CategorySizeType.CategoryId).Distinct().ToList();
            var sizeIds = categorySizeTypeProducts.Select(cstp => cstp.SizeId).Distinct().ToList();
            productDto.CategoryIds = categoryIds;
            productDto.SizeIds = sizeIds;

            productDto.Quantity = totalQuantity;
            productDto.QuantityDetails = quantityDetails;

            return new SuccessDataResult<ProductDTO>(productDto, "Ürün Gösterme Başarılı");
        }

        public async Task<IDataResult<List<CategorySizeTypeProductListDTO>>> GetByProductId(Guid productId)
        {
            var selectedCategorySizeTypes = await _categorySizeTypeProductRepository.GetAllAsync(x => x.ProductId == productId);
            var selectedCategorySizeTypesListDTOs = selectedCategorySizeTypes.Adapt<List<CategorySizeTypeProductListDTO>>();
            if (selectedCategorySizeTypes.Count() <= 0)
            {
                return new ErrorDataResult<List<CategorySizeTypeProductListDTO>>(selectedCategorySizeTypesListDTOs, "Listenecek Varyasyonlu ürün Bulunamadı");

            }
            return new SuccessDataResult<List<CategorySizeTypeProductListDTO>>(selectedCategorySizeTypesListDTOs, "Varyasyon Listeleme Başarılı ");
        }

        public async Task<IDataResult<CategorySizeTypeDTO>> GetCategorySizeType(Guid sizeId, Guid categoryId)
        {

            var size = await _sizeRepository.GetByIdAsync(sizeId);
            if (size == null)
            {
                return new ErrorDataResult<CategorySizeTypeDTO>("Size Bulunamadı");
            }
            var categorySizeType = await _categorySizeTypeRepostory.GetAsync(x => x.SizeTypeId == size.SizeTypeId && x.CategoryId == categoryId);
            if (categorySizeType == null)
            {
                return new ErrorDataResult<CategorySizeTypeDTO>("CategorySizeType Bulunamadı");
            }
            return new SuccessDataResult<CategorySizeTypeDTO>(categorySizeType.Adapt<CategorySizeTypeDTO>(), "CategorySizeType başarıyla bulundu.");
        }

        public async Task<IDataResult<List<ProductListDTO>>> GetLatestProductsAsync(int count)
        {
            var products = await _productRepository.GetLatestProductsAsync(count);

            if (products == null || products.Count == 0)
            {
                return new ErrorDataResult<List<ProductListDTO>>(new List<ProductListDTO>(), "Gösterilecek ürün bulunamadı.");
            }

            var productListDtos = products.Adapt<List<ProductListDTO>>();

            return new SuccessDataResult<List<ProductListDTO>>(productListDtos, "Product listeleme başarılı.");
        }

        public async Task<IDataResult<ProductDTO>> UpdateAsync(ProductUpdateDTO productUpdateDTO)
        {
            DataResult<ProductDTO> result = new ErrorDataResult<ProductDTO>("Product güncellenirken bir hata oluştu.");

            var strategy = await _productRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _productRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var existingProduct = await _productRepository.GetByIdAsync(productUpdateDTO.Id);
                    if (existingProduct == null)
                    {
                        result = new ErrorDataResult<ProductDTO>("Product bulunamadı.");
                        return;
                    }

                    existingProduct.Name = productUpdateDTO.Name;
                    existingProduct.Description = productUpdateDTO.Description;
                    existingProduct.UnitPrice = productUpdateDTO.UnitPrice;
                    existingProduct.Image = productUpdateDTO.Image;
                    existingProduct.Gender = productUpdateDTO.Gender;

                    var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
                    if (updatedProduct == null)
                    {
                        result = new ErrorDataResult<ProductDTO>("Product güncellenirken bir hata oluştu.");
                        transactionScope.Rollback();
                        return;
                    }

                    var deletedCategorySizeTypeProducts = await _categorySizeTypeProductRepository.GetAllAsync(x => x.ProductId == productUpdateDTO.Id);
                    foreach (var item in deletedCategorySizeTypeProducts)
                    {
                        if (!productUpdateDTO.CategorySizeTypeProductUpdateDTOs.Any(x => x.Id == item.Id))
                        {
                            await _categorySizeTypeProductRepository.DeleteAsync(item);
                        }
                        continue;
                        
                    }

                    foreach (var item in productUpdateDTO.CategorySizeTypeProductUpdateDTOs)
                    {
                        if (item.Id == Guid.Empty || item.Id == null)  // Nullable Guid kontrolü- Yeni Eklenen CategorySizeTypeProduct'ları tespit etmek için.
                        {
                            item.ProductId = existingProduct.Id;

                            var categorySizeTypeResult = await GetCategorySizeType(item.SizeId, item.CategoryId);
                            if (categorySizeTypeResult == null)
                            {
                                result = new ErrorDataResult<ProductDTO>("CategorySizeType bilgisi alınamadı.");
                                transactionScope.Rollback();
                                return;
                            }

                            item.CategorySizeTypeId = categorySizeTypeResult.Data.Id;
                            var newCategorySizeTypeProduct = item.Adapt<CategorySizeTypeProduct>();
                            var addedCategorySizeTypeProduct = await _categorySizeTypeProductRepository.AddAsync(newCategorySizeTypeProduct);
                            if (addedCategorySizeTypeProduct == null)
                            {
                                result = new ErrorDataResult<ProductDTO>("CategorySizeTypeProduct eklenirken bir hata oluştu.");
                                transactionScope.Rollback();
                                return;
                            }
                        }
                        else
                        {
                            if (item.Id.HasValue) // Nullable Guid kontrolü
                            {
                                item.ProductId = existingProduct.Id;
                                var existingCategorySizeTypeProduct = await _categorySizeTypeProductRepository.GetByIdAsync(item.Id.Value);
                                if (existingCategorySizeTypeProduct == null)
                                {
                                    result = new ErrorDataResult<ProductDTO>("CategorySizeTypeProduct bulunamadı.");
                                    transactionScope.Rollback();
                                    return;
                                }
                                var selectedSize = await _sizeRepository.GetByIdAsync(item.SizeId);
                                var selectedCategorySizeType = await _categorySizeTypeRepostory.GetAllAsync(x => x.CategoryId == item.CategoryId && x.SizeTypeId== selectedSize.SizeTypeId);
                                existingCategorySizeTypeProduct.SizeId = item.SizeId;
                                existingCategorySizeTypeProduct.CategorySizeTypeId = selectedCategorySizeType.First().Id;
                                existingCategorySizeTypeProduct.Quantity = item.Quantity;

                                var updatedCategorySizeTypeProduct = await _categorySizeTypeProductRepository.UpdateAsync(existingCategorySizeTypeProduct);
                                if (updatedCategorySizeTypeProduct == null)
                                {
                                    result = new ErrorDataResult<ProductDTO>("CategorySizeTypeProduct güncellenirken bir hata oluştu.");
                                    transactionScope.Rollback();
                                    return;
                                }
                            }
                        }
                    }

                    await _productRepository.SaveChangesAsync();
                    await _categorySizeTypeProductRepository.SaveChangesAsync();

                    result = new SuccessDataResult<ProductDTO>("Product güncelleme başarılı");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<ProductDTO>($"Product güncellenirken bir hata oluştu: {ex.Message}");
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });

            return result;
        }

        public async Task<IDataResult<List<ProductListDTO>>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
            if (products == null || !products.Any())
            {
                return new ErrorDataResult<List<ProductListDTO>>("Belirtilen kategoriye ait ürün bulunamadı.");
            }

            var productDTOs = products.Adapt<List<ProductListDTO>>();
            return new SuccessDataResult<List<ProductListDTO>>(productDTOs, "Ürünler başarıyla getirildi.");
        }

        public async Task<IDataResult<CategorySizeTypeProductDTO>> GetCategorySizeTypeProduct(Guid categorySizeTypeProductId)
        {
            var categorySizeTypeProduct = await _categorySizeTypeProductRepository.GetByIdAsync(categorySizeTypeProductId);
            if (categorySizeTypeProduct == null)
            {
                return new ErrorDataResult<CategorySizeTypeProductDTO>("CategorySizeTypeProduct bulunamadı!");

            }
            return new SuccessDataResult<CategorySizeTypeProductDTO>(categorySizeTypeProduct.Adapt<CategorySizeTypeProductDTO>(), "CategorySizeTypeProduct başarıyla bulundu");
        }

        public async Task<int> GetProductCountByGenderAsync(Gender? gender = null)
        {
            if (gender == null)
            {
                return await _productRepository.CountAsync(p => p.Status != Status.Deleted);
            }
            return await _productRepository.CountAsync(p => p.Gender == gender.Value && p.Status != Status.Deleted);
        }
    }
}
