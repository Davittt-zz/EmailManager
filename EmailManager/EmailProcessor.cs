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
		public static bool ProcessEmail(string headerPath, string bodyPath, string filename)
		{
			EmailHeader emailHeader = GetHeader(headerPath, filename);

			var headerString = FileManager.FileToString(headerPath, filename);
			var bodyString = FileManager.FileToString(bodyPath, filename);
			
			List<string> OverallEmailList = GetAllEmails(headerString, bodyString);

			List<string> OverallDomainList = GetAllDomains(headerString, bodyString);

			//Find EmailID or creat new elements
			List<int> EmailIDList = UpdateEmailTable(OverallEmailList);

			//Find DomainID or creat new elements
			List<int> DomainIDList = UpdateDomainTable(OverallDomainList);
			return false;
		}

		private static List<int> UpdateEmailTable(List<string> overallEmailList)
		{
			throw new NotImplementedException();
		}

		private static List<int> UpdateDomainTable(List<string> OverallDomainList)
		{
			throw new NotImplementedException();
		}

		private static List<string> GetAllDomains(string headerString, string bodyString)
		{
			//Get domains from header
			List<string> headerEmailsList = ExtractEmails(headerString) ?? new List<string>();
			//Get domains from body
			List<string> bodyEmailList = ExtractEmails(bodyString);

			List<string> FullList = ExtractDomains(headerEmailsList);
			List<string> bodyDomainList = ExtractDomains(bodyEmailList);
			//http,ftp,https domains
			List<string> headerHttpDomainList = ExtractHttpDomains(headerString);
			List<string> bodyHttpDomainList = ExtractHttpDomains(bodyString);

			FullList.AddRange(bodyDomainList);
			FullList.AddRange(headerHttpDomainList);
			FullList.AddRange(bodyHttpDomainList);

			return FullList.Distinct().ToList();
		}

		private static List<string> GetAllEmails(string headerString, string bodyString)
		{
			//Get emails from header
			List<string> FullList = ExtractEmails(headerString);
			//Get emails from body
			List<string> bodyEmailList = ExtractEmails(bodyString);
			FullList.AddRange(bodyEmailList);
			return FullList.Distinct().ToList();
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

		//private static Emails GetEmail(string bodyPath, string filename)
		//{
		//	//Read Body
		//	string body = FileManager.FileToString(bodyPath, filename);
		//	//Format string to JSON
		//	string[] jsonHeaderArray = body.Replace("=", "\":\"").Replace("\t", "\"\t\"").Split('\t');
		//	//JoingArray to JSON 
		//	string jsonEmailHeader = String.Join(",", jsonHeaderArray);
		//	//Conver to EmailHeaderObject
		//	Emails emailHeader = (Emails)(new JavaScriptSerializer())
		//								.Deserialize(jsonEmailHeader, (new EmailHeader()).GetType());
		//	//adding the timestamp
		//	emailHeader.DateAdded = DateTime.Now;
		//	return emailHeader;
		//}

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

		public static List<string> ExtractHttpDomains(string textToScrape)
		{
			Regex reg = new Regex(@"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?", RegexOptions.IgnoreCase);
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
	}
}
