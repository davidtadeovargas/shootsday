using System;
using System.Collections.Generic;

namespace ShootsDay.Models
{
	public class Event
	{
		public string id { get; set; }
        public string url_image { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string coordinates { get; set; }
        public string address_second { get; set; }
        public object coordinates_second { get; set; }
        public string created { get; set; }
        public string modified { get; set; }
        public int count_users { get; set; }
        public List<User> Users { get; set; }
    }
}
