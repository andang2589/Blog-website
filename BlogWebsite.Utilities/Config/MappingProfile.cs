using AutoMapper;
using BlogWebsite.Data.Models;
using BlogWebsite.DTO.Blog;
using BlogWebsite.DTO.Category;
using BlogWebsite.DTO.Comment;
using BlogWebsite.DTO.Role;
using BlogWebsite.DTO.User;

namespace BlogWebsite.Utilities.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<AppUser, UserDTO>();
            CreateMap<UserUpdateRequest, AppUser>();
            CreateMap<UserDTO,UserUpdateRequest>();
            //CreateMap<UserUpdateRequest, AppUser>()
            //.BeforeMap((src, dest) =>
            //{
            //    dest.SecurityStamp = Guid.NewGuid().ToString();
            //});
            CreateMap<RegisterRequest,AppUser>();
            CreateMap<BlogPostDTO, BlogPost>().ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));
            CreateMap<BlogPost,BlogPostDTO>().ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories)); 
            CreateMap<CreateCommentDto, Comment>().ForMember(dest=>dest.ParentID, opt=>opt.MapFrom(src=>src.ReplyToCommentId));
            CreateMap<Category,CategoryDto>().ReverseMap();
            //CreateMap<CategoryDto,Category>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<AppRole, RoleDto>().ReverseMap();
            CreateMap<AppUser, GetUsersByRoleDto>().ReverseMap();
        }
    }
    
}
