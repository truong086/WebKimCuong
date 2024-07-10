using System.ComponentModel.DataAnnotations;

namespace thuongmaidientus1.Models
{
    public class JewelTypeMst : BaseEntity
    {
        public int product_id { get; set; }
        public string? product_name { get; set;}
        public float? price { get; set; }
        public int soluong { get; set; }
        public int total {  get; set; } 
        public string? trangthai { get; set; }
        public UserRegMst? Account { get; set; }
        public DimQltyMst? vanchuyen { get; set; }

    }

    

}
