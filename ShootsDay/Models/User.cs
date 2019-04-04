using System;
namespace ShootsDay.Models
{
	public class User
	{
		public string name { get; set; }
        public string fullname { get; set; }
        public string lastname { get; set; }
		public string username { get; set; }
		public string email { get; set; }
		public string url_image { get; set; }
		public int id { get; set; }
        public int role_id { get; set; }
        public bool super { get; set; }
    }
}
