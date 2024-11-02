using AutoMapper;
using BlogWebsite.DTO.Category;
using BlogWebsite.DTO.Common;
using BlogWebsite.Service.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model =  BlogWebsite.Data.Models;

namespace BlogWebsite.Service.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICommonService<Model.Category> _categoryCmSv;
        private readonly IMapper _mapper;

        public CategoryService(ICommonService<Model.Category> categoryCmSv, IMapper mapper) 
        {
            _mapper = mapper;
            _categoryCmSv = categoryCmSv;
        }

        public async Task<ClassResult<Model.Category>> CreateCategory(CategoryDto dto)
        {         
            try
            {
                var cate = _mapper.Map<Model.Category>(dto);
                await _categoryCmSv.AddAs(cate);
                return ClassResult<Model.Category>.SuccessResult(cate);
            }
            catch (Exception ex)
            {
                return ClassResult<Model.Category>.FailureResult(ex.ToString());
            }  
        }

        public async Task<ClassResult<List<CategoryDto>>> GetListCategories()
        {
            try
            {
                var list = await _categoryCmSv.TableT().ToListAsync();
                var mappedList = _mapper.Map<List<CategoryDto>>(list);
                return ClassResult<List<CategoryDto>>.SuccessResult(mappedList);
            }
            catch(Exception ex)
            {
                return ClassResult<List<CategoryDto>>.FailureResult(ex.ToString());
            }
        }

        public async Task<ClassResult<CategoryDto>> DeleteCategory(int id)
        {
            try
            {
                var findCategory = await _categoryCmSv.TableT().FindAsync(id);
                if(findCategory == null)
                {
                    return ClassResult<CategoryDto>.FailureResult("Id not exist");
                }
                await _categoryCmSv.DeleteAs(findCategory);
                return ClassResult<CategoryDto>.SuccessResult();
            }
            catch (Exception ex)
            {
                return ClassResult<CategoryDto>.FailureResult(ex.ToString());
            }
        }
    }
}
