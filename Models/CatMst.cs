namespace thuongmaidientus1.Models
{
    public class CatMst : BaseEntity
    {
        public string? name { get; set; }
        public string? images { get; set; }
        public string? creatorId { get; set; }
        public virtual UserRegMst? account { get; set; }
        public virtual IList<ItemMst>? CategoryProduct { get; set; }
        public virtual IList<ProdMst>? Products { get; set; }
    }
}
