using Newtonsoft.Json;

namespace TrelloHttpClientClassLib.Models
{
    public class CardsList
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        [JsonProperty("idBoard")]
        public string BoardId { get; set; }
    }
}