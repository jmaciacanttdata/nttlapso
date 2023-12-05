using NTTLapso.Tools;
using System.Text.Json.Serialization;

namespace NTTLapso.Models.DataDump
{
    public class SimpleResponse
    {
        [JsonPropertyOrder(-3)]
        public bool Completed { get; set; } = false;
        [JsonPropertyOrder(-2)]
        public int StatusCode { get; set; } = 200;
        [JsonPropertyOrder(-1)]
        public LogBuilder Log {  get; set; }
    }
}
