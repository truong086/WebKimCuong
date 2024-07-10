namespace thuongmaidientus1.Models
{
    public class PaymentTransaction : BaseEntity
    {
        public string? TranMessage { get; set; }
        public string? TranPayLoad { get; set; }
        public string? TranStatus { get; set; }
        public decimal? TranAmount { get; set; }
        public DateTimeOffset? TranDate { get; set; }
        public Payment? PaymentId { get; set; }

        
    }
}
