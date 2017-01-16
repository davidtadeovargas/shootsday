using System;
namespace ShootsDay.Models
{
	public class DataUser
	{
		public User user { get; set; }
	}
	public class RequestUser
	{
		public Status status { get; set; }
		public string code_event { get; set; }
		public DataUser data { get; set; }
	}
}
