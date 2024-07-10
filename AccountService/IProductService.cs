using System.Runtime.CompilerServices;
using thuongmaidientus1.Common;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public interface IProductService
    {
        Task<PayLoad<ProductDTO>> AddProduct(ProductDTO productDTO);
        Task<PayLoad<ProductDTO>> EditProduct(int id, ProductDTO productDTO, string? name);
        Task<PayLoad<string>> DeleteProduct(IList<string> id);
        Task<PayLoad<object>> FindAll(string? name, string? account_name, string? type, string? thutu, int page = 1, int pageSize = 10);
        Task<PayLoad<object>> FindAllProductCategory(IList<string> id, int page = 1, int pageSize = 20);
        Task<PayLoad<object>> FindOne(int id, string? name);
        Task<PayLoad<object>> UpdateClick(int id);

    }
}
