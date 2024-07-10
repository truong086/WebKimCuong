using thuongmaidientus1.Common;
using thuongmaidientus1.Models;

namespace thuongmaidientus1.AccountService
{
    public interface IVanChuyenService
    {
        Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 20);
        Task<PayLoad<object>> FindOneIdOrName(IList<string> id);
        Task<PayLoad<DimQltyMst>> AddVanChuyen(DimQltyMst vanchuyen);
        Task<PayLoad<DimQltyMst>> EditVanChuyen(int id, DimQltyMst vanchuyen);
        Task<PayLoad<string>> DeleteVanChuyen(IList<string> id);

    }
}
