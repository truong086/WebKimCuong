using System.ComponentModel.DataAnnotations.Schema;

namespace thuongmaidientus1.Models
{
    public class PaymentDescription : BaseEntity
    {
        public string? DesLogo { get; set; }
        public string? DesShortName { get; set; }
        public string? DesName { get; set; }
        public int DesSortIndex { get; set; }
        public PaymentDescription? ParentId { get; set; } // Đây là khóa ngoại sẽ liệu kết đến khóa chính của bảng này luôn("ParentId" này sẽ liên kết trực tiếp đến "Id" của bảng "PaymentDescription" này luôn)
        public bool? IsActive { get; set; }
        public virtual IList<Payment>? Payments { get; set; }
        public virtual ICollection<PaymentDescription>? PaymentDescriptions { get; set; }

    }
}
