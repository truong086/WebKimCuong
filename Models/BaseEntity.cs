using System.ComponentModel.DataAnnotations;

namespace thuongmaidientus1.Models
{
    public class BaseEntity
    {
        protected BaseEntity() { }
        [Key]
        public int id { get; set; }
        public bool Deleted { get; set; }
        public string? CretorEdit { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
