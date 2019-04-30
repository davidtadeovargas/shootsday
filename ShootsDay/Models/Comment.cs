using System;
namespace ShootsDay.Models
{
	public class Comment
	{
        public int id { get; set; }
        public string comment { get; set; }
		public DateTime created { get; set; }
        public string createdString { get; set; }
        public User User { get; set; }
	}
}
