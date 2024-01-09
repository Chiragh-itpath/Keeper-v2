using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Keeper.Services.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepo _file;
        private readonly IItemFileLInkerRepo _linker;
        private readonly IWebHostEnvironment _env;
        private readonly string imagePath;
        public FileService(IFileRepo file, IItemFileLInkerRepo linker, IWebHostEnvironment env, IConfiguration config)
        {
            _file = file;
            _linker = linker;
            _env = env;
            imagePath = config["ImagePath"];
        }
        public async Task AddAsync(Guid UserId, Guid KeepId, Guid ItemId, List<IFormFile> files)
        {
            string webRoot = _env.WebRootPath;
            string userDict = Path.Combine(webRoot, "Images", UserId.ToString());
            EnsureDirectoryExists(userDict);
            string KeepDict = Path.Combine(userDict, KeepId.ToString());
            EnsureDirectoryExists(KeepDict);
            foreach (var file in files)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fileStorePath = Path.Combine(UserId.ToString(), KeepId.ToString(), filename);
                string fileToUpload = Path.Combine(KeepDict, filename);

                using (var stream = new FileStream(fileToUpload, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileModel = await _file.AddAsync(new FileModel
                {
                    FilePath = fileStorePath,
                    OriginalName = file.FileName,
                });

                await _linker.AddAsync(new ItemFileLinkerModel
                {
                    FileId = fileModel.Id,
                    ItemId = ItemId
                });
            }
        }
        public async Task<List<FileViewModel>> GetAllFiles(Guid itemId)
        {
            List<string> imageExtensions = new() { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG", ".JFIF" };
            var files = (await _file.GetFilesAsync(itemId))
                .Select(x => new FileViewModel
                {
                    FileName = x.OriginalName,
                    FileUrl = Path.Combine(this.imagePath, $"{x.Id}"),
                    IsImage = imageExtensions.Contains(Path.GetExtension(x.FilePath)?.ToUpperInvariant() ?? "")
                })
                .ToList();
            return files;
        }
        public async Task<FileModel> GetFileDetails(Guid id)
        {
            return await _file.GetByIdAsync(id);
        }
        private static void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
