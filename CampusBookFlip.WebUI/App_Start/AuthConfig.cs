using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using CampusBookFlip.WebUI.Models;

namespace CampusBookFlip.WebUI
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "zGFA6wb8YBqLIWUzKMBw",
                consumerSecret: "coyktYUsiGHxQ8m4gdpIOxpie81smnzWJEAdNgS3Zsk");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "116447055231811",
                appSecret: "a83986f6208df7cce098fa50ff83d231");

            OAuthWebSecurity.RegisterGoogleClient();

            OAuthWebSecurity.RegisterLinkedInClient(
                consumerKey: "756lkuaxqsy6zm",
                consumerSecret: "wek4Mt0qVBOQNklS");
        }
    }
}
