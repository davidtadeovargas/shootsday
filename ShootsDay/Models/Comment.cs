using System;
namespace ShootsDay.Models
{
	public class Comment
	{
		public string comment { get; set; }
		public string created { get; set; }
		public User User { get; set; }
	}
}
