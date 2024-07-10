using thuongmaidientus1.Common;
using thuongmaidientus1.Models;

namespace thuongmaidientus1.AccountService
{
    public interface ICategoryService
    {
        Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 20);
        Task<PayLoad<CatMst>> FindOneId(int id);
        Task<PayLoad<CatMst>> AddCategory(CatMst category);
        Task<PayLoad<CatMst>> UpdateCategory(int id, CatMst category, string? name);
        Task<PayLoad<string>> DeleteCategory(IList<string> id);


    }
}
