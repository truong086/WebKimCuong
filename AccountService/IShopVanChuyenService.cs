using thuongmaidientus1.Common;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public interface IShopVanChuyenService
    {
        Task<PayLoad<List<ShopVanchuyenDTO>>> FindOneIdShop(int id);
    }
}
