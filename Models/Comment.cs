namespace thuongmaidientus1.Models
{
    public class Comment : BaseEntity
    {
        public string? message { get; set; }
        public byte[]? images { get; set; }
        public UserRegMst? accounts { get; set; }
        public ProdMst? products { get; set; }

        public virtual IList<CommentDescription>? commentDescriptions { get; set; }
    }
}
