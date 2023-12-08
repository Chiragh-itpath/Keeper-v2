using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepo _comment;
        public CommentService(ICommentRepo comment)
        {
            _comment = comment;
        }
        public async Task<CommentViewModel> AddCommentAsync(AddComment addComment, Guid UserId)
        {
            var comment = await _comment.AddAsync(new CommentModel
            {
                UserId = UserId,
                TimeStamp = DateTime.Now,
                Content = addComment.Content,
                CommentId = addComment.CommentId,
                ItemId = addComment.ItemId
            });
            var comments = new CommentViewModel
            {
                Id = comment.Id,
                Content = comment.Content,
                Date = comment.TimeStamp,
                User = ""
            };
            return comments;
        }
        public async Task<List<CommentViewModel>> GetAllCommnets(Guid ItemId)
        {
            var topComments = await _comment.GetAllAsync(ItemId);
            List<CommentViewModel> comments = new();
            topComments.ForEach(comment =>
            {
                var viewModel = new CommentViewModel
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    Date = comment.TimeStamp,
                    User = comment.User.UserName
                };
                comments.Add(viewModel);
                LoadReplies(comment, viewModel);
            });
            return comments;
        }
        private static void LoadReplies(CommentModel comment, CommentViewModel viewModel)
        {
            viewModel.Comments = comment.Comments!.Select(rep => new CommentViewModel
            {
                Id = rep.Id,
                Content = rep.Content,
                Date = rep.TimeStamp,
                User = rep.User.UserName,
            }).ToList();
            for (int i = 0; i < comment.Comments!.Count; i++)
            {
                LoadReplies(comment.Comments[i], viewModel.Comments[i]);
            }
        }
    }
}
