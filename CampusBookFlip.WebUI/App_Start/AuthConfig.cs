using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using CampusBookFlip.WebUI.Models;
using CampusBookFlip.WebUI.Concrete;

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
            //Can't test on localhost - also need https - can't use http
            //OAuthWebSecurity.RegisterClient(new AmazonClient(
            //    clientId: "amzn1.application-oa2-client.938ca32874aa4fdb94b6011b35b656ef",
            //    clientSecret: "b01af741b8958f1ae345a07ea9af64e517d342272b6608753b9a4bab9aeb9023"), "Amazon", null);
            //Not working yet
            //OAuthWebSecurity.RegisterClient(new InstagramClient(
            //    clientId: "467f349a0aa842ebbd53c348ba0115b7",
            //    clientSecret: "dc5ef91ae2ad4ce6b4a08c72dfa55ddd"), "Instagram", null);

        }
    }
}
