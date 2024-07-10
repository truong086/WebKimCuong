namespace thuongmaidientus1.Models
{
    public class ItemMst : BaseEntity
    {
        public virtual ProdMst? Product { get; set; }
        public virtual CatMst? Category { get; set; }
    }
}
