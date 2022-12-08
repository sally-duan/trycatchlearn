using api.DTOs;
using api.Entities;
using api.Helpers;

namespace api.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);
         Task<bool> SaveAllAsync();
         Task<IEnumerable<AppUser>> GetUsersAsync();
         Task<AppUser> GetUserByIdAsync(int id);
         Task<AppUser> GetUser(string username);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string username);        
    }
}