using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepo _repo;
        private readonly IFileService _file;
        private readonly ICommentService _comment;
        public ItemService(IItemRepo itemRepo, IFileService file, ICommentService comment)
        {
            _repo = itemRepo;
            _file = file;
            _comment = comment;
        }
        public async Task<List<ItemViewModel>> GetAllAsync(Guid keepId)
        {
            var data = await _repo.GetAllAsync(keepId);
            List<ItemViewModel> items = new();
            for (int i = 0; i < data.Count; i++)
            {
                var item = Mapper(data[i]);
                item.Files = await _file.GetAllFiles(data[i].Id);
                item.Comments = await _comment.GetAllCommnets(data[i].Id);
                items.Add(item);
            }
            return items;
        }
        public async Task<ItemViewModel> GetAsync(Guid id)
        {
            var data = await _repo.GetAsync(id) ?? throw new InnerException("", StatusType.NOT_FOUND);
            var Files = await _file.GetAllFiles(data.Id);
            var comments = await _comment.GetAllCommnets(data.Id);
            var item = Mapper(data);
            item.Files = Files;
            item.Comments = comments;
            return item;
        }
        public async Task<ItemViewModel> SaveAsync(AddItem addItem, Guid userId)
        {
            ItemModel item = new()
            {
                Title = addItem.Title,
                Description = addItem.Description,
                Type = addItem.Type,
                URL = addItem.URL,
                Number = addItem.Number,
                KeepId = addItem.KeepId,
                CreatedById = userId,
                CreatedOn = DateTime.Now,
                To = addItem.To,
                DiscussedBy = addItem.DiscussedBy,
            };
            var itemId = await _repo.SaveAsync(item);
            if (addItem.Files != null)
            {
                await _file.AddAsync(userId, item.KeepId, itemId, addItem.Files);
            }
            var res = await GetAsync(itemId);
            return res;
        }
        public async Task<ItemViewModel> UpdateAsync(EditItem editItem, Guid userId)
        {
            var res = await _repo.GetAsync(editItem.Id) ?? throw new InnerException("",StatusType.NOT_FOUND);
            res.Title = editItem.Title;
            res.Description = editItem.Description;
            res.Type = editItem.Type;
            res.URL = editItem.URL;
            res.Number = editItem.Number;
            res.To = editItem.To;
            res.DiscussedBy = editItem.DiscussedBy;
            res.UpdatedById = userId;
            res.UpdatedOn = DateTime.Now;
            var itemId = await _repo.Update(res);
            if (editItem.Files != null)
            {
                await _file.AddAsync(res.CreatedById, res.KeepId, res.Id, editItem.Files);
            }
            var response = await GetAsync(itemId);
            return response;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var res = await _repo.GetAsync(id) ?? throw new InnerException("", StatusType.NOT_FOUND);
            await _repo.Delete(res);
            return true;
        }
        private static ItemViewModel Mapper(ItemModel item)
        {
            return new ItemViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Type = item.Type,
                Number = item.Number,
                URL = item.URL,
                Description = item.Description,
                CreatedBy = item.CreatedBy.Email,
                CreatedOn = item.CreatedOn,
                UpdatedOn = item.UpdatedOn,
                UpdatedBy = item.UpdatedBy?.Email,
                To = item.To,
                DiscussedBy = item.DiscussedBy,
            };
        }
    }
}
