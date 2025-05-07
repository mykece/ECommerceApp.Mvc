using ETicaret.Applicationn.DTOs.SizeTypeDTOs;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using ETicaret.Infrastructure.Repositories.SizeRepositories;
using ETicaret.Infrastructure.Repositories.SizeTypeRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Applicationn.Services.SizeTypeServices
{
    public class SizeTypeService : ISizeTypeService
    {
        private readonly ISizeTypeRepository _sizeTypeRepository;
        private readonly ISizeRepository _sizeRepository;

        public SizeTypeService(ISizeTypeRepository sizeTypeRepository, ISizeRepository sizeRepository)
        {
            _sizeTypeRepository = sizeTypeRepository;
            _sizeRepository = sizeRepository;
        }

        public async Task<IDataResult<SizeTypeDTO>> CreateAsync(SizeTypeCreateDTO sizeTypeCreateDTO)
        {
            if (await _sizeTypeRepository.AnyAsync(x => x.SizeTypeName.ToLower() == sizeTypeCreateDTO.SizeTypeName.ToLower()))
            {
                return new ErrorDataResult<SizeTypeDTO>("Size type already added!");
            }
            var newSizeType = sizeTypeCreateDTO.Adapt<SizeType>();
            await _sizeTypeRepository.AddAsync(newSizeType);
            await _sizeTypeRepository.SaveChangesAsync();

            var sizeTypeDto = newSizeType.Adapt<SizeTypeDTO>();
            return new SuccessDataResult<SizeTypeDTO>(sizeTypeDto, "Size Type added successfully!");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DataResult<SizeTypeDTO> result = new ErrorDataResult<SizeTypeDTO>();

            var strategy = await _sizeTypeRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _sizeTypeRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var sizeType = await _sizeTypeRepository.GetByIdAsync(id);
                    if (sizeType == null)
                    {
                        result = new ErrorDataResult<SizeTypeDTO>("Silinecek Size Type Bulunamadı!");
                        transactionScope.Rollback();
                        return;
                    }
                    await _sizeTypeRepository.DeleteAsync(sizeType);
                    var sizes = await _sizeRepository.GetAllAsync(x => x.SizeTypeId == id);
                    foreach (var item in sizes)
                    {
                        await _sizeRepository.DeleteAsync(item);
                    }
                    await _sizeRepository.SaveChangesAsync();
                    result = new SuccessDataResult<SizeTypeDTO>("Size Type Silme başarılı");
                    transactionScope.Commit();

                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<SizeTypeDTO>("Size Type could not be deleted!" + ex.Message);
                    transactionScope.Rollback();

                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;

        }

        public async Task<IDataResult<List<SizeTypeListDTO>>> GetAllAsync()
        {
            var sizeTypes = await _sizeTypeRepository.GetAllAsync();
            var sizeTypeListDTOs = sizeTypes.Adapt<List<SizeTypeListDTO>>();
            if (sizeTypes.Count() <= 0)
            {
                return new ErrorDataResult<List<SizeTypeListDTO>>(sizeTypeListDTOs, "Size types could not be found!");

            }
            return new SuccessDataResult<List<SizeTypeListDTO>>(sizeTypeListDTOs, "Size types listed succesfully! ");
        }

        public async Task<IDataResult<SizeTypeDTO>> GetByIdAsync(Guid id)
        {
            var sizeType = await _sizeTypeRepository.GetByIdAsync(id);
            if (sizeType is null)
            {
                return new ErrorDataResult<SizeTypeDTO>("Size type could not be found!");
            }
            var sizeTypeDTO = sizeType.Adapt<SizeTypeDTO>();
            return new SuccessDataResult<SizeTypeDTO>(sizeTypeDTO, "Size type found successfully!");
        }

        public async Task<IDataResult<SizeTypeDTO>> UpdateAsync(SizeTypeUpdateDTO sizeTypeUpdateDTO)
        {
            var updatingSizeType = await _sizeTypeRepository.GetByIdAsync(sizeTypeUpdateDTO.Id);
            if (updatingSizeType is null)
            {
                return new ErrorDataResult<SizeTypeDTO>("Size type could not be found!");
            }
            var updatedSizeType = sizeTypeUpdateDTO.Adapt(updatingSizeType);
            await _sizeTypeRepository.UpdateAsync(updatedSizeType);
            await _sizeTypeRepository.SaveChangesAsync();
            return new SuccessDataResult<SizeTypeDTO>(updatedSizeType.Adapt<SizeTypeDTO>(), "Size type updated successfully!");
        }
    }
}
