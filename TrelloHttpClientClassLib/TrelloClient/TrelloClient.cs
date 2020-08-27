using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrelloHttpClientClassLib.Entities;

namespace TrelloHttpClientClassLib.TrelloClient
{
    
    //get all user's boards
    //get lists from certain board
    //get cards
    //create card in the list
    //enter credentials in the constructor
    public class TrelloClient
    {
        private const string TrelloPath = "https://api.trello.com";
        private string _userName;
        private string _apiKey;
        private string _apiToken; // to get private info and get access to creation of cards

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="apiKey">User's api key</param>
        /// <param name="apiToken">Enter the apiToken to get private boards and get access to create cards</param>
        public TrelloClient(
            string userName,
            string apiKey,
            string apiToken = null)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName), "User name cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey), "Api key name cannot be null or empty!");
            }
            
            _userName = userName;
            _apiKey = apiKey;// apiKey;
            _apiToken = apiToken;// apiToken;
        }
        
        public async Task<IList<Board>> GetBoardsAsync()
        {
            using var httpClient = new HttpClient();

            var boardsResponse =
                await httpClient.GetAsync(
                    TrelloPath +
                    $"/1/members/{_userName}/boards?{GetQueryCredentialsString()}" 
                    );
            
            if (boardsResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorResponse = await boardsResponse.Content.ReadAsStringAsync();
                throw new Exception(errorResponse);
            }
            
            var boardsJson = await boardsResponse.Content.ReadAsStringAsync();

            var boards = JsonConvert.DeserializeObject<List<Board>>(boardsJson);

            return boards;
        }

        public async Task<IList<CardsList>> GetListsByBoardIdAsync(string boardId)
        {
            if (string.IsNullOrEmpty(boardId))
            {
                throw new ArgumentNullException(nameof(boardId),"Board id cannot be null or empty");
            }
            
            using var httpClient = new HttpClient();
            
            var listsResponse =
                await httpClient.GetAsync(
                    TrelloPath + 
                    $"/1/boards/{boardId}/lists?{GetQueryCredentialsString()}" 
                );
            
            if (listsResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorResponse = await listsResponse.Content.ReadAsStringAsync();
                throw new Exception(errorResponse);
            }
            
            var listsJson = await listsResponse.Content.ReadAsStringAsync();

            var lists = JsonConvert.DeserializeObject<List<CardsList>>(listsJson);

            return lists;
        }

        public async Task<IList<Card>> GetCardsByListIdAsync(string listId)
        {
            if (string.IsNullOrEmpty(listId))
            {
                throw new ArgumentNullException(nameof(listId),"List id cannot be null or empty");
            }
            
            using var httpClient = new HttpClient();
            
            var cardsResponse =
                await httpClient.GetAsync(
                    TrelloPath + 
                    $"/1/lists/{listId}/cards?{GetQueryCredentialsString()}"
                );

            if (cardsResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorResponse = await cardsResponse.Content.ReadAsStringAsync();
                throw new Exception(errorResponse);
            }
            
            var cardsJson = await cardsResponse.Content.ReadAsStringAsync();

            var cards = JsonConvert.DeserializeObject<List<Card>>(cardsJson);

            return cards;
        }

        public async Task CreateCardByListIdAsync(string listId, string name, string desc)
        {
            if (string.IsNullOrEmpty(_apiToken))
            {
                throw new Exception("Enter api token before creating!");
            }

            if (string.IsNullOrEmpty(listId))
            {
                throw new ArgumentNullException(nameof(listId),"List id cannot be null or empty");
            }
            
            using var httpClient = new HttpClient();

            var requestData = $"idList={listId}&name={name}&desc={desc}";
            
            var response = await httpClient.PostAsync(
                TrelloPath + $"/1/cards?{GetQueryCredentialsString()}&{requestData}", new StringContent(string.Empty));
            
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception(errorResponse);
            }
        }
        
        public void SetApiKey(string apiKey)
        {
            _apiKey = apiKey;
        }

        public void SetApiToken(string apiToken)
        {
            _apiToken = apiToken;
        }

        public void SetUserName(string userName)
        {
            _userName = userName;
        }

        private string GetQueryCredentialsString()
        {
            return $"key={_apiKey}{(_apiToken == null ? "" : $"&token={_apiToken}")}";
        }
    }
}