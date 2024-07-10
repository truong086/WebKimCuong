namespace thuongmaidientus1.Models
{
    // Bảng lưu chữ ký thuật toán
    public class PaymentSignature : BaseEntity
    {
        public string? SignValue {  get; set; } // Lưu nội dung giá trị của chữ ký này
        public string? SignAlgo { get; set; } // Lưu thuật toán tạo ra chữ ký này
        public DateTimeOffset? SignDate { get; set; }
        public string? SignOwn {  get; set; } // Để chữ ký của bên nào(Bên tạo ra chữ ký)
        public Payment? PaymentId {  get; set; }
        public bool? IsValid { get; set; }
    }
}
