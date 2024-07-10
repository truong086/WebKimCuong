using System.ComponentModel.DataAnnotations;
using thuongmaidientus1.Common;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public interface IOrderService
    {
        Task<PayLoad<CertifyMst>> AddOrder(string? name, IList<XulydonhangDTO> donhang);
        Task<PayLoad<string>> DeleteOrder(IList<string> id);
        Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 10);
        Task<PayLoad<XulydonhangDTO>> AddDonHangXuLy(IList<XulydonhangDTO> xulydonhangs);
        Task<PayLoad<XulydonhangDTO>> UpdateDonHangXuLy(int product_id, string? name, string? account_name, Status? status = Status.Pending);
        Task<PayLoad<object>> FindAllDonHangXuLy(string? name, int page = 1, int pageSize = 20);
        Task<PayLoad<object>> FindAllDonHangByMax(string? name, string? account_name, int page = 1, int pageSize = 20);
        Task<PayLoad<object>> FindAllAccountDonHangByMax(string? name, string? account_name, int page = 1, int pageSize = 20);
        Task<PayLoad<object>> FindAllShopDonHangByMax(string? name, string? account_name, int page = 1, int pageSize = 20);
        Task<PayLoad<object>> FindAllShopProductMax(string? name, string? account_name, int page = 1, int pageSize = 20);
    }

    
    // Class tiện ích để chuyển đổi enum thành chuỗi
    public static class EnumHelper
    {
        // Hàm lấy ra thuộc tính trong enum(không phải lấy ra "[Display(Name = "")]")
        public static string GetEnumString<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            return value.ToString();
        }
    }
}
