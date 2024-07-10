using AutoMapper;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using thuongmaidientus1.Common;
using thuongmaidientus1.EmailConfig;
using thuongmaidientus1.Models;

namespace thuongmaidientus1.AccountService
{
    public class CategoryService : ICategoryService
    {
        private readonly DBContexts _dbcontext;
        private readonly IMapper _mapper;
        private readonly EmailSetting _emaiSetting;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly Jwt _jwt;
        private readonly IUserService _userService;
        public CategoryService(DBContexts dbcontext, IMapper mapper, IOptions<EmailSetting> emailSetting, IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IOptionsMonitor<Jwt> jwt, IUserService userService)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _emaiSetting = emailSetting.Value;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _jwt = jwt.CurrentValue;
            _userService = userService;
        }
        public async Task<PayLoad<CatMst>> AddCategory(CatMst category)
        {
            try
            {
                var checkName = _dbcontext.CatMsts.Where(x => x.name == category.name).FirstOrDefault();
                if(checkName != null)
                {
                    return await Task.FromResult(PayLoad<CatMst>.CreatedFail("Thể loại đã tồn tại"));
                }

                _dbcontext.CatMsts.Add(category);
                if(await _dbcontext.SaveChangesAsync() > 0)
                {
                    return await Task.FromResult(PayLoad<CatMst>.Successfully(category));
                }
                return await Task.FromResult(PayLoad<CatMst>.CreatedFail("Add Faild"));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(PayLoad<CatMst>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<string>> DeleteCategory(IList<string> id)
        {
            string? message = null;
            try
            {
                if(!id.Any() || id.Count <= 0 || id == null)
                {
                    message = "Dữ liệu rỗng";
                    return await Task.FromResult(PayLoad<string>.CreatedFail(message));
                }

                for(var i = 0; i < id.Count; i++)
                {
                    var data = id[i];
                    bool checkInt = Regex.IsMatch(data, @"^\d+$");
                    if(int.TryParse(data, out int songuyen))
                    {
                        var checkId = _dbcontext.CatMsts.Where(x => x.id == songuyen).FirstOrDefault();
                        if(checkId != null)
                        {
                            checkId.Deleted = true;
                            _dbcontext.CatMsts.Update(checkId);
                        }
                    }
                    if (!checkInt)
                    {
                        var checkName = _dbcontext.CatMsts.Where(x => x.name.Contains(data)).ToList();
                        if(checkName.Count > 0)
                        {
                            var listRemoveCategory = new List<CatMst>();
                            foreach(var item in checkName)
                            {
                                item.Deleted = true;
                                listRemoveCategory.Add(item);
                            }

                            _dbcontext.CatMsts.UpdateRange(listRemoveCategory);

                        }
                    }
                }

                if(await _dbcontext.SaveChangesAsync() <= 0)
                {
                    message = "Delete Faild";
                    return await Task.FromResult(PayLoad<string>.CreatedFail(message));
                }

                message = "Success";
                return await Task.FromResult(PayLoad<string>.Successfully(message));

            }
            catch(Exception ex)
            {
                message = ex.Message;
                return await Task.FromResult(PayLoad<string>.CreatedFail(message));
            }
        }

        public async Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 20)
        {
            try
            {
                page = page <= 0 ? 1 : page;
                pageSize = pageSize <= 0 ? 20 : pageSize;
                var data = _dbcontext.CatMsts.Include(a => a.account).Where(x => !x.Deleted).Select(x => new
                {
                    Id = x.id,
                    name = x.name,
                    image = x.images,
                    accountName = x.account.username
                }).AsQueryable();

                var pageList = await PageList<object>.Create(data, page, pageSize);
                return await Task.FromResult(PayLoad<object>.Successfully(new
                {
                    data = pageList,
                    page,
                    pageList.pageSize,
                    pageList.totalCounts,
                    pageList.totalPages
                }));
            }catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<object>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<CatMst>> FindOneId(int id)
        {
            var data = _dbcontext.CatMsts.Include(a => a.account).Where(x => x.id == id).Select(x => new
            {
                Id = x.id,
                name = x.name,
                image = x.images,
                accountName = x.account
            }).FirstOrDefault();

            return await Task.FromResult(PayLoad<CatMst>.Successfully(new CatMst { id = data.Id, name = data.name, images = data.image, account = data.accountName}));
        }

        public async Task<PayLoad<CatMst>> UpdateCategory(int id, CatMst category, string? name)
        {
            try
            {
                var checkId = _dbcontext.CatMsts.Include(a => a.account).Where(x => x.id == id).FirstOrDefault();
                if(checkId == null) {
                    return await Task.FromResult(PayLoad<CatMst>.CreatedFail("Dữ liệu không tồn tại"));
                }
                var checkAccountName = _dbcontext.UserRegMsts.Where(x => x.username == name).FirstOrDefault();
                if(checkAccountName != null)
                {
                    checkId.CretorEdit = checkAccountName.username + " là người đã chỉnh sửa bản ghi vào lúc " + DateTimeOffset.UtcNow;
                }

                var checkNameAction = _dbcontext.CatMsts.Where(x => x.name != checkId.name && x.name == category.name).FirstOrDefault();
                if(checkNameAction != null)
                {
                    return await Task.FromResult(PayLoad<CatMst>.CreatedFail("Dữ liệu đã tồn tại"));
                }

                checkId.account = checkAccountName;
                checkId.name = category.name;
                checkId.images = category.images;

                _dbcontext.CatMsts.Update(checkId);
                if(await _dbcontext.SaveChangesAsync() > 0)
                {
                    return await Task.FromResult(PayLoad<CatMst>.Successfully(category));
                }

                return await Task.FromResult(PayLoad<CatMst>.CreatedFail("Update Faild"));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<CatMst>.CreatedFail(ex.Message));
            }
        }
    }
}
