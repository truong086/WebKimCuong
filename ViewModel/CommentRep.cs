namespace thuongmaidientus1.ViewModel
{
    public class CommentRep
    {
        public int id { get; set; }
        public string? message { get; set; }
        public byte[]? images { get; set; }
        public string? accountName { get; set; }
        public int? commentId { get; set; }
        public int? commentDescriptionId { get; set; }
    }
}
