namespace thuongmaidientus1.Models
{
    public class Inquiry : BaseEntity
    {
        public string? Name { get; set; }
        public string? diachi { get; set; }
        public string? email { get; set;}
        public string? sodienthoai { get; set;}
        public string? image { get; set;}
        public UserRegMst? account { get; set;}
        public DimQltyMst? vanchuyen { get; set; }
        public virtual IList<ProdMst>? Products { get; set; }
        public virtual IList<StoneMst>? ShopVanchuyens { get; set; }

    }
}
