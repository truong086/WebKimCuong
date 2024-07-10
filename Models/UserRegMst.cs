using System.Data;

namespace thuongmaidientus1.Models
{
    public class UserRegMst : BaseEntity
    {
        public string? username { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public string? phonenumber { get; set; }
        public bool Action { get; set; }
        // Khóa ngoại
        //public int role_id { get; set; }
        public Roles? role { get; set; }
        public string? image { get; set; }
        public virtual IList<Inquiry>? Shops { get; set; }
        public virtual IList<ProdMst>? Products { get; set; }
        public virtual IList<CatMst>? Categories { get; set; }
        public virtual IList<CertifyMst>? Orders { get; set; }  
        public virtual IList<JewelTypeMst>? Xulydonhangs { get; set; }
        public virtual IList<CommentDescription>? commentDescriptions { get; set; }
        public virtual IList<Comment>? Comments { get; set; }
        public virtual IList<Danhgia>? Danhgia { get; set; }
    }
}
