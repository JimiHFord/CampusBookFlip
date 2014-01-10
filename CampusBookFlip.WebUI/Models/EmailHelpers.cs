using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusBookFlip.WebUI.Models
{
    public class CBFEmailException : Exception
    {
        public CBFEmailException(string ex) : base(ex)
        {
            
        }
    }

    public class CBFEmail : Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public string FirstName { get; set; }
        public string Subject { get; set; }
        //public override void Send()
        //{
        //    throw new CBFEmailException("Do not use the email's send method. Use the IEmailService provider");
        //}
    }

    public class ConfirmTokenEmail : CBFEmail
    {
        public string ConfirmationToken { get; set; }
        public string SharedSecret { get; set; }
    }

    public class NewEmailTokenEmail : ConfirmTokenEmail
    {
        public string NewEmail { get; set; }
        public string OldEmail { get; set; }
    }

    public class ForgotPasswordEmail : ConfirmTokenEmail
    {
        public DateTime Expiration { get; set; }
    }
}