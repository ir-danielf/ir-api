using System.Collections.Generic;
using Newtonsoft.Json;

namespace IRCore.DataAccess.Model
{
    public class GoogleMarkup
    {
        [JsonProperty("@type")]
        public string type { get; set; }
        [JsonProperty("@context")]
        public string context { get; set; }
        public string name { get; set; }
        public Location location { get; set; }
        public List<Offer> offers { get; set; }
        public string startDate { get; set; }
        public string url { get; set; }

        public class Address
        {
            [JsonProperty("@type")]
            public string type { get; set; }
            public string streetAddress { get; set; }
            public string addressLocality { get; set; }
            public string addressRegion { get; set; }
            public string postalCode { get; set; }
            public string addressCountry { get; set; }
        }

        public class Location
        {
            [JsonProperty("@type")]
            public string type { get; set; }
            public string name { get; set; }
            public Address address { get; set; }
        }

        public class Offer
        {
            [JsonProperty("@type")]
            public string type { get; set; }
            public string name { get; set; }
            public string category { get; set; }
            public string price { get; set; }
            public string priceCurrency { get; set; }
            public string url { get; set; }
            public string availability { get; set; }
        }
    }
}
