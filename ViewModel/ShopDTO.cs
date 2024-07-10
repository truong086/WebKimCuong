using thuongmaidientus1.Models;

namespace thuongmaidientus1.ViewModel
{
    public class ShopDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? diachi { get; set; }
        public string? email { get; set; }
        public string? sodienthoai { get; set; }
        public string? image { get; set; }
        public string? creator { get; set; }
        public int account_id { get; set; }
        public string? role { get; set; }
        public List<string>? vanchuyens { get; set; }
    }
}
