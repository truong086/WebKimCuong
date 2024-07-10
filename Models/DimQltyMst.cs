namespace thuongmaidientus1.Models
{
    public class DimQltyMst : BaseEntity
    {
        public string? name { get; set; }
        public string? diachi { get; set; }
        public string? image { get; set; }

        public virtual IList<Inquiry>? Shops { get; set; }
        public virtual IList<StoneMst>? ShopVanchuyens { get; set; }
        public virtual IList<JewelTypeMst>? Vanchuyens { get; set; }
    }
}
