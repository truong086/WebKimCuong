using System.ComponentModel.DataAnnotations;

namespace WebKimCuong.Models
{
	public class StoneQltyMst
	{
		[Key]
		public char StoneQlty_ID { get; set; }
		public string? StoneQlty { get; set; }

	}
}
