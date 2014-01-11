/* -------------------------------------------------
 * Instagram Login Client for DotNetOpenAuth 
 * Maarten Sikkema, Macaw
 * MIT Licence (http://opensource.org/licenses/MIT)
 *
 * ------------------------------------------------- */

namespace CampusBookFlip.WebUI.Concrete
{
    using DotNetOpenAuth.AspNet.Clients;
    using DotNetOpenAuth.Messaging;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    //using Validation;


    /// <summary>
    /// The Instagram client.
    /// </summary>
    public sealed class InstagramClient : OAuth2Client
    {
        #region Constants and Fields

        /// <summary>
        /// The authorization endpoint.
        /// </summary>
        private const string AuthorizationEndpoint = "https://api.instagram.com/oauth/authorize/";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string TokenEndpoint = "https://api.instagram.com/auth/o2/token";

        /// <summary>
        /// The _app id.
        /// </summary>
        private readonly string clientId;

        /// <summary>
        /// The _app secret.
        /// </summary>
        private readonly string clientSecret;

        /// <summary>
        /// The scope.
        /// </summary>
        private readonly string[] scope;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class
        /// with "email" as the scope.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        public InstagramClient(string clientId, string clientSecret)
            : this(clientId, clientSecret, "basic")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        /// <param name="clientId">
        /// The app id.
        /// </param>
        /// <param name="clientSecret">
        /// The app secret.
        /// </param>
        /// <param name="scope">
        /// The scope of authorization to request when authenticating with Facebook. The default is "profile".
        /// </param>
        public InstagramClient(string clientId, string clientSecret, params string[] scope)
            : base("instagram")
        {
            //Requires.NotNullOrEmpty(clientId, "clientId");
            //Requires.NotNullOrEmpty(clientSecret, "clientSecret");
            //Requires.NotNullOrEmpty(scope, "scope");

            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.scope = scope;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get service login url.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>An absolute URI.</returns>
        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            var builder = new UriBuilder(AuthorizationEndpoint);
            builder.AppendQueryArgument("client_id", this.clientId);
            // We must circumvert DotNetOpenAuth adding its stuff to the returnUrl to the Redirect Uri... The OAuth spec does not allow it and Instagram will reject it
            builder.AppendQueryArgument("redirect_uri", returnUrl.GetLeftPart(UriPartial.Path));
            // we must send the extra stuff in the state parameter. On return we'll add it back and redirect to make DotNetOpenAuth happy
            builder.AppendQueryArgument("state", returnUrl.Query);
            builder.AppendQueryArgument("response_type", "code");
            builder.AppendQueryArgument("scope", string.Join(" ", this.scope));

            return builder.Uri;
        }

        /// <summary>
        /// The get user data.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        /// <returns>A dictionary of profile data.</returns>
        protected override IDictionary<string, string> GetUserData(string accessToken)
        // protected override NameValueCollection GetUserData(string accessToken)
        {
            InstagramProfileData userProfile;
            var request = WebRequest.Create("https://api.Instagram.com/oauth/authorize/" + HttpUtility.UrlEncode(accessToken));
            request.Headers.Add(HttpRequestHeader.Authorization, string.Format(CultureInfo.InvariantCulture, "Bearer {0}", accessToken));
            request.PreAuthenticate = true;

            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    userProfile = JsonHelper.Deserialize<InstagramProfileData>(responseStream);
                }
            }

