namespace thuongmaidientus1.ViewModel
{
    public class CommnetRepList
    {
        public int id { get; set; }
        public string? message { get; set; }
        public byte[]? images { get; set; }
        public List<CommnetRepList>? commentRep { get; set; } = new List<CommnetRepList>();
        public string? accountName { get; set; }
        public int? commentId { get; set; }
        public int? commentDescriptionId { get; set; }
        
    }
}
