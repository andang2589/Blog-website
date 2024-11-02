using BlogWebsite.DTO.Common;
using BlogWebsite.DTO.User;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWebsite.Areas.Admin.CallApiClient
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PagedResult<UserDTO>> GetUserAndPaging(GetUserAndPagingRequest request);
        Task<bool> RegisterUser(RegisterRequest registerRequest);
        Task<ApiResult<bool>> UpdateUser(Guid id,UserUpdateRequest request);
        Task<ApiResult<UserDTO>> GetById(Guid id);
        Task<bool> IsEmailInUse(string email);

        Task<ClassResult<UserDTO>> DeleteUser(string userId);
    }
}
