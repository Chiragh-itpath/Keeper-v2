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
        private readonly IItemRepo _itemRepo;
        private readonly IFileService _fileService;
        private readonly IFileRepo _fileRepo;
        private readonly ICommentService _commentService;
        private readonly ICommentRepo _commentRepo;
        public ItemService(IItemRepo itemRepo, IFileService file, ICommentService comment, IFileRepo fileRepo, ICommentRepo commentRepo)
        {
            _itemRepo = itemRepo;
            _fileService = file;
            _commentService = comment;
            _fileRepo = fileRepo;
            _commentRepo = commentRepo;
        }
        public async Task<List<ItemViewModel>> GetAllAsync(Guid keepId)
        {
            var data = await _itemRepo.GetAllAsync(keepId);
            List<ItemViewModel> items = new();
            foreach (var item in data)
            {
                var _item = Mapper(item);
                _item.Files = await _fileService.GetAllFiles(item.Id);
                items.Add(_item);
            }
            return items;
        }
        public async Task<ItemViewModel> GetAsync(Guid id)
        {
            var data = await _itemRepo.GetAsync(id) ?? throw new InnerException("", StatusType.NOT_FOUND);
            var Files = await _fileService.GetAllFiles(data.Id);
            var comments = await _commentService.GetAllCommnets(data.Id);
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
                StatusId = addItem.StatusId
            };
            var itemId = await _itemRepo.SaveAsync(item);
            if (addItem.Files != null)
            {
                await _fileService.AddAsync(userId, item.KeepId, itemId, addItem.Files);
            }
            var res = await GetAsync(itemId);
            return res;
        }
        public async Task<ItemViewModel> UpdateAsync(EditItem editItem, Guid userId)
        {
            var res = await _itemRepo.GetAsync(editItem.Id) ?? throw new InnerException("", StatusType.NOT_FOUND);
            res.Title = editItem.Title;
            res.Description = editItem.Description;
            res.Type = editItem.Type;
            res.URL = editItem.URL;
            res.Number = editItem.Number;
            res.To = editItem.To;
            res.DiscussedBy = editItem.DiscussedBy;
            res.UpdatedById = userId;
            res.UpdatedOn = DateTime.Now;
            var itemId = await _itemRepo.Update(res);
            if (editItem.Files != null)
            {
                await _fileService.AddAsync(res.CreatedById, res.KeepId, res.Id, editItem.Files);
            }
            var response = await GetAsync(itemId);
            return response;
        }
        public async Task<ItemViewModel> UpdateStatus(UpdateItemStatus newStatusDetails, Guid userId)
        {
            var item = await _itemRepo.GetAsync(newStatusDetails.Id) ?? throw new InnerException("No Item found with id", StatusType.NOT_FOUND);
            item.StatusId = newStatusDetails.StatusId;
            item.UpdatedById = userId;
            item.UpdatedOn = DateTime.Now;
            await _itemRepo.Update(item);
            return Mapper(item);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var res = await _itemRepo.GetAsync(id) ?? throw new InnerException("", StatusType.NOT_FOUND);
            await _itemRepo.Delete(res);
            return true;
        }
        public async Task<bool> MoveItem(MoveItemModel moveItem)
        {
            var item = await _itemRepo.GetAsync(moveItem.ItemId) ?? throw new InnerException("Item not found", StatusType.NOT_FOUND);

            if (moveItem.Action == MoveAction.Copy)
            {
                ItemModel newItem = new()
                {
                    Title = item.Title,
                    Description = item.Description,
                    Type = item.Type,
                    URL = item.URL,
                    Number = item.Number,
                    To = item.To,
                    DiscussedBy = item.DiscussedBy,
                    StatusId = item.StatusId,
                    KeepId = moveItem.TargetKeepId,
                    CreatedById = item.CreatedById,
                    CreatedOn = DateTime.Now
                };

                await _itemRepo.SaveAsync(newItem);
                var files = await _fileRepo.GetFilesAsync(item.Id);
                if (files != null && files.Any())
                {
                    var itemFiles = files.Select(x => new ItemFileLinkerModel
                    {
                        FileId = x.Id,
                        ItemId = newItem.Id
                    }).ToList();
                    await _fileRepo.AddFileLinksRange(itemFiles);
                }
                var comments = await _commentRepo.GetAllAsync(moveItem.ItemId);
                if(comments != null && comments.Any())
                {
                    comments.ForEach(x =>
                    {
                        x.ItemId = newItem.Id;
                    });
                    await _commentRepo.AddRangeAsync(comments);
                }
            }
            else if (moveItem.Action == MoveAction.Move)
            {
                item.KeepId = moveItem.TargetKeepId;
                item.UpdatedOn = DateTime.Now;
                await _itemRepo.Update(item);
            }

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
                Status = item.Status,
                KeepId = item.KeepId,
                StatusId = item.StatusId ?? Guid.Empty
            };
        }
    }
}
