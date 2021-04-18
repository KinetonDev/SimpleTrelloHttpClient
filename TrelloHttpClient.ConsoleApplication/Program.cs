using System;
using System.Threading.Tasks;
using TrelloHttpClientClassLib.TrelloClient;

namespace TrelloHttpClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var credentialsService = new CredentialsService.CredentialsService();
            var credentials = await credentialsService.GetCredentials();
            
            var client = new TrelloClient(
                credentials.UserName, 
                credentials.ApiKey, 
                credentials.ApiToken);

            var boards = await client.GetBoardsAsync();

            foreach (var board in boards)
            {
                var lists = await client.GetListsByBoardIdAsync(board.Id);

                Console.WriteLine(board.Id);
                Console.WriteLine(board.Name);
                Console.WriteLine(board.Desc);
                Console.WriteLine(board.Url);

                foreach (var list in lists)
                {
                    Console.WriteLine("\t" + list.Name);
                    Console.WriteLine("\t" + list.BoardId);

                    var cards = await client.GetCardsByListIdAsync(list.Id);
        
                    foreach (var card in cards)
                    {
                        Console.WriteLine("\t\t" + card.Name);
                        if (!string.IsNullOrEmpty(card.Desc)) Console.WriteLine("\t\tDesc: " + card.Desc);
                        Console.WriteLine("\t\t"+card.ListId);
                    }
                }
            }
        }
    }
}