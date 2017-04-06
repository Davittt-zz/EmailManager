using EmailManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			EmailProcessor.ProcessEmail(@"C:\Users\simlog\Desktop\MailWrapper\EmailSampleFiles\cmd"
									  , @"C:\Users\simlog\Desktop\MailWrapper\EmailSampleFiles\msg"
									  ,"00D1810DA720471C9D89262EF55F23A6.MAI");
		}
	}
}
