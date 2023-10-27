using Keeper.Common.Response;
using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface ICommentService
    {
        Task<ResponseModel<CommentViewModel>> AddCommentAsync(AddComment addComment, Guid UserId);
        Task<List<CommentViewModel>> GetAllCommnets(Guid ItemId);
    }
}
