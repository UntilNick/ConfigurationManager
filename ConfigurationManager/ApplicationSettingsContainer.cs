using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConfigurationManager
{
    internal class ApplicationSettingsContainer
    {
        //TODO: Do a proper version check!
        //[JsonProperty("version")]
        //public float SaverVersion { get; set; }

        [JsonProperty("settings")]
        public Dictionary<string, object> VariablesDictionary { get; set; } = new Dictionary<string, object>();
    }
}