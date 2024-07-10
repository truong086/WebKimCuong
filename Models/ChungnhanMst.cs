using System.ComponentModel.DataAnnotations;

namespace WebKimCuong.Models
{
	public class ChungnhanMst
	{
		[Key]
		public char Id { get; set; }
		public string? Name { get; set; }
	}
}
