using Keeper.Context.Model;

namespace Keeper.Repos.Repositories.Interfaces
{
    public interface ICommentRepo
    {
        Task<CommentModel> AddAsync(CommentModel comment);
        Task<List<CommentModel>> GetAllAsync(Guid itemId);
    }
}
