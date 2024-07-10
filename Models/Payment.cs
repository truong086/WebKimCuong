namespace thuongmaidientus1.Models
{
    public class Payment : BaseEntity
    {
		public string? paymentId { get; set; }
		public string? PaymentContent { get; set; }
        public string? PaymentCurrency { get; set; }
        public int? PaymentRefId { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public DateTimeOffset? ExpireDate { get; set; }
        public string? PaymentLanguage { get; set; }
        public Merchant? MerchantId { get; set; }
        public PaymentDescription? PaymentDestinationId { get; set; }
        public decimal? PaidAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentLastMessage { get; set; }
        public IList<PaymentNotification>? PaymentNotifications { get; set; }
        public IList<PaymentSignature>? PaymentSignatures { get; set; }
        public IList<PaymentTransaction>? PaymentTransactions { get; set; }
    }
}
