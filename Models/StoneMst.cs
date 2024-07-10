namespace thuongmaidientus1.Models
{
    public class StoneMst : BaseEntity
    {
        public Inquiry? shop { get; set; }  
        public DimQltyMst? Vanchuyen { get; set; }
    }
}
