using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thuongmaidientus1.AccountService;
using thuongmaidientus1.Common;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route(nameof(FindAll))]
        public async Task<PayLoad<object>> FindAll(string? name, string? account_name, string? type, string thutu, int page = 1, int pageSize = 10)
        {
            return await _productService.FindAll(name, account_name, type, thutu, page, pageSize);
        }

        [HttpGet]
        [Route(nameof(FindOneId))]
        public async Task<PayLoad<object>> FindOneId(int id, string? name)
        {
            return await _productService.FindOne(id, name);
        }

        [HttpPost]
        [Route(nameof(FindAllProductCategory))]
        public async Task<PayLoad<object>> FindAllProductCategory([FromBody] IList<string> id, int page = 1, int pageSize = 10)
        {
            return await _productService.FindAllProductCategory(id, page, pageSize);
        }

        [HttpPost]
        [Route(nameof(AddProduct))]
        public async Task<PayLoad<ProductDTO>> AddProduct(ProductDTO productDTO)
        {
            return await _productService.AddProduct(productDTO);
        }

        [HttpPut]
        [Route(nameof(EditProduct))]
        public async Task<PayLoad<ProductDTO>> EditProduct(int id, ProductDTO productDTO, string? name)
        {
            return await _productService.EditProduct(id, productDTO, name);
        }
        [HttpDelete]
        [Route(nameof(DeleteProduct))]
        public async Task<PayLoad<string>> DeleteProduct(IList<string> id)
        {
            return await _productService.DeleteProduct(id);
        }
    }
}
