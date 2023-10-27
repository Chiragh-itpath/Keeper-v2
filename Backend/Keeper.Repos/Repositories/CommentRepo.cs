using Keeper.Context;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Repos.Repositories
{
    public class CommentRepo : ICommentRepo
    {
        private readonly DbKeeperContext _db;
        public CommentRepo(DbKeeperContext db)
        {
            _db = db;
        }
        public async Task<CommentModel> AddAsync(CommentModel comment)
        {
            await _db.Comment.AddAsync(comment);
            await _db.SaveChangesAsync();
            return comment;
        }
        public async Task<List<CommentModel>> GetAllAsync(Guid itemId)
        {
            var commentQuery = from c in _db.Comment.Include(c => c.User)
                               where c.ItemId == itemId && c.CommentId == null
                               orderby c.TimeStamp descending
                               select c;
            var comments = await commentQuery.ToListAsync();
            for (int i = 0; i < comments.Count; i++)
            {
                await LoadReplies(comments[i]);
            }
            return comments;
        }
        private async Task LoadReplies(CommentModel comment)
        {
            var query = from r in _db.Comment.Include(c => c.User)
                        where r.CommentId == comment.Id
                        orderby r.TimeStamp descending
                        select r;
            comment.Comments = await query.ToListAsync();
            for (int i = 0; i < comment.Comments.Count; i++)
            {
                await LoadReplies(comment.Comments[i]);
            }
        }
    }
}
