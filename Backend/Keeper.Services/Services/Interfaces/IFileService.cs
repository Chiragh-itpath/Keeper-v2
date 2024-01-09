using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Microsoft.AspNetCore.Http;

namespace Keeper.Services.Services.Interfaces
{
    public interface IFileService
    {
        Task AddAsync(Guid UserId, Guid KeepId, Guid ItemId, List<IFormFile> files);
        Task<List<FileViewModel>> GetAllFiles(Guid itemId);
        Task<FileModel> GetFileDetails(Guid id);
    }
}
