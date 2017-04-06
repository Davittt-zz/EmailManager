using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailManager
{
	public class FileManager
	{
		public static string FileToString(string filename) {

			//It should not be blanck spaces before the equal
			//

			return "Recipients=9SMTP:q@emailxray.com"
						+ "\tServer=MAIL2"
						+ "\tSender=[SMTP: root@s002.quarkserver.com]"
						+ "\tTimeAcquired=1491391358"
						+ "\tClientIP=10.1.5.65"
						+ "\tAuthenticationStatus=0"
						+ "\tUser="
						+ "\tAccount =quarksoft"
						+ "\tPriority="
						+ "\tCampaign="
						+ "\tStatus= Delivering"
						+ "\tCommandType="
						+ "\tSubject= < s002.quarkserver.com >  Package Update Manager notification";
		}
	}
}
