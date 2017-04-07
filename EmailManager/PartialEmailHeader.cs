using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailManager
{
	public partial class EmailHeader
	{
		[NotMapped]
		public string Recipients { get; set; }
		[NotMapped]
		public string Sender { get; set; }
	}
}
