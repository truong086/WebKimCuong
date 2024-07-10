using System.ComponentModel.DataAnnotations;

namespace WebKimCuong.Models
{
	public class DimQltySubMst
	{
		[Key]
		public char DimSubType_ID { get; set; }
		public string? DimQlty { get; set; }
	}
}
