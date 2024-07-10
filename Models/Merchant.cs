namespace thuongmaidientus1.Models
{
    public class Merchant : BaseEntity
    {
        public string? MerchantName { get; set; }   
        public string? MerchantWebLink { get; set; }   
        public string? MerchantIpnUrl { get; set; }   // Trường dữ liệu này trả dữ liệu về phía BackEnd 
        public string? MerchantReturnUrl { get; set; }   
        public string? SecretKey { get; set; }   
        public bool? IsActive { get; set; } 
        public virtual IList<Payment>? Payments { get; set; }

    }
}
