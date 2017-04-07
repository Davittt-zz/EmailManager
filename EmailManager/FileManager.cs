using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmailManager
{
	public class FileManager
	{
		public static string FileToString(string path, string filename)
		{
			using (StreamReader sr = new StreamReader(Path.GetFullPath(path + @"\" + filename)))
			{
				string outputFile = null;

				while (sr.Peek() >= 0)
				{
					string line = sr.ReadLine();
					if (!line.Equals("") && !line.Equals(" ") && line != " " && line != "")
					{
						var regex = new Regex(Regex.Escape("="));
						var newText = regex.Replace(line, "#EQUAL#", 1);
						outputFile += newText + "\t";
					}
				}
				return outputFile.Remove(outputFile.Count() - 1, 1);
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

		public static bool MoveFile(string fromPath, string toPath, string filename)
		{
			string path = Path.GetFullPath(fromPath + @"\" + filename);
			string path2 = Path.GetFullPath(toPath + @"\" + filename);
			try
			{
				{
					if (!File.Exists(path))
						// This statement ensures that the file is created,
						// but the handle is not kept.
						using (FileStream fs = File.Create(path)) { }
				}

				// Ensure that the directory exists.
				if (!Directory.Exists(Path.GetFullPath(toPath)))
				{
					Directory.CreateDirectory(Path.GetFullPath(toPath));
				}

				// Ensure that the target does not exist.
				if (File.Exists(path2))
					File.Delete(path2);

				// Move the file.
				File.Move(path, path2);

				Console.WriteLine("{0} was moved to {1}.", path, path2);

				// See if the original exists now.
				if (File.Exists(path))
				{
					Console.WriteLine("The original file still exists, which is unexpected.");
					return false;
				}
				else
				{
					//Console.WriteLine("The original file no longer exists, which is expected.");
					return true;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("The process failed: {0}", e.ToString());
				return false;
			}
		}

		public static bool IsValidDirectory(string path)
		{
			try
			{
				// Ensure that the directory exists.
				if (!Directory.Exists(Path.GetFullPath(path)))
				{
					//Okay is valid
					return true;
				}
				else
				{
					Directory.CreateDirectory(Path.GetFullPath(path));
					return true;
				}
			}
			catch (Exception Ex)
			{
				return false;
			}
		}
	}
}
