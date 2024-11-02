using AutoMapper;
using BlogWebsite.Data.Models;
using BlogWebsite.DTO.Blog;
using BlogWebsite.DTO.Common;
using BlogWebsite.Service.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = BlogWebsite.Data.Models;

namespace BlogWebsite.Service.Blog
{
    public class BlogService : IBlogService
    {
        private readonly IMapper _mapper;
        private readonly ICommonService<BlogPost> _blogPostCmSv;
        private readonly ICommonService<Model.Category> _categoryCmSv;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogService(ICommonService<BlogPost> blogPostCmSv, IMapper mapper, ICommonService<Model.Category> categoryCmSv, IWebHostEnvironment webHostEnvironment)
        {
            _blogPostCmSv = blogPostCmSv;
            _mapper = mapper;
            _categoryCmSv = categoryCmSv;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ClassResult<BlogPostDTO>> CreateNewPost(BlogPostDTO blogPostDTO)
        {
            try
            {
                var newPost = _mapper.Map<BlogPost>(blogPostDTO);
                newPost.CreateDate = DateTime.Now;
                if (blogPostDTO.SelectedCategoryIds != null)
                {
                    foreach (var categoryId in blogPostDTO.SelectedCategoryIds)
                    {
                        var category = await _categoryCmSv.TableT().FindAsync(categoryId);
                        if (category != null)
                        {
                            newPost.Categories.Add(category);
                        }
                    }


                    if (blogPostDTO.Thumbnail == null || blogPostDTO.Thumbnail.Length == 0)
                    {
                        return null;
                    }

                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var filePath = Path.Combine(uploadsFolder, blogPostDTO.Thumbnail.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await blogPostDTO.Thumbnail.CopyToAsync(stream);
                    }

                    newPost.ThumbnailUrl = $"https://localhost:7297/uploads/{blogPostDTO.Thumbnail.FileName}";
                    await _blogPostCmSv.AddAs(newPost);

                    return ClassResult<BlogPostDTO>.SuccessResult();
                }
                return null;
            }
            catch (Exception ex)
            {
                //throw new Exception("error...");
                return ClassResult<BlogPostDTO>.FailureResult(ex.Message);
            }
        }


        public async Task<ClassResult<BlogPostDTO>> GetPost(int id)
        {
            try
            {
                //var post = await _blogPostCmSv.GetById(id);
                var p = _blogPostCmSv.TableT().Include(b => b.Categories).FirstOrDefault(b=>b.BlogPostID==id);
                var mapped = _mapper.Map<BlogPostDTO>(p);

                mapped.AllCategories = _categoryCmSv.TableT().Select(x=> new CategoryDTO { CategoryID = x.CategoryID, CategoryName =x.CategoryName }).ToList();
                mapped.SelectedCategoryIds = mapped.Categories.Select(x => x.CategoryID).ToList();
                return ClassResult<BlogPostDTO>.SuccessResult(mapped);
            }
            catch (Exception ex)
            {

                return ClassResult<BlogPostDTO>.FailureResult($"{ex.Message}");
            }
        }

        public async Task<ClassResult<BlogPostDTO>> UpdatePost(BlogPostDTO dto)
        {
            try
            {
                
                var p = _blogPostCmSv.TableT().Include(b => b.Categories).FirstOrDefault(b=>b.BlogPostID==dto.BlogPostID);

                if(p == null)
                {
                    return ClassResult<BlogPostDTO>.FailureResult();
                }
                 _mapper.Map(dto, p);
                
                p.Categories.Clear();
                if(dto.SelectedCategoryIds != null)
                {
                    foreach(var categoryId in dto.SelectedCategoryIds) 
                    {
                        var category = await _categoryCmSv.TableT().FindAsync(categoryId);

                        if(category != null)
                        {
                            p.Categories.Add(category);
                        }
                    }
                }
                //if (dto.Thumbnail == null || dto.Thumbnail.Length == 0)
                //{
                //    return null;
                //}
                if(dto.Thumbnail != null)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var filePath = Path.Combine(uploadsFolder, dto.Thumbnail.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.Thumbnail.CopyToAsync(stream);
                    }
                    p.ThumbnailUrl = $"https://localhost:7297/uploads/{dto.Thumbnail.FileName}";
                }
                

                await _blogPostCmSv.UpdateAs(p);////////////////////
                return ClassResult<BlogPostDTO>.SuccessResult();
            }
            catch (Exception ex)
            {
                return ClassResult<BlogPostDTO>.FailureResult(ex.ToString());
            }
        }

        public async Task<ClassResult<List<BlogPostDTO>>> GetPostsList()
        {
            try
            {
                var includeCate = _blogPostCmSv.TableT().Include(c => c.Categories).ToList();
                
                var mapped = _mapper.Map<List<BlogPostDTO>>(includeCate);
                

                return ClassResult<List<BlogPostDTO>>.SuccessResult(mapped);
            }
            catch(Exception ex)
            {
                return ClassResult<List<BlogPostDTO>>.FailureResult(ex.ToString());
            }
            
        }

        public async Task<ClassResult<BlogPostDTO>> DeletePost(int id)
        {
            try
            {
                var findPost = await _blogPostCmSv.TableT().FindAsync(id);
                if (findPost!=null)
                {
                     await _blogPostCmSv.DeleteAs(findPost);
                    return ClassResult<BlogPostDTO>.SuccessResult();
                }
                return ClassResult<BlogPostDTO>.FailureResult("Cant find the post!");

            }
            catch ( Exception ex )
            {
                return ClassResult<BlogPostDTO>.FailureResult(ex.ToString());
            }
        }


    }
}
