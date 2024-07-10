namespace thuongmaidientus1.Models
{
    public class Danhgia : BaseEntity
    {
        public int sao { get; set; }
        public string? message { get; set; }
        public UserRegMst? account { get; set; }
        public ProdMst? product { get; set; }

    }
}
