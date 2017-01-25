using System;
namespace ShootsDay.Models
{
	public class RequestMyPictures
	{
		public Status status { get; set; }
		public Data data { get; set; }
		public string host_url { get; set; }
	}
}
