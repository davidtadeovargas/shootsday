using System;
namespace ShootsDay.Models
{
	public class Picture
	{
		public int id { get; set; }
		public string url_image { get; set; }
		public bool status { get; set; }
		public string description { get; set; }
		public int user_id { get; set; }
		public int event_id { get; set; }
		public int likes { get; set; }
		public bool like_user { get; set; }
        public User User { get; set; }
    }
}
