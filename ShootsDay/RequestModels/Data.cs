using ShootsDay.Models;
using System;
using System.Collections.Generic;

namespace ShootsDay.RequestModels
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

        public List<User> Users { get; set; }
        public List<Event> Events { get; set; }

        public Status status { get; set; }
    }
}
