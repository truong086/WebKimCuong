using thuongmaidientus1.Common;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public interface IDanhGiaService
    {
        Task<PayLoad<DanhgiaDTO>> AddDanhgia(DanhgiaDTO danhgia);
        Task<PayLoad<object>> FindAll (string? name, int page = 1, int pageSize = 10);
        Task<PayLoad<DanhgiaDTO>> EditDanhgia(int id,  DanhgiaDTO danhgia);
        Task<PayLoad<string>> DeleteDanhgia(IList<string> id);
    }
}
