using System.ComponentModel.DataAnnotations;

namespace thuongmaidientus1.Models
{
    public class Roles : BaseEntity
    {
        public virtual IList<UserRegMst>? accounts { get; set; }
        [MaxLength(100)]
        public string? name { get; set; }
    }
}