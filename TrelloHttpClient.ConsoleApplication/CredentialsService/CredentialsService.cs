using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrelloHttpClientClassLib.TrelloClient;

namespace TrelloHttpClient.CredentialsService
{
    public class CredentialsService
    {
        public async Task<Credentials> GetCredentials()
        {
            return JsonConvert.DeserializeObject<Credentials>(await File.ReadAllTextAsync(@"O:\TrelloHttpClient\SimpleTrelloHttpClient\TrelloHttpClient.ConsoleApplication\credentials.json"));
        }
    }
}