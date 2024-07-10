using System.ComponentModel.DataAnnotations;

namespace WebKimCuong.Models
{
	public class DimInfoMst
	{
		[Key]
		public char DimID { get; set; }
		public string? DimType { get; set; }
		public string? DimSubType { get; set; }
		public string? DimCrt { get; set; }
		public string? DimPrice { get; set; }
		public string? DimImg { get; set; }

	}
}
