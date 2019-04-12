using System;
namespace ShootsDay.Models
{
	public class Photoshoot
	{
		public string id { get; set; }
		public string url_image { get; set; }
		public bool status { get; set; }
        public Like Like { get; set; }
        public string description { get; set; }
		public string user_id { get; set; }
		public string event_id { get; set; }
		public string created { get; set; }
		public string modified { get; set; }
	}
}
