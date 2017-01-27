using System;
using System.Collections.Generic;

namespace ShootsDay.Models
{
	public class Host
	{ 
		public string url { get; set; }
	}

	public class Data
	{
		public List<Picture> pictures { get; set; }

		public User User { get; set; }
		public Event Event { get; set; }
		public Host Host { get; set; }
		public List<Comment> Comments { get; set; }

		public List<Photoshoot> Photoshoots { get; set; }

        public Status status { get; set; }
    }
}
