using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ShootsDay
{
	public static class ConnectApi
	{

		/*public async static Task<T> GetJson<T>(this string url)
		{
			try
			{
				HttpClient client = new HttpClient();
				string response = await client.GetStringAsync(url);
				//var json = await response.Content.ReadAsStringAsync();
				response = response.Insert(0, "[");
				response = response.Insert(response.Length, "]");
				var jsonSystem = JsonConvert.DeserializeObject<T>(response);
				return jsonSystem;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Excepcion: " + ex.Message);
				Debug.WriteLine("Error en: REST/GetDatos");
				return default(T);
			}
		}*/
	}
}
