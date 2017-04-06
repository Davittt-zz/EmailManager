using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailManager
{
	public class FileManager
	{
		public static string FileToString(string path,string filename)
		{
			using (StreamReader sr = new StreamReader(Path.GetFullPath(path+ @"\" + filename)))
			{
				string outputFile = null;

				while (sr.Peek() >= 0) 
				{
					string line = sr.ReadLine();
					if (!line.Equals("") && !line.Equals(" ") && line!=" " && line != "")
					{
						outputFile += line + "\t";
					}
				}
				return outputFile.Remove(outputFile.Count()-1,1);
			}
			//It should not be blanck spaces before the equal
			//
			//return "Recipients=9SMTP:q@emailxray.com"
			//			+ "\tServer=MAIL2"
			//			+ "\tSender=[SMTP: root@s002.quarkserver.com]"
			//			+ "\tTimeAcquired=1491391358"
			//			+ "\tClientIP=10.1.5.65"
			//			+ "\tAuthenticationStatus=0"
			//			+ "\tUser="
			//			+ "\tAccount =quarksoft"
			//			+ "\tPriority="
			//			+ "\tCampaign="
			//			+ "\tStatus= Delivering"
			//			+ "\tCommandType="
			//			+ "\tSubject= < s002.quarkserver.com >  Package Update Manager notification";

		}
	}
}
