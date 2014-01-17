using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace TwitterFeed
{
    public class RestApiCaller
    {
        private const string ConsumerKey = "4rSifLsf7P8MkTtyctX9eg";
        private const string ConsumerSecret = "iXFck1VOdXbqnfuCVjT3lSh6rkQ09xAbR9Y2va0XTw";
        private const string AuthToken = "112838812-Wgcf2Y0dEcNB7zeptYwI7U5yut539Tq1dHg46VQ5";
        private const string AuthTokenSecret = "fzlMtnnQ6BOyLLItYiuHQqAerDLBbKNx4YbMRGsyhMezT";
        private const string BaseUrl = "https://api.twitter.com/1.1/";
        private readonly OauthProtocol oauth = new OauthProtocol(ConsumerKey, ConsumerSecret, AuthToken, AuthTokenSecret);

        public async Task<string> GetAsync(string apiMethod, string apiParams)
        {
            return await ExecuteAsync(apiMethod, apiParams, WebRequestMethods.Http.Get);
        }

        private async Task<string> ExecuteAsync(string apiMethod, string apiParams, string requestMethod)
        {
            var fullUrl = BaseUrl + apiMethod;
            var oauthData = oauth.PrepareOauthHeader(fullUrl, apiParams, requestMethod);

            var request = WebRequest.Create(fullUrl + "?" + apiParams);
            request.Headers.Add("Authorization", oauthData);
            request.Method = requestMethod;

            return await ReadResponseAsync(request);
        }
        private async Task<string> ReadResponseAsync(WebRequest webrequest)
        {
            using (var webResponse = await webrequest.GetResponseAsync())
            {
                using (var responseStream = webResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var responseReader = new StreamReader(responseStream))
                        {
                            return responseReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }
    }
}