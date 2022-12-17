using api.DTOs;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using API.DTOs;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<Entities.UserLike> GetUserLike(int sourceUserId, int targetUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}