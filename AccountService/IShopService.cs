using thuongmaidientus1.Common;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public interface IShopService
    {
        Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 20);
        Task<PayLoad<object>> FindOneIdOrName(IList<string> name);

        Task<PayLoad<ShopDTO>> AddShop(ShopDTO shop);
        Task<PayLoad<ShopDTO>> EditShop(int id, ShopDTO shop, string? username);
        Task<PayLoad<string>> DeleteShop(IList<string> id);


    }
}
