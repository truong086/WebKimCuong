namespace thuongmaidientus1.ViewModel
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public List<string>? image { get; set; }
        public List<string>? category { get; set; }
        public float price { get; set; }
        public int click { get; set; }
        public int account_id { get; set; }
        public string? account_name { get; set; }
        public string? imageShop {  get; set; }
        public string? nameShop {  get; set; }
    }
}
