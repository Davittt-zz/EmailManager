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
			EmailProcessor.ProcessEmail("samplefile");
		}
	}
}
