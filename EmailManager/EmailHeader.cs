//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmailManager
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class EmailHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmailHeader()
        {
            this.EmailDomains = new HashSet<EmailDomains>();
            this.EmailRecipients = new HashSet<EmailRecipients>();
        }
    
        public long EmailHeaderID { get; set; }
        public string Account { get; set; }
        public string Server { get; set; }
        public Nullable<int> SenderEmailID { get; set; }
        public string TimeAcquired { get; set; }
        public string ClientIP { get; set; }
        public Nullable<int> AuthenticationStatus { get; set; }
        public string User { get; set; }
        public string Priority { get; set; }
        public string Campaign { get; set; }
        public string Status { get; set; }
        public string CommandType { get; set; }
        public string Subject { get; set; }
        public string SupplementalTextFilename { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
    
        public virtual Emails Emails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailDomains> EmailDomains { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailRecipients> EmailRecipients { get; set; }

		[NotMapped]
		public string Recipients { get; set; }
		[NotMapped]
		public string Sender  { get; set; }
    }
}
