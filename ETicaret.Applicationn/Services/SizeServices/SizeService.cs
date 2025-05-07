using ETicaret.Applicationn.DTOs.ProductDTOs;
using ETicaret.Applicationn.DTOs.SizeDTOs;
using ETicaret.Applicationn.DTOs.SizeTypeDTOs;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using ETicaret.Infrastructure.Repositories.CategorySizeTypeRepositories;
using ETicaret.Infrastructure.Repositories.SizeRepositories;
using ETicaret.Infrastructure.Repositories.SizeTypeRepositories;
using Mapster;


namespace ETicaret.Applicationn.Services.SizeServices
{
    public class SizeService : ISizeServices
    {
        private readonly ISizeRepository _sizeRepository;
        private readonly ISizeTypeRepository _sizeTypeRepository;
        private readonly ICategorySizeTypeRepository _categorySizeTypeRepository;

        public SizeService(ISizeRepository sizeRepository, ISizeTypeRepository sizeTypeRepository, ICategorySizeTypeRepository categorySizeTypeRepository)
        {
            _sizeRepository = sizeRepository;
            _sizeTypeRepository = sizeTypeRepository;
            _categorySizeTypeRepository = categorySizeTypeRepository;
        }

        public async Task<IDataResult<SizeDTO>> CreateAsync(SizeCreateDTO sizeCreateDTO)
        {
            if (await _sizeRepository.AnyAsync(x => x.SizeName.ToLower() == sizeCreateDTO.SizeName.ToLower()))
            {
                return new ErrorDataResult<SizeDTO>("Mevcut Size Sistemde Kayıtlı!");
            }
            var newSize = sizeCreateDTO.Adapt<Size>();
            await _sizeRepository.AddAsync(newSize);
            await _sizeRepository.SaveChangesAsync();

            var sizeDto = newSize.Adapt<SizeDTO>();
            return new SuccessDataResult<SizeDTO>(sizeDto, "Size Ekleme Başarılı!");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var deletingSize = await _sizeRepository.GetByIdAsync(id);
            if (deletingSize == null)
            {
                return new ErrorResult("Silinecek Veri Bulunamadı");
            }
            await _sizeRepository.DeleteAsync(deletingSize);
            await _sizeRepository.SaveChangesAsync();
            return new SuccessResult("Size Silme İşlemi Başarılı");
        }

        public async Task<IDataResult<List<SizeListDTO>>> GetAllAsync()
        {
            var sizes = await _sizeRepository.GetAllAsync();
            var sizeListDtos = sizes.Adapt<List<SizeListDTO>>();
            if (sizes.Count() <= 0)
            {
                return new ErrorDataResult<List<SizeListDTO>>(sizeListDtos, "Listenecek Size Bulunamadı");

            }
            return new SuccessDataResult<List<SizeListDTO>>(sizeListDtos, "Size Listeleme Başarılı ");
        }




        // seçilen kategoriye ait Sizeları getiren metod
        public async Task<IDataResult<List<SizeDTO>>> GetByCategoryIdAsync(Guid categoryId)
        {
            var categorySizeTypes = await _categorySizeTypeRepository.GetAllAsync(x=>x.CategoryId == categoryId);
            var sizeTypes = new List<SizeType>();
            foreach (var categorySizeType in categorySizeTypes)
            {
                var sizeType = await _sizeTypeRepository.GetAllAsync(x => x.Id == categorySizeType.SizeTypeId);
                sizeTypes.AddRange(sizeType);
            }


            List<Size> sizes = new List<Size>();
            foreach (var item in sizeTypes)
            {
                var categorySizes = await _sizeRepository.GetAllAsync(x => x.SizeTypeId == item.Id);
                sizes.AddRange(categorySizes);
            }
            return new SuccessDataResult<List<SizeDTO>>(sizes.Adapt<List<SizeDTO>>(), "Size Listeleme başarılı");
        }

        public async Task<IDataResult<SizeDTO>> GetByIdAsync(Guid id)
        {
            var size = await _sizeRepository.GetByIdAsync(id);
            if (size is null)
            {
                return new ErrorDataResult<SizeDTO>("Gösterilecek Size Bulunamadı");
            }
            var sizetDto = size.Adapt<SizeDTO>();
            return new SuccessDataResult<SizeDTO>(sizetDto, "Size Gösterme Başarılı");
        }

        public async Task<IDataResult<SizeDTO>> UpdateAsync(SizeUpdateDTO sizeUpdateDTO)
        {
            var updatingSize = await _sizeRepository.GetByIdAsync(sizeUpdateDTO.Id);
            if (updatingSize is null)
            {
                return new ErrorDataResult<SizeDTO>("Güncellenecek Size Bulunamadı");
            }
            var updatedSize = sizeUpdateDTO.Adapt(updatingSize);
            await _sizeRepository.UpdateAsync(updatedSize);
            await _sizeRepository.SaveChangesAsync();
            return new SuccessDataResult<SizeDTO>(updatedSize.Adapt<SizeDTO>(), "Size Güncelleme Başarılı");
        }
    }
}
