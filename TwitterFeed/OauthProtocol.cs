using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TwitterFeed
{
    public class OauthProtocol
    {
        private readonly string ConsumerKey;
        private readonly string ConsumerSecret;
        private readonly string AuthToken;
        private readonly string AuthTokenSecret;

        public OauthProtocol(string consumerKey, string consumerSecret, string authToken, string authTokenSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
            this.AuthToken = authToken;
            this.AuthTokenSecret = authTokenSecret;
        }

        public string PrepareOauthHeader(string apiUrl, string apiParams, string requestMethod)
        {
            var oauthParams = CreateOauthParams();

            var requestSignature = BuildRequestSignature(apiParams, oauthParams, requestMethod, apiUrl);

            oauthParams.Add("oauth_signature", requestSignature);

            string oauthData = "";
            oauthParams.OrderBy(x => x.Key).ToList().ForEach(x =>
                {
                    if (oauthData != string.Empty) oauthData += ",";
                    oauthData += Uri.EscapeDataString(x.Key) + "=" + Uri.EscapeDataString(x.Value);
                });
            return "OAuth  " + oauthData;
        }



        private string BuildRequestSignature(string apiParams, Dictionary<string, string> oauthParams, string requestMethod, string url)
        {
            var allParams = new Dictionary<string, string>(oauthParams);
            if (!string.IsNullOrWhiteSpace(apiParams))
            {
                var individualApiParams = apiParams.Split('&');
                individualApiParams.ToList().ForEach(x =>
                    {
                        var param = x.Split('=');
                        allParams.Add(param[0], param[1]);
                    });
            }

            string paramString = "";
            allParams.OrderBy(x => x.Key).ToList().ForEach(x =>
                {
                    if (paramString != string.Empty) paramString += "&";
                    paramString += Uri.EscapeDataString(x.Key) + "=" + Uri.EscapeDataString(x.Value);
                });

            var signature = requestMethod + "&" + Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(paramString);


            var enc = Encoding.ASCII;
            var secretKey = Uri.EscapeDataString(ConsumerSecret) + "&" + Uri.EscapeDataString(AuthTokenSecret);
            var hmac = new HMACSHA1(enc.GetBytes(secretKey));
            hmac.Initialize();

            byte[] buffer = enc.GetBytes(signature);
            return Convert.ToBase64String(hmac.ComputeHash(buffer));
        }

        private  Dictionary<string, string> CreateOauthParams()
        {
            var oauthParams = new Dictionary<string, string>
            {
                {"oauth_consumer_key", ConsumerKey},
                {"oauth_nonce", Guid.NewGuid().ToString()},
                {"oauth_signature_method", "HMAC-SHA1"},
                {"oauth_timestamp", Helpers.CurrentTimestamp().ToString(CultureInfo.InvariantCulture)},
                {"oauth_token", AuthToken},
                {"oauth_version", "1.0"}
            };
            return oauthParams;
        }

       
    }
}