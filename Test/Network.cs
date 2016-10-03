using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using Bridge.jQuery2;

namespace WinFormjs
{
	public class Network
	{

		public static void Post(string uri, object jsonFile, Action<object, string, jqXHR> success = null, Action<jqXHR, string, string> error = null)
		{			
			jQuery.Ajax(
				new AjaxOptions()
				{
					Url = uri,
					Cache = false,					
					Success = success,
					Error = error,					
					Data = jsonFile == null ?
						string.Empty :
						JSON.Stringify(jsonFile),
					DataType = "json",
					ContentType = "application/json",
					Type = "POST",
				}
			);
		}

		public static void Get(string uri, Action<object, string, jqXHR> success = null, Action<jqXHR, string, string> error = null)
		{			
			jQuery.Ajax(
				new AjaxOptions()
				{					
					Url = uri,
					Cache = false,					
					Type = "GET",
					Success = success,
					Error = error
				}
			);
		}				
	}
}
