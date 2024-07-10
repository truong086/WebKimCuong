namespace thuongmaidientus1.ViewModel
{
    public class CommnentList
    {
        public int id {  get; set; }
        public string? message { get; set; }
        public byte[]? images { get; set; }
        public List<CommnetRepList>? commentRep {  get; set; } = new List<CommnetRepList>();
        public string? accountName { get; set; }
        public string? productName { get; set; }
    }
}
