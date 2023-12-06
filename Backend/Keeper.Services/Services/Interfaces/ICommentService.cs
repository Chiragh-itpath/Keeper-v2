using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentViewModel> AddCommentAsync(AddComment addComment, Guid UserId);
        Task<List<CommentViewModel>> GetAllCommnets(Guid ItemId);
    }
}
