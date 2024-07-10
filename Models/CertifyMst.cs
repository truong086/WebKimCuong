namespace thuongmaidientus1.Models
{
    public class CertifyMst : BaseEntity
    {
        public string? orderName { get; set; }
        public string? status { get; set; }
        //public int account_id { get; set; }
        public virtual UserRegMst? Accounts { get; set; }
        public virtual IList<DimMst>? OrderDetails { get; set; }
    }
}
