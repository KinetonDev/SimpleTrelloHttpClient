using Newtonsoft.Json;

namespace TrelloHttpClientClassLib.Models
{
    public class Card
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Desc { get; set; }
        
        [JsonProperty("idList")]
        public string ListId { get; set; }
    }
}