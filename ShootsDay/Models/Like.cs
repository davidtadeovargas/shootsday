using System;
namespace ShootsDay.Models
{
	public class Like
    {
		public int id { get; set; }
        public int user_id { get; set; }
        public int photoshoot_id { get; set; }
        public DateTime created { get; set; }
    }
}
