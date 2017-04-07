using EmailManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			string sourceCmdPath="", sourceMsgPath = "", processedCmdPath = "", processedMsgPath = "";

			try
			{
				if (args.Count() != 4)
				{
					//string filename = "00D1810DA720471C9D89262EF55F23A6.MAI";
					Console.WriteLine("Wrong number of arguments. ");
					Console.WriteLine("Please run Programm with the following parameters:  ");
					Console.WriteLine("		Programm <InCmdPath> <InMsgPath> <OutCmdPath> <OutMsgPath>");

					Console.WriteLine("Continue with default directories...");
					Console.Read();

					sourceCmdPath = @"C:\Users\simlog\Desktop\MailWrapper\EmailSampleFiles\cmd";
					sourceMsgPath = @"C:\Users\simlog\Desktop\MailWrapper\EmailSampleFiles\msg";
					processedCmdPath = @"C:\Users\simlog\Desktop\MailWrapper\EmailSampleFiles\processed\cmd";
					processedMsgPath = @"C:\Users\simlog\Desktop\MailWrapper\EmailSampleFiles\processed\msg";
				}
				else
				{
					if (Directory.Exists(args[0]) && Directory.Exists(args[1]))
					{
						if (FileManager.IsValidDirectory(args[2]) && FileManager.IsValidDirectory(args[3]))
						{
							sourceCmdPath = args[0];
							sourceMsgPath = args[1];
							processedCmdPath = args[2];
							processedMsgPath = args[3];
						} else
						{
							Console.WriteLine(string.Format("Invalid destination directories {0} {1}", args[2], args[3]));
						}
					}
					else
					{
						Console.WriteLine("Invalid source directories or not exist");
					}
				}

				string[] fileEntries = Directory.GetFiles(sourceCmdPath);
				foreach (string filepath in fileEntries)
				{
					string fileName = Path.GetFileName(filepath);
					if (EmailProcessor.ProcessEmail(sourceCmdPath, sourceMsgPath, fileName))
					{
						//If everithing is ok just move the file to the processed folder
						FileManager.MoveFile(sourceCmdPath, processedCmdPath, fileName);
						FileManager.MoveFile(sourceMsgPath, processedMsgPath, fileName);
					}
				}
				Console.WriteLine("Success!");
				Console.Read();
			}
			catch (Exception Ex)
			{
				Console.WriteLine("Something wrong " + Ex.Message);
				Console.Read();
			}
		}
	}
}
