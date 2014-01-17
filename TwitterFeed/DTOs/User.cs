using Newtonsoft.Json;

namespace TwitterFeed
{
    public class User
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        public override string ToString()
        {
            return Name + " @" + ScreenName;
        }
    }
}