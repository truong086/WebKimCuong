namespace thuongmaidientus1.ViewModel
{
    public class AccountDTO
    {
        public int id { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public string? phonenumber { get; set; }
        public bool Action { get; set; }
        public string? image { get; set; }
        // Khóa ngoại
        public int? role_id { get; set; }
        public int? shop_id { get; set; }
        public string? shop_name { get; set; }
        public string? role_Name { get; set; }
        public string? token { get; set; }
    }
}