            // this dictionary must contains 
            var userData = new Dictionary<string, string>();
            // var userData = new NameValueCollection();
            if (!string.IsNullOrEmpty(userProfile.CustomerId)) userData.Add("id", userProfile.CustomerId);
            if (!string.IsNullOrEmpty(userProfile.PrimaryEmail)) userData.Add("email", userProfile.PrimaryEmail);
            if (!string.IsNullOrEmpty(userProfile.Name)) userData.Add("name", userProfile.Name);
            if (!string.IsNullOrEmpty(userProfile.PostalCode)) userData.Add("postal_code", userProfile.PostalCode);
            return userData;
        }

        /// <summary>
        /// Obtains an access token given an authorization code and callback URL.
        /// </summary>
        /// <remarks>
        /// Instagram uses the state parameter and does not allow adding data to the reply address, as per the OAuth 2.0 spec
        /// Add the following code in the top to ExternalLoginCallback to translate the state parameters back where FotNetOpenAuth expects them
        /// 
        ///     if (!String.IsNullOrEmpty(Request.QueryString["state"]))
        ///     {
        ///         var routeValues = this.RouteData.Values;
        ///         foreach (string key in Request.QueryString.AllKeys)
        ///         if (key.ToLower() == "state")
        ///         {
        ///             NameValueCollection stateCollection = HttpUtility.ParseQueryString(Request.QueryString["state"]);
        ///             foreach (string stateKey in stateCollection.AllKeys)
        ///             routeValues.Add(stateKey, stateCollection[stateKey]);
        ///         }
        ///         else
        ///         {
        ///             routeValues.Add(key, Request.QueryString[key]);
        ///         }
        ///         return RedirectToRoute(routeValues);
        ///     }
        ///     
        /// </remarks>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <param name="authorizationCode">
        /// The authorization code.
        /// </param>
        /// <returns>
        /// The access token.
        /// </returns>
        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            var sb = new StringBuilder();
            sb.Append("grant_type=authorization_code");
            sb.Append("&code=").Append(HttpUtility.UrlEncode(authorizationCode));
            // we need to circumvert DotNetOpenAuth adding its stuff to the returnUrl to the Redirect Uri... Instagram will reject it
            sb.Append("&redirect_uri=").Append(HttpUtility.UrlEncode(returnUrl.GetLeftPart(UriPartial.Path)));
            sb.Append("&client_id=").Append(HttpUtility.UrlEncode(this.clientId));
            sb.Append("&client_secret=").Append(HttpUtility.UrlEncode(this.clientSecret));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TokenEndpoint);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            request.ContentLength = bytes.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        InstagramTokenResponse result = JsonHelper.Deserialize<InstagramTokenResponse>(stream);
                        return result.AccessToken;
                    }
                }

            }
        }


        #endregion

        /// <summary>
        /// The json helper.
        /// </summary>
        internal static class JsonHelper
        {

            #region Public Methods and Operators

            /// <summary>
            /// The deserialize.
            /// </summary>
            /// <param name="stream">
            /// The stream.
            /// </param>
            /// <typeparam name="T">The type of the value to deserialize.</typeparam>
            /// <returns>
            /// The deserialized value.
            /// </returns>
            public static T Deserialize<T>(Stream stream) where T : class
            {
                //Requires.NotNull(stream, "stream");

                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }

            #endregion
        }
    }


    /// <summary>
    /// Contains data of a Instagram user.
    /// </summary>
    /// <remarks>
    /// Technically, this class doesn't need to be public, but because we want to make it serializable in medium trust, it has to be public.
    /// </remarks>
    [DataContract]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Instagram", Justification = "Brand name")]
    public class InstagramProfileData
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value> The email. </value>
        [DataMember(Name = "email")]
        public string PrimaryEmail { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value> The id. </value>
        [DataMember(Name = "user_id")]
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value> The name. </value>
        [DataMember(Name = "name")]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value> The name. </value>
        [DataMember(Name = "postal_code")]
        public string PostalCode { get; set; }

        #endregion
    }

    [DataContract]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Instagram", Justification = "Brand name")]
    public class InstagramTokenResponse
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "expires_in")]
        public long ExpiresIn { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "scope")]
        public string Scope { get; set; }

        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "error_description")]
        public string ErrorDescription { get; set; }

        [DataMember(Name = "error_uri")]
        public string ErrorUri { get; set; }
    }
}