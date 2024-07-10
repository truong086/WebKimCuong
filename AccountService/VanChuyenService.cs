using AutoMapper;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using thuongmaidientus1.Common;
using thuongmaidientus1.EmailConfig;
using thuongmaidientus1.Models;

namespace thuongmaidientus1.AccountService
{
    public class VanChuyenService : IVanChuyenService
    {
        private readonly DBContexts _dbcontext;
        private readonly IMapper _mapper;
        private readonly EmailSetting _emaiSetting;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        public VanChuyenService(DBContexts dbcontext, IMapper mapper, IOptions<EmailSetting> emailSetting, IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _emaiSetting = emailSetting.Value;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }
        public async Task<PayLoad<DimQltyMst>> AddVanChuyen(DimQltyMst vanchuyen)
        {
            try
            {
                vanchuyen.name = vanchuyen.name.ToLower(); // Chuyển hết sang chữ thường(không viết hoa)
                var checkName = _dbcontext.DimQltyMst.Where(x => x.name.ToLower() == vanchuyen.name && !x.Deleted).FirstOrDefault(); // Chuyển hết dữ liệu về chữ thường để kiểm tra xem đã tồn tại chưa
                if(checkName != null) {
                    return await Task.FromResult(PayLoad<DimQltyMst>.CreatedFail("Tên công ty vận chuyển đã tồn tại"));
                }

                vanchuyen.Deleted = false;
                _dbcontext.DimQltyMst.Add(vanchuyen);
                if(await _dbcontext.SaveChangesAsync() <= 0)
                {
                    return await Task.FromResult(PayLoad<DimQltyMst>.CreatedFail("Faild"));
                }
                return await Task.FromResult(PayLoad<DimQltyMst>.Successfully(vanchuyen));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<DimQltyMst>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<string>> DeleteVanChuyen(IList<string> id)
        {
            string? message = null;
            if(!id.Any() || id == null || id.Count <= 0)
            {
                message = "Dữ liệu null";
                return await Task.FromResult(PayLoad<string>.CreatedFail(message));
            }

            for(var i = 0; i < id.Count; i++)
            {
                var data = id[i];
                bool checkInt = Regex.IsMatch(data, @"^\d+$");
                if(int.TryParse(data, out var chuyenId))
                {
                    var checkId = _dbcontext.DimQltyMst.FirstOrDefault(x => x.id == chuyenId);
                    if(checkId != null)
                    {
                        checkId.Deleted = true;
                        _dbcontext.DimQltyMst.Update(checkId);
                    }
                    
                }
                if(!checkInt)
                {
                    var checkName = _dbcontext.DimQltyMst.FirstOrDefault(x => x.name == data);
                    if(checkName != null)
                    {
                        checkName.Deleted = true;
                        _dbcontext.DimQltyMst.Update(checkName);
                    }
                }
            }

            if(await _dbcontext.SaveChangesAsync() > 0)
            {
                message = "Success";
                return await Task.FromResult(PayLoad<string>.Successfully(message));
            }

            message = "Faild";
            return await Task.FromResult(PayLoad<string>.CreatedFail(message));
        }

        public async Task<PayLoad<DimQltyMst>> EditVanChuyen(int id, DimQltyMst vanchuyen)
        {
            try
            {
                var checkId = _dbcontext.DimQltyMst.Where(x => x.id ==  id).FirstOrDefault();
                if(checkId == null)
                {
                    return await Task.FromResult(PayLoad<DimQltyMst>.CreatedFail("Chưa tồn tại"));
                }

                var checkNameVanChuyen = _dbcontext.DimQltyMst.Where(x => x.name != checkId.name && x.name == vanchuyen.name).FirstOrDefault();
                if(checkNameVanChuyen != null)
                {
                    return await Task.FromResult(PayLoad<DimQltyMst>.CreatedFail("Tên công ty vận chuyển đã tồn tại"));
                }

                checkId.name = vanchuyen.name;
                checkId.diachi = vanchuyen.diachi;
                checkId.Deleted = false;
                _dbcontext.DimQltyMst.Update(checkId);
                if(await _dbcontext.SaveChangesAsync() > 0)
                {
                    return await Task.FromResult(PayLoad<DimQltyMst>.Successfully(vanchuyen));
                }
                return await Task.FromResult(PayLoad<DimQltyMst>.CreatedFail("Update Faild"));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<DimQltyMst>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 20)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 20 : pageSize;
            var data = _dbcontext.DimQltyMst.Where(x => !x.Deleted).AsQueryable();
            if(!string.IsNullOrEmpty(name))
                data = data.Where(x => x.name.Contains(name)).AsQueryable();

            var pageList = await PageList<object>.Create(data, page, pageSize);

            return await Task.FromResult(PayLoad<object>.Successfully(new
            {
                data = pageList,
                page,
                pageList.pageSize,
                pageList.totalCounts,
                pageList.totalPages
            }));
        }

        public async Task<PayLoad<object>> FindOneIdOrName(IList<string> id)
        {
            try
            {
                if(id == null || id.Count <= 0 || !id.Any())
                {
                    return await Task.FromResult(PayLoad<object>.CreatedFail("Id null"));
                }

                var listData = new List<object>();
                for(var i = 0; i < id.Count; i++)
                {
                    var data = id[i];
                    bool check = Regex.IsMatch(data, @"^\d+$");
                    if(int.TryParse(data, out int songuyen))
                    {
                        var checkId = _dbcontext.DimQltyMst.Where(x => x.id == songuyen && !x.Deleted).FirstOrDefault();
                        if(checkId != null)
                        {
                            listData.Add(checkId);
                        }
                    }
                    if (!check)
                    {
                        var checkName = _dbcontext.DimQltyMst.Where(x => x.name.Contains(data) && !x.Deleted).ToList();
                        if(checkName != null)
                        {
                            listData.Add(checkName);
                        }
                    }

                }

                return await Task.FromResult(PayLoad<object>.Successfully(listData));
            }catch(Exception ex) {
                return await Task.FromResult(PayLoad<object>.CreatedFail(ex.Message));
            }
        }
    }
}
