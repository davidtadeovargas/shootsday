using System;
namespace ShootsDay.RequestModels
{
	public class RequestMyPictures
	{
		public Status status { get; set; }
		public Data data { get; set; }
		public string host_url { get; set; }
	}
}
