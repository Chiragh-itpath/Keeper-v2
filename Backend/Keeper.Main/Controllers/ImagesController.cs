using Keeper.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Keeper.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly string imagePath;
        
        public ImagesController(IFileService fileService, IWebHostEnvironment env)
        {
            _fileService = fileService;
            imagePath = Path.Combine(env.WebRootPath,"Images");
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> DownloadFile([FromRoute] Guid Id)
        {
            var filedetails = await _fileService.GetFileDetails(Id);
            var filePath = Path.Combine(imagePath,filedetails.FilePath);
            if(System.IO.File.Exists(filePath))
            {
                var fileByte = System.IO.File.ReadAllBytes(filePath);
                return File(fileByte, "application/octet-stream",filedetails.OriginalName);
            }
            return NotFound();
        }
    }
}
