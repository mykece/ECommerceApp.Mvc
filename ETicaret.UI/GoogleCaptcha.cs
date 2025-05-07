using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ETicaret.UI
{
    public class GoogleCaptcha
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
