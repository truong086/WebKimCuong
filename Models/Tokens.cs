namespace thuongmaidientus1.Models
{
    public class Tokens : BaseEntity
    {
        public UserRegMst? account { set; get; }
        public string? token { get; set; }
        public string? geneToken { get; set; }
    }
}
