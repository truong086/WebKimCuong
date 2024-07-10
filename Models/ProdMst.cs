namespace thuongmaidientus1.Models
{
    public class ProdMst : BaseEntity
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public string? image { get; set; }
        public float price { get; set; }
        public int click { get; set; }
        //public int account_id { get; set; }
        public virtual UserRegMst? Accounts { get; set; }
        public virtual CatMst? Categorys { get; set; }
        public virtual Inquiry? Shops { get; set; }
        public virtual IList<DimMst>? OrderDetails { get; set; }
        public virtual IList<ItemMst>? CategoryProduct { get; set; }
        public virtual IList<GoldKrtMst>? ProductImages { get; set; }
        public virtual IList<Comment>? Comments { get; set; }
        public virtual IList<Danhgia>? Danhgia { get; set; }
    }
}
