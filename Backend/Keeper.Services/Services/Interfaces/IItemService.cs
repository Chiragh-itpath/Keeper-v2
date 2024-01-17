using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemViewModel>> GetAllAsync(Guid Keepid);
        Task<ItemViewModel> GetAsync(Guid id);
        Task<ItemViewModel> SaveAsync(AddItem addItem,Guid userId);
        Task<ItemViewModel> UpdateAsync(EditItem editItem, Guid userId);
        Task<bool> DeleteAsync(Guid id);
        Task<ItemViewModel> UpdateStatus(UpdateItemStatus newStatusDetails,Guid userId);
    }
}
