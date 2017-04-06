using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace EmailManager
{
	public class EmailProcessor
	{
		public static bool ProcessEmail(string filename) {

			//Read Header
			string header = "{\"" + FileManager.FileToString(filename) + "\"}";

			string[] jsonHaderArray = header.Replace("=", "\":\"").Replace("\t", "\"\t\"").Split('\t');



			string emailHeader = String.Join(",", jsonHaderArray);

			JavaScriptSerializer json_serializer = new JavaScriptSerializer();

			var myObject = json_serializer.Deserialize(emailHeader, (new EmailHeader()).GetType());

			return false;

		}
	}
}
