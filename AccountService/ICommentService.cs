using thuongmaidientus1.Common;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public interface ICommentService
    {
        Task<PayLoad<object>> FindAll(string name, int page, int pageSize);
        Task<PayLoad<CommentDTO>> CreateComment(CommentDTO comment);
        Task<PayLoad<CommentDescriptionDTO>> CreateCommentDescription(CommentDescriptionDTO commentDescription);
        Task<PayLoad<CommentDescriptionDTO>> UpdateComment(int id, CommentDescriptionDTO comment);
        Task<PayLoad<CommentDTO>> DeleteComment(IList<string> id);
    }
}
