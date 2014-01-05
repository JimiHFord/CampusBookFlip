using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusBookFlip.WebUI.Models
{
    public class ConfirmTokenEmail : Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public string FirstName { get; set; }
        public string ConfirmationToken { get; set; }
        public string Subject { get; set; }
    }

    public class NewEmailTokenEmail : ConfirmTokenEmail
    {
        public string NewEmail { get; set; }
        public string OldEmail { get; set; }
    }
}