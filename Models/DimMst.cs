namespace thuongmaidientus1.Models
{
    public class DimMst : BaseEntity
    {
        public int quantity { get; set; }
        public int total { get; set; }
        public float price { get; set; }
        //public int order_id { get; set; }
        public virtual CertifyMst? Orders { get; set; }
        //public int product_id { get; set; }
        public virtual ProdMst? Products { get; set; }

    }
}
