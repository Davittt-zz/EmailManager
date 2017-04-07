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
			try
			{
				var headerString = FileManager.FileToString(headerPath, filename);
				var bodyString = FileManager.FileToString(bodyPath, filename);

				List<string> OverallEmailList = GetAllEmails(headerString, bodyString);

				List<string> OverallDomainList = GetAllDomains(headerString, bodyString);

				//Find EmailID or creat new elements
				List<int> EmailIDList = UpdateEmailTable(OverallEmailList);

				//Find DomainID or creat new elements
				List<int> DomainIDList = UpdateDomainTable(OverallDomainList);

				EmailHeader emailHeader = GetHeader(headerPath, filename);
				long HeaderID = SaveEmailHeader(emailHeader);

				SaveEmailDomains(HeaderID, DomainIDList);

				SaveEmailRecipients(HeaderID, EmailIDList);

				return true;
			}
			catch (Exception Ex) {
				Console.WriteLine("Something wrong with the email: " + filename);
				return false;
			}
		}

		private static void SaveEmailRecipients(long headerID, List<int> emailIDList)
		{
			using (QuarksoftDBEmailsEntities database = new QuarksoftDBEmailsEntities())
			{
				foreach (int emailID in emailIDList)
				{
					database.EmailRecipients.Add(new EmailRecipients() { EmailHeaderID = headerID, EmailID = emailID,DateAdded=DateTime.Now});
				}
				database.SaveChanges();
			}
		}

		private static void SaveEmailDomains(long headerID, List<int> domainIDList)
		{
			using (QuarksoftDBEmailsEntities database = new QuarksoftDBEmailsEntities())
			{
				foreach (int domainID in domainIDList)
				{
					database.EmailDomains.Add( new EmailDomains() { EmailHeaderID = headerID, DomainID = domainID, DateAdded = DateTime.Now });
				}
				database.SaveChanges();
			}
		}

		private static long SaveEmailHeader(EmailHeader emailHeader)
		{
			using (QuarksoftDBEmailsEntities database = new QuarksoftDBEmailsEntities())
			{
				emailHeader.SenderEmailID = GetEmailID(ExtractEmails(emailHeader.Sender).First());
				//adding the timestamp
				emailHeader.DateAdded = DateTime.Now;

				database.EmailHeader.Add(emailHeader);
				database.SaveChanges();
				return emailHeader.EmailHeaderID;
			}
		}

		private static int? GetEmailID(string emailName)
		{
			using (QuarksoftDBEmailsEntities database = new QuarksoftDBEmailsEntities())
			{
				var Email = database.Emails.FirstOrDefault(x => x.Email.Equals(emailName));
				return (Email != null) ? Email.EmailID : -1;
			}
		}

		private static List<int> UpdateEmailTable(List<string> OverallEmailList)
		{
			using (QuarksoftDBEmailsEntities database = new QuarksoftDBEmailsEntities())
			{
				List<int> emailIDList = new List<int>();

				foreach (string emailItem in OverallEmailList)
				{
					var foundEmail = database.Emails.FirstOrDefault(x => x.Email.Equals(emailItem));
					if (foundEmail != null)
					{
						emailIDList.Add(foundEmail.EmailID);
					}
					else
					{
						//Add a New email
						Emails newEmail = new Emails() { Email = emailItem, DateAdded = DateTime.Now };
						database.Emails.Add(newEmail);
						database.SaveChanges();
						//Add new EmailID
						emailIDList.Add(newEmail.EmailID);
					}
				}
				return emailIDList;
			}
		}

		private static List<int> UpdateDomainTable(List<string> OverallDomainList)
		{
			using (QuarksoftDBEmailsEntities database = new QuarksoftDBEmailsEntities())
			{
				List<int> emailIDList = new List<int>();

				foreach (string domainItem in OverallDomainList)
				{
					var foundDomain = database.Domains.FirstOrDefault(x => x.DomainName.Equals(domainItem));
					if (foundDomain != null)
					{
						emailIDList.Add(foundDomain.DomainID);
					}
					else
					{
						//Add a New Domain
						Domains newDomain = new Domains() { DomainName = domainItem, DateAdded = DateTime.Now };
						database.Domains.Add(newDomain);
						database.SaveChanges();
						//Add new DomainID
						emailIDList.Add(newDomain.DomainID);
					}
				}
				return emailIDList;
			}
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
			string[] jsonHeaderArray = header.Replace("#EQUAL#", "\":\"").Replace("\t", "\"\t\"").Split('\t');

			//JoingArray to JSON 
			string jsonEmailHeader = String.Join(",", jsonHeaderArray);

			//Conver to EmailHeaderObject
			EmailHeader emailHeader = (EmailHeader)(new JavaScriptSerializer())
													.Deserialize(jsonEmailHeader, (new EmailHeader()).GetType());
			//Adding the filename
			emailHeader.SupplementalTextFilename = filename;
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
