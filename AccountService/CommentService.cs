using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using thuongmaidientus1.Common;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public class CommentService : ICommentService
    {
        private readonly DBContexts _context;
        private readonly IMapper _mapper;
        private readonly Lazy<IDanhGiaService> _daiaService;

        // Sử dụng "Lazy" để gọi Service "IDanhGiaService" trong Service "CommentService"
        public CommentService(DBContexts context, IMapper mapper, Lazy<IDanhGiaService> danhGiaService)
        {
            _context = context;   
            _mapper = mapper;
            _daiaService = danhGiaService;
        }
        public async Task<PayLoad<CommentDTO>> CreateComment(CommentDTO comment)
        {
            //var checks = _daiaService.Value;
            //var danhgia = new DanhgiaDTO();
            //var check = checks.AddDanhgia(danhgia);

            var checkAccount = _context.UserRegMsts.Where(x => x.id == comment.account_id).FirstOrDefault();
            if(checkAccount == null)
            {
                return await Task.FromResult(PayLoad<CommentDTO>.BadRequest());
            }
            var checkProduct = _context.ProdMsts.Where(x => x.id == comment.product_id).FirstOrDefault();
            if (checkProduct == null)
            {
                return await Task.FromResult(PayLoad<CommentDTO>.BadRequest());
            }

            var mapData = _mapper.Map<Comment>(comment);
            mapData.products = checkProduct;
            mapData.accounts = checkAccount;

            _context.comments.Add(mapData);
            if(await _context.SaveChangesAsync() > 0) { 
                return await Task.FromResult(PayLoad<CommentDTO>.Successfully(new CommentDTO
                {
                    message = comment.message,
                    images = comment.images,
                }));
            }

            return await Task.FromResult(PayLoad<CommentDTO>.CreatedFail("Add Faild"));
        }

        public async Task<PayLoad<CommentDescriptionDTO>> CreateCommentDescription(CommentDescriptionDTO commentDescription)
        {
            try
            {
                if(commentDescription.commentDescription_ID == 0)
                {
                    var checkComment = _context.comments.Where(x => x.id == commentDescription.comment_ID).FirstOrDefault();
                    if(checkComment == null)
                    {
                        return await Task.FromResult(PayLoad<CommentDescriptionDTO>.NotFound("Comment không tồn tại!"));
                    }

                    var checkAccount = _context.UserRegMsts.Where(x => x.id == commentDescription.account_ID).FirstOrDefault();
                    if (checkAccount == null)
                    {
                        return await Task.FromResult(PayLoad<CommentDescriptionDTO>.NotFound("Faild"));
                    }
                    var mapData = _mapper.Map<CommentDescription>(commentDescription);
                    mapData.comment = checkComment;
                    mapData.account = checkAccount;

                    _context.commentDescriptions.Add(mapData);

                }
                if(commentDescription.comment_ID == 0)
                {
                    var checkCommentDescription = _context.commentDescriptions.Where(x => x.id == commentDescription.commentDescription_ID).FirstOrDefault();
                    
                    if(checkCommentDescription == null)
                    {
                        return await Task.FromResult(PayLoad<CommentDescriptionDTO>.NotFound("Faild"));
                    }
                    var checkAccount = _context.UserRegMsts.Where(x => x.id == commentDescription.account_ID).FirstOrDefault();
                    if(checkAccount == null)
                    {
                        return await Task.FromResult(PayLoad<CommentDescriptionDTO>.NotFound("Faild"));
                    }
                    var mapDataDescription = _mapper.Map<CommentDescription>(commentDescription);
                    mapDataDescription.commentDescriptions = checkCommentDescription;
                    mapDataDescription.account = checkAccount;

                    _context.commentDescriptions.Add(mapDataDescription);

                }

                if(commentDescription.comment_ID != 0 && commentDescription.commentDescription_ID > 0)
                {
                    var checkCommentId = _context.comments.Where(x => x.id == commentDescription.comment_ID).FirstOrDefault();
                    var checkCommentDescId = _context.commentDescriptions.Where(x => x.id == commentDescription.commentDescription_ID).FirstOrDefault();
                    var checkAcc = _context.UserRegMsts.Where(x => x.id == commentDescription.account_ID).FirstOrDefault();

                    if(checkCommentId == null)
                        return await Task.FromResult(PayLoad<CommentDescriptionDTO>.CreatedFail("Lỗi Add"));
                    if(checkCommentDescId == null)
                        return await Task.FromResult(PayLoad<CommentDescriptionDTO>.CreatedFail("Lỗi Add"));

                    var mapDataRep = _mapper.Map<CommentDescription>(commentDescription);
                    mapDataRep.account = checkAcc;
                    mapDataRep.comment = checkCommentId;
                    mapDataRep.commentDescriptions = checkCommentDescId;

                    _context.commentDescriptions.Add(mapDataRep);
                }

                if(await _context.SaveChangesAsync() < 0)
                {
                    return await Task.FromResult(PayLoad<CommentDescriptionDTO>.CreatedFail("Lỗi Add"));
                }
                return await Task.FromResult(PayLoad<CommentDescriptionDTO>.Successfully(commentDescription));
            }catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<CommentDescriptionDTO>.NotFound(ex.Message));
            }

        }

        public async Task<PayLoad<CommentDTO>> DeleteComment(IList<string> id)
        {
            try
            {
                if (id == null || !id.Any())
                    return await Task.FromResult(PayLoad<CommentDTO>.CreatedFail("Null"));

                foreach(var item in id)
                {
                    bool checkInt = Regex.IsMatch(item, @"^\d+$");
                    if(int.TryParse(item, out int songuyen))
                    {
                        var checkId = _context.comments.Where(x => x.id == songuyen).FirstOrDefault();
                        if(checkId == null)
                        {
                            return await Task.FromResult(PayLoad<CommentDTO>.CreatedFail("Id Null"));
                        }
                        var checkIdCommentDesc = _context.commentDescriptions.Where(x => x.comment == checkId).ToList();
                        if(checkIdCommentDesc.Any())
                        {
                            for(var i = 0; i < checkIdCommentDesc.Count; i++)
                            {
                                var data = checkIdCommentDesc[i];
                                var checkCommentDescSup = _context.commentDescriptions.Where(x => x.commentDescriptions == data).ToList();
                                if (checkCommentDescSup.Any())
                                {
                                    for(var j = 0; j < checkCommentDescSup.LongCount(); j++)
                                    {
                                        var dataSup = checkCommentDescSup[j];
                                        var checkCommenttSubCon = _context.commentDescriptions.Where(x => x.commentDescriptions.Equals(dataSup)).ToList();
                                        if (checkCommenttSubCon.Any())
                                        {
                                            for(var k = 0; k < checkCommenttSubCon.LongCount(); k++)
                                            {
                                                var supData = checkCommenttSubCon[k];
                                                var checkSubCon = _context.commentDescriptions.Where(x => x.commentDescriptions.Equals(supData)).ToList();
                                                if (checkSubCon.Any())
                                                {
                                                    foreach (var subCon in checkSubCon)
                                                    {
                                                        var checkCon = _context.commentDescriptions.Where(x => x.commentDescriptions.Equals(subCon)).ToList();
                                                        if (checkCon.Any())
                                                        {
                                                            _context.commentDescriptions.RemoveRange(checkCon);
                                                        }
                                                    }


                                                    _context.commentDescriptions.RemoveRange(checkSubCon);
                                                }
                                            }
                                            
                                            _context.commentDescriptions.RemoveRange(checkCommenttSubCon);

                                        }
                                    }
                                    _context.commentDescriptions.RemoveRange(checkCommentDescSup);
                                }

                            }

                        }
                        _context.commentDescriptions.RemoveRange(checkIdCommentDesc);
                        _context.comments.Remove(checkId);

                        if(await _context.SaveChangesAsync() > 0)
                        {
                            return await Task.FromResult(PayLoad<CommentDTO>.Successfully(new CommentDTO
                            {
                                message = "Delete Success"
                            }));
                        }

                        

                    }

                }
                return await Task.FromResult(PayLoad<CommentDTO>.CreatedFail("Delete Faild"));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<CommentDTO>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<object>> FindAll(string name, int page, int pageSize)
        {
            try
            {
                page = page < 1 ? 1 : page;
                pageSize = pageSize < 1 ? 20 : pageSize;


                // Cách 1
                //var list = new List<CommnentList>();

                //var data = _context.comments.ToList();

                //for(var i = 0; i < data.Count; i++)
                //{
                //    var dataItem = data[i];
                //    var listComment = new CommnentList();
                //    listComment.id = dataItem.id;
                //    listComment.message = dataItem.message;
                //    //listComment.images = dataItem.images;

                //    var commentRep = _context.commentDescriptions.Where(x => x.comment == dataItem).ToList();
                //    if(commentRep != null || commentRep.Any())
                //    {


                //        for (var j = 0; j < commentRep.LongCount(); j++)
                //        {
                //            var commentReplist = new CommnetRepList();
                //            var dataRep = commentRep[j];
                //            commentReplist.id = dataRep.id;
                //            commentReplist.message = dataRep.message;
                //            var checkRepcomment = _context.commentDescriptions.Where(x => x.commentDescriptions == dataRep).ToList();
                //            if(checkRepcomment.Any() || checkRepcomment != null)
                //            {
                //                for(var k = 0; k < checkRepcomment.LongCount() ; k++)
                //                {
                //                    var rep = new CommnetRepList();
                //                    rep.id = checkRepcomment[k].id;

                //                    commentReplist.commentRep.Add(rep);
                //                }
                //            }

                //            listComment.commentRep.Add(commentReplist);

                //        }

                //    }

                //    list.Add(listComment);
                //}

                // Cách 2
                var list = _context.comments.Include(a => a.accounts).ThenInclude(d => d.Danhgia).Include(p => p.products).Include(x => x.commentDescriptions).ThenInclude(ac => ac.account).Select(x => new
                {
                    id = x.id,
                    message = x.message,
                    accountName = x.accounts.username,
                    danhgia = x.accounts.Danhgia.Sum(da => da.sao),
                    //image = x.images,
                    commentDescription = x.commentDescriptions.Select(d => new
                    {
                        id = d.id,
                        messagedescription = d.message,
                        account_Name = d.account.username,
                        danhgia = x.accounts.Danhgia.Sum(da => da.sao),
                        commentdescriptionrep = d.commentDescriptionsList.Select(c => new
                        {
                            IDs = c.id,
                            account_username = c.account.username,
                            danhgia = x.accounts.Danhgia.Sum(da => da.sao)

                        }).ToList(),
                    }).ToList(),
                }).ToList();

                var pageList = new PageList<object>(list, page - 1, pageSize);

                return await Task.FromResult(PayLoad<object>.Successfully(new
                {
                    data = pageList,
                    page = pageList.pageIndex,
                    pagesize = pageList.pageSize,
                    totalPage = pageList.totalPages,
                    totalCount = pageList.totalCounts
                }));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<object>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<CommentDescriptionDTO>> UpdateComment(int id, CommentDescriptionDTO comment)
        {
            try
            {
                if(comment.commentDescription_ID == 0)
                {
                    var checkIdComment = _context.comments.Where(x => x.id == id).FirstOrDefault();
                    if(checkIdComment == null)
                    {
                        return await Task.FromResult(PayLoad<CommentDescriptionDTO>.NotFound("Null"));
                    }

                    checkIdComment.message = comment.message;

                    _context.comments.Update(checkIdComment);
                }   
                if(comment.comment_ID == 0)
                {
                    var checkIdCommentDescription = _context.commentDescriptions.Where(x => x.id == id).FirstOrDefault();
                    if (checkIdCommentDescription == null)
                    {
                        return await Task.FromResult(PayLoad<CommentDescriptionDTO>.NotFound("Null"));
                    }

                    checkIdCommentDescription.message = comment.message;

                    _context.commentDescriptions.UpdateRange(checkIdCommentDescription);

                }

                if(await _context.SaveChangesAsync() > 0)
                {
                    return await Task.FromResult(PayLoad<CommentDescriptionDTO>.Successfully(new CommentDescriptionDTO
                    {
                        message = comment.message
                    }));
                }

                return await Task.FromResult(PayLoad<CommentDescriptionDTO>.CreatedFail("Edit Faild"));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<CommentDescriptionDTO>.NotFound(ex.Message));
            }
        }
    }
}
