namespace thuongmaidientus1.Models
{
    public class PaymentNotification : BaseEntity
    {
        public int? PaymentRefId { get; set; }
        public string? NotiDate { get; set; }
        public string? NotiAmount { get; set; }
        public string? NotiContent { get; set; }
        public string? NotiMessage { get; set; }
        public string? NotiSignature { get; set; }
        public Payment? PaymentId { get; set; }
        public int? MerchantId { get; set; }
        public string? NotiSatus { get; set; }
        public string? NotiResDate { get; set; }
        
        

    }
}
