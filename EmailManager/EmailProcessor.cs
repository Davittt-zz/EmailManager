using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace EmailManager
{
	public class EmailProcessor
	{
		public static bool ProcessEmail(string headerPath, string bodyPath, string filename) {

			EmailHeader emailHeader = GetHeader(headerPath, filename);
			
			//Get emails from header
			List<string> headerEmailsList = ExtractEmails(FileManager.FileToString(headerPath, filename));
			
			//Get emails from body
			List<string> bodyEmailList = ExtractEmails(FileManager.FileToString(bodyPath, filename));

			List<string> headerDomainList = ExtractDomains(headerEmailsList);

			List<string> bodyDomainList = ExtractDomains(bodyEmailList);

			return false;
		}
		
		private static EmailHeader GetHeader(string headerPath, string filename)
		{
			//Read Header
			string header = "{\"" + FileManager.FileToString(headerPath, filename) + "\"}";

			//Format string to JSON
			string[] jsonHeaderArray = header.Replace("=", "\":\"").Replace("\t", "\"\t\"").Split('\t');

			//JoingArray to JSON 
			string jsonEmailHeader = String.Join(",", jsonHeaderArray);

			//Conver to EmailHeaderObject
			EmailHeader emailHeader = (EmailHeader)(new JavaScriptSerializer())
													.Deserialize(jsonEmailHeader, (new EmailHeader()).GetType());

			//Adding the filename
			emailHeader.SupplementalTextFilename = filename;

			//adding the timestamp
			emailHeader.DateAdded = DateTime.Now;

			return emailHeader;
		}

		private static Emails GetEmail(string bodyPath, string filename)
		{
			//Read Body
			string body  =  FileManager.FileToString(bodyPath, filename);

			//Format string to JSON
			string[] jsonHeaderArray = body.Replace("=", "\":\"").Replace("\t", "\"\t\"").Split('\t');

			//JoingArray to JSON 
			string jsonEmailHeader = String.Join(",", jsonHeaderArray);

			//Conver to EmailHeaderObject
			Emails emailHeader = (Emails)(new JavaScriptSerializer())
										.Deserialize(jsonEmailHeader, (new EmailHeader()).GetType());


			//adding the timestamp
			emailHeader.DateAdded = DateTime.Now;

			return emailHeader;
		}

		public static List<string> ExtractEmails(string textToScrape)
		{
			Regex reg = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);
			Match match;

			List<string> results = new List<string>();
			for (match = reg.Match(textToScrape); match.Success; match = match.NextMatch())
			{
				if (!(results.Contains(match.Value)))
					results.Add(match.Value);

				Console.WriteLine(match.Value.ToString());
			}
			return results;
		}

		private static List<string> ExtractDomains(List<string> bodyEmailList)
		{
			List<string> domainList = new List<string>();
			//MailAddress address = null;
			foreach (string email in bodyEmailList)
			{
				domainList.Add((new MailAddress(email)).Host);
			}
			return domainList;
		}

		//private static List<string> emas(string text)
		//{
		//	const string MatchEmailPattern =
		//   @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
		//   + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
		//	 + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
		//   + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
		//	Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
		//	// Find matches.
		//	MatchCollection matches = rx.Matches(text);
		//	// Report the number of matches found.
		//	int noOfMatches = matches.Count;
		//	// Report on each match.
		//	List<string> results = new List<string>();

		//	foreach (Match match in matches)
		//	{
		//		results.Add(match.Value);
		//		Console.WriteLine(match.Value.ToString());
		//	}
		//	return results;
		//}
	}
}
