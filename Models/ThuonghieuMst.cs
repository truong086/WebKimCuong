using System.ComponentModel.DataAnnotations;

namespace WebKimCuong.Models
{
	public class ThuonghieuMst
	{
		[Key]
		public char Id { get; set; }
		public string? Name { get; set; }

	}
}
