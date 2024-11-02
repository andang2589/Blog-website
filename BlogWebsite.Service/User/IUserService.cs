using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogWebsite.Data.Models;
using BlogWebsite.DTO.User;
using BlogWebsite.DTO.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace BlogWebsite.Service.User
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);

        Task<PagedResult<UserDTO>> GetUserAndPaging(GetUserAndPagingRequest request);
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);
        Task<ApiResult<UserDTO>> GetById(Guid id);
        Task<bool> IsEmailInUse(string email);
        Task<ClassResult<UserDTO>> DeleteUser(string userId);
        Task<string> GenerateJwtTokenAsync(AppUser user);
    }
}
