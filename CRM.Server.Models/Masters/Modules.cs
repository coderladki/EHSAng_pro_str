using System.ComponentModel;
using System.Text.Json.Serialization;

namespace EHS.Server.Models.Masters
{
    public class Modules
    {
        [JsonPropertyName("id")]
        public int ModuleId { get; set; }
        [JsonPropertyName("name")]
        public string ModuleName { get; set; }
        public bool Status { get; set; }
    }
}
