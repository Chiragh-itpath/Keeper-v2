using Keeper.Common.Response;
using Keeper.Common.ViewModels;
using Keeper.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeper.Main.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ICommentService _comment;
        public ItemController(IItemService itemService, ICommentService comment)
        {
            _itemService = itemService;
            _comment = comment;
        }
        [HttpGet("{id}")]
        public async Task<ResponseModel<ItemViewModel>> GetById([FromRoute] Guid id)
        {
            var res = await _itemService.GetAsync(id);
            return new ResponseModel<ItemViewModel> { Data = res };
        }

        [HttpGet("")]
        public async Task<ResponseModel<List<ItemViewModel>>> GetAll(Guid keepId)
        {
            var res = await _itemService.GetAllAsync(keepId);
            return new ResponseModel<List<ItemViewModel>> { Data = res };
        }

        [HttpPost("")]
        public async Task<ResponseModel<ItemViewModel>> Post([FromForm] AddItem addItem)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _itemService.SaveAsync(addItem, userId);
            return new ResponseModel<ItemViewModel> { Data = res };
        }
        [HttpPut("")]
        public async Task<ResponseModel<ItemViewModel>> Put([FromForm] EditItem editItem)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _itemService.UpdateAsync(editItem, userId);
            return new ResponseModel<ItemViewModel> { Data = res };
        }
        [HttpPut("UpdateStatus")]
        public async Task<ResponseModel<ItemViewModel>> UpdateStatus(UpdateItemStatus newStatusDetails)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var response = await _itemService.UpdateStatus(newStatusDetails, userId);
            return new() { Data = response };
        }
        [HttpDelete("{Id}")]
        public async Task<ResponseModel<string>> Delete(Guid id)
        {
            var res = await _itemService.DeleteAsync(id);
            return new ResponseModel<string>() { Data = res.ToString() };
        }
        [HttpPost("AddComment")]
        public async Task<ResponseModel<CommentViewModel>> AddComment(AddComment addComment)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _comment.AddCommentAsync(addComment, userId);
            return new ResponseModel<CommentViewModel> { Data = res };
        }
        [HttpGet("GetComments/{itemId}")]
        public async Task<ResponseModel<List<CommentViewModel>>> GetComments([FromRoute] Guid itemId)
        {
            await Task.CompletedTask;
            return new() { };
        }
    }
}
