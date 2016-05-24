using System;
using Newtonsoft.Json;

namespace IRCore.BusinessObject.Models.AccountKitModel
{
    public class UserModel
    {
        public string id { get; set; }
        public PhoneModel phone { get; set; }
    }

    public class PhoneModel
    {
        public string number { get; set; }

        [JsonProperty(PropertyName = "country_prefix")]
        public string countryPrefix { get; set; }

        [JsonProperty(PropertyName = "national_number")]
        public string nationalNumber { get; set; }
    }

    public class AccessTokenModel
    {
        [JsonProperty(PropertyName = "access_token")]
        public string accessToken { get; set; }

        [JsonProperty(PropertyName = "expires_at")]
        public int expiresAt { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string userId { get; set; }
    }

    public class ErrorModel
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public string message { get; set; }

        public string type { get; set; }

        public int code { get; set; }

        [JsonProperty(PropertyName = "fbtrace_id")]
        public string fbTraceId { get; set; }
    }
}