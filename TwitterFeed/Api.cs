using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TweetSharp;
using System.Linq;

namespace TwitterFeed
{
    public class Api
    {
        private readonly RestApiCaller caller = new RestApiCaller();

        //public string GetFeed()
        //{
        //    //var service = new TweetSharp.TwitterService(ConsumerKey, ConsumerSecret);
        //    //service.AuthenticateWith(AuthToken, AuthTokenSecret);
        //    //var rls = service.GetRateLimitStatus(new GetRateLimitStatusOptions{Resources=new List<string>{"statuses"}});

        //    //var rateinfo = "";
        //    //foreach (var resource in rls.Resources)
        //    //{
        //    //    rateinfo += resource.Name + "\n";
        //    //}
        //    //return rateinfo;

        //    //var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions{Count=2});
        //    //string retval = "Using TweetSharp\n";
        //    //foreach (var tweet in tweets)
        //    //{
        //    //    retval += tweet.Author.ScreenName + ": " + tweet.Text + "\n";
        //    //}
        //    //retval += "\n\nUsing raw code\n";
        //    string retval = "";
        //    retval += "count:2\n" + GetHomeTimelineAsync(2);
        //    retval += "\n\ncount:2 includeEntities\n" + GetHomeTimelineAsync(2, includeEntities: false);
        //    retval += "\n\ncount:2 trimUser\n" + GetHomeTimelineAsync(2, trimUser: true);
        //    retval += "\n\ncount:2 trimUser contributerDetails\n" + GetHomeTimelineAsync(2, trimUser: true, contributerDetails: true);
        //    return retval;
        //}

        public async Task<IEnumerable<Tweet>> GetHomeTimelineAsync(int count = 20, bool trimUser = false, bool includeEntities = true, bool contributerDetails = false)
        {

            const string apiCall = "statuses/home_timeline.json";
            var apiParams = "count=" + count;
            if (trimUser) apiParams += "&trim_user=1";
            if (!includeEntities) apiParams += "&include_entities=0";
            if (contributerDetails) apiParams += "&contributer_details=1";

            var timelineJson = await caller.GetAsync(apiCall, apiParams );
            var timeline = JsonConvert.DeserializeObject<List<RawTweet>>(timelineJson).Select(t=>new Tweet(t)).ToList();
            var start = DateTime.Now;
            //await Task.WhenAll(timeline.Select(tweet => tweet.Body.InitialiseSegmentsAsync()));
            foreach (var tweet in timeline)
            {
                await tweet.Body.InitialiseSegmentsAsync();
            }
            Console.WriteLine("ending url extending after " + (DateTime.Now - start).TotalSeconds + " seconds");
            return timeline;

            //return "\n" + Helpers.Render(timeline, "\n");// +(timelineJson.Contains("retweeted_status") ? "\n JSON DUMP\n" + timelineJson : "\n meh, dumping anyways\n" + timelineJson);
            
            //return timelineJson.Replace("{", "\n{\n").Replace("}", "\n}\n");
        }

        public enum RateLimitResources
        {
            help,
            statuses,
            users,
            search,
            lists,
            application,
            friendships,
            blocks,
            geo,
            prompts,
            followers,
            friends,
            direct_messages,
            account,
            favorites,
            saved_searches,
            trends
        }

        public async Task<IEnumerable<RateLimit>> GetRateLimitsAsync(params RateLimitResources[] resources)
        {
            const string apiCall = "application/rate_limit_status.json";
            string apiParams = null;
            if (resources.Length > 0)
            {
                apiParams = "resources=";
                foreach (var rateLimitResource in resources)
                {
                    apiParams += rateLimitResource + ",";
                }
            }

            var rates = await caller.GetAsync(apiCall, apiParams);
            return ParseRateLimit(rates);
        }

        private List<RateLimit> ParseRateLimit(string rates)
        {
            var limits = new List<RateLimit>();
            var lines = rates.Replace("}", "}\n").Replace("\":{\"\\/", "\n,\"\\/").Split('\n');
            foreach (var line in lines)
            {
                if (line.StartsWith(",\"\\/"))
                {
                    var limit = new RateLimit();
                    var parser = new LineParser(line);

                    limit.Method = parser.GetDelimitedValue(",\"", "\"").Replace("\\/", "/");
                    limit.Limit = int.Parse(parser.GetDelimitedValue("\"limit\":", ","));
                    limit.Remaining = int.Parse(parser.GetDelimitedValue("\"remaining\":", ","));
                    limit.Reset = int.Parse(parser.GetDelimitedValue("\"reset\":", "}"));
                    limits.Add(limit);
                }
            }
            return limits;
        }

        public class LineParser
        {
            private readonly string line;
            private int currentIndex;

            public LineParser(string line)
            {
                this.line = line;
            }

            public bool Finished {get { return currentIndex >= line.Length; }}

            public string GetDelimitedValue(string startDelimiter, string endDelimiter)
            {
                int startIndex = line.IndexOf(startDelimiter, currentIndex) + startDelimiter.Length;
                currentIndex = line.IndexOf(endDelimiter, startIndex);
                return line.Substring(startIndex, currentIndex - startIndex);
            }

            public string GetUntil(string delimiter)
            {
                int startIndex = currentIndex;
                currentIndex = line.IndexOf(delimiter, startIndex);
                if (currentIndex < 0)
                    currentIndex = line.Length;
                return line.Substring(startIndex, currentIndex - startIndex);
            }
        }
    }
}