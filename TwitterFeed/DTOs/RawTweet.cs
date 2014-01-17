using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TwitterFeed
{
    public class RawTweet
    {
        [JsonProperty("created_at")]
        public string Date { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("retweeted_status")]
        public RawTweet RetweetedStatus { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("entities")]
        public Entities Entities { get; set; }

        public List<Url> Urls { get { return Entities.Urls; } } 
    }


    public class Entities
    {
        //public List<object> hashtags { get; set; }
        //public List<object> symbols { get; set; }
        [JsonProperty("urls")]
        public List<Url> Urls { get; set; }
        //public List<object> user_mentions { get; set; }
    }
    public class Url
    {
        //public string url { get; set; }
        [JsonProperty("expanded_url")]
        public string Link { get; set; }
        //public string display_url { get; set; }
        [JsonProperty("indices")]
        public List<int> Indices { get; set; }

        public int StartIndex { get { return Indices.First(); } }
        public int EndIndex { get { return Indices.Last(); } }
    }

}
