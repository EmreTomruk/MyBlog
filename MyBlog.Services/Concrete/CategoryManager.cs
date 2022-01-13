﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using MyBlog.Data.Abstract;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Results.Abstract;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.Shared.Utilities.Results.Concrete;

namespace MyBlog.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<CategoryDto>> Get(int categoryId) //Id'ye esit olan categori ve ona ait olan articles'ları getir...
        {
           var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);

           if (category != null)
               return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
               {
                   Category=category,
                   ResultStatus=ResultStatus.Success
               });
           return new DataResult<CategoryDto>(ResultStatus.Error, "Böyle bir kategori bulunamadı", new CategoryDto
           {
               Category=null,
               ResultStatus=ResultStatus.Error,
               Message= "Böyle bir kategori bulunamadı"
           });
        }

        public async Task<IDataResult<CategoryListDto>> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null, c => c.Articles);

            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }

            return new DataResult<CategoryListDto>(ResultStatus.Error, "Hiçbir kategori bulunamadı.", new CategoryListDto //Burada kurulan yapi ile ister Controller'da istersek View icinde yapimizi kurabiliriz...
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = "Hiçbir kategori bulunamadı."
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeleted()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted, c => c.Articles); //or c => c == c.IsDeleted==false

            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }

            return new DataResult<CategoryListDto>(ResultStatus.Error, "Hiçbir kategori bulunamadı.", new CategoryListDto //Burada kurulan yapi ile ister Controller'da istersek View icinde yapimizi kurabiliriz...
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = "Hiçbir kategori bulunamadı."
            });
        }

        public async Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category = _mapper.Map<Category>(categoryAddDto);

            category.CreatedByName= createdByName;
            category.ModifiedByName = createdByName;

            var addedCategory = await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success, $"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir.", new CategoryDto
            {
                Category=addedCategory,
                ResultStatus=ResultStatus.Success,
                Message=$"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir."
            });
        }

        public async Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var oldCategory = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id); //Her guncellemede category'ye yeni bir CreatedDate ve CreatedByName eklenmeyecek... 
            var category = _mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, oldCategory);

            category.ModifiedByName = modifiedByName;

            var updatedCategory = await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success, $"{categoryUpdateDto.Name} adlı kategori başarıyla güncellenmiştir.", new CategoryDto
            {
                Category = updatedCategory,
                ResultStatus = ResultStatus.Success,
                Message = $"{categoryUpdateDto.Name} adlı kategori başarıyla güncellenmiştir."
            });            
        }

        public async Task<IDataResult<CategoryDto>> Delete(int categoryId, string modifiedByName)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);

            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;

                var deletedCategory = await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveAsync();

                return new DataResult<CategoryDto>(ResultStatus.Success, $"{deletedCategory.Name} adlı kategori başarıyla silinmiştir.", new CategoryDto
                {
                    Category = deletedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = $"{deletedCategory.Name} adlı kategori başarıyla silinmiştir."
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, "Böyle bir kategori bulunamadı.", new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = "Böyle bir kategori bulunamadı."
            });
        }

        public async Task<IResult> HardDelete(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);

            if (category != null)
            {
                await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, message: $"{category.Name} adlı kategori başarıyla veritabanından silinmiştir.");
            }
            return new Result(ResultStatus.Error, message: "Böyle bir kategori bulunamadı.");
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive, c => c.Articles); //or c => c == c.IsDeleted==false

            if (categories.Count > -1)
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Hiçbir kategori bulunamadı.", null);
        }

        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDto(int categoryId)
        {
            var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);

            if (result)
            {
                var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category); //category verisi CategoryUpdateDto'ya Map edilerek bir "categoryUpdateDto" degiskenini elde ettik...

                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }

            else
            {
                return new DataResult<CategoryUpdateDto>(ResultStatus.Error, message: "Böyle bir kategori bulunamadı...", null);
            }
        }
    }
}
