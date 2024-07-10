using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thuongmaidientus1.AccountService;
using thuongmaidientus1.Common;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(CreateComment))]
        public async Task<PayLoad<CommentDTO>> CreateComment(CommentDTO commentDTO)
        {
            return await commentService.CreateComment(commentDTO);
        }

        [HttpPut]
        [AllowAnonymous]
        [Route(nameof(UpdateComment))]
        public async Task<PayLoad<CommentDescriptionDTO>> UpdateComment(int id, CommentDescriptionDTO commentDTO)
        {
            return await commentService.UpdateComment(id, commentDTO);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(CreateComemntDescription))]
        public async Task<PayLoad<CommentDescriptionDTO>> CreateComemntDescription(CommentDescriptionDTO commentDescriptionDTO)
        {
            return await commentService.CreateCommentDescription(commentDescriptionDTO);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(nameof(FindAll))]
        public async Task<PayLoad<object>> FindAll(string? name, int page, int pageSize)
        {
            return await commentService.FindAll(name, page, pageSize);
        }

        [HttpDelete]
        [AllowAnonymous]
        [Route(nameof(FindAll))]
        public async Task<PayLoad<CommentDTO>> DeleteComment(IList<string> id)
        {
            return await commentService.DeleteComment(id);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(FileImage))]
        public async Task<IActionResult> FileImage( IFormFile file)      
        {
            if(file == null || file.Length == 0)
            {
                return BadRequest();
            }

            using (var memoryStream = new MemoryStream())
            {
                // Đọc dữ liệu từ IFormFile vào một MemoryStream
                file.CopyTo(memoryStream);

                // Chuyển dữ liệu trong MemoryStream thành một mảng byte
                byte[] bytes = memoryStream.ToArray();

                // Mã hóa mảng byte thành chuỗi Base64
                string base64 = Convert.ToBase64String(bytes);

                return Ok(base64);  


            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(FileImageToArr))]
        public async Task<IActionResult> FileImageToArr(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }

            byte[] arrByte = await ConvertByteArr(file);
            return Ok(arrByte);
        }


        // Hàm chuyển ảnh sang mảng byte[]
        private async Task<byte[]> ConvertByteArr(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
