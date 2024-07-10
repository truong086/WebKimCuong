using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using thuongmaidientus1.Common;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public class DanhGiaService : IDanhGiaService
    {
        private readonly DBContexts _context;
        private readonly IMapper mapper;
        private readonly Lazy<ICommentService> _commentService;

        // Sử dụng "Lazy" để gọi Service "ICommentService" trong Service "DanhGiaService"
        public DanhGiaService(DBContexts context, IMapper mapplers, Lazy<ICommentService> commentService)
        {
            _context = context;
            mapper = mapplers;
            _commentService = commentService;

        }
        public async Task<PayLoad<DanhgiaDTO>> AddDanhgia(DanhgiaDTO danhgia)
        {
            try
            {
                //var checks = _commentService.Value;
                //var comment = new CommentDTO();
                //var check = checks.CreateComment(comment);


                var checkAccount = _context.UserRegMsts.Where(x => x.id == danhgia.account_id).FirstOrDefault();
                if(checkAccount == null)
                {
                    return await Task.FromResult(PayLoad<DanhgiaDTO>.CreatedFail("UserRegMst NUll"));
                }
                var checkProduct = _context.ProdMsts.Where(x => x.id == danhgia.product_id).FirstOrDefault();
                if(checkProduct == null)
                {
                    return await Task.FromResult(PayLoad<DanhgiaDTO>.CreatedFail("UserRegMst NUll"));
                }

                var checkAccountExsic = _context.danhGias.Include(a => a.account).Where(x => x.account.Equals(checkAccount)).FirstOrDefault();
                if(checkAccountExsic != null)
                {
                    return await Task.FromResult(PayLoad<DanhgiaDTO>.CreatedFail("UserRegMst Đã đánh giá"));
                }
                var data = mapper.Map<Danhgia>(danhgia);
                data.account = checkAccount;
                data.product = checkProduct;

                _context.danhGias.Add(data);

                if(await _context.SaveChangesAsync() > 0) {
                    return await Task.FromResult(PayLoad<DanhgiaDTO>.Successfully(danhgia));
                }
                return await Task.FromResult(PayLoad<DanhgiaDTO>.CreatedFail("Add Faild"));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<DanhgiaDTO>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<string>> DeleteDanhgia(IList<string> id)
        {
            try {
                if (!id.Any() || id.Count < 0)
                    return await Task.FromResult(PayLoad<string>.CreatedFail("Id Null"));

                for(var i = 0; i < id.Count; i++)
                {
                    var data = id[i];
                    bool checkInt = Regex.IsMatch(data, @"^\d+$");

                    if(int.TryParse(data, out int danhgiaId))
                    {
                        var danhgiaData = _context.danhGias.Where(x => x.id == danhgiaId).FirstOrDefault();

                        if(danhgiaData == null)
                            return await Task.FromResult(PayLoad<string>.CreatedFail("Data Null"));

                        danhgiaData.Deleted = true;

                        _context.danhGias.Update(danhgiaData);
                        if(await  _context.SaveChangesAsync() < 0)
                        {
                            return await Task.FromResult(PayLoad<string>.CreatedFail("Delete Faild"));
                        }
                    }
                    if (!checkInt)
                    {
                        
                    }
                }

                return await Task.FromResult(PayLoad<string>.Successfully("Delete Success"));
            }catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<string>.CreatedFail(ex.Message));
            }
        }

        public Task<PayLoad<DanhgiaDTO>> EditDanhgia(int id, DanhgiaDTO danhgia)
        {
            throw new NotImplementedException();
        }

        public async Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 10)
        {
            try
            {
                var total = 0;
                var checkProduct = _context.ProdMsts.Where(x => x.id == int.Parse(name)).FirstOrDefault();
                if(checkProduct == null)
                {
                    return await Task.FromResult(PayLoad<object>.CreatedFail("ProdMst Faild"));
                }
                var totalStart = await _context.danhGias.Where(x => x.product == checkProduct).SumAsync(x => x.sao);
                var totalAccount = await _context.danhGias.Where(x => x.product == checkProduct).CountAsync(x => x.id > 0);

                total = totalStart / totalAccount;

                return await Task.FromResult(PayLoad<object>.Successfully(new
                {
                    totalSao = total
                }));


            }catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<object>.CreatedFail(ex.Message));
            }
        }
    }
}
