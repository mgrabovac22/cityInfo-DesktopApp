using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BusinessLogicLayer
{
    //Marin Grabovac
    public class ChatGPTService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public ChatGPTService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a helpful FAQ assistant." },
                        new { role = "user", content = prompt }
                    }
                };

                string jsonBody = JsonConvert.SerializeObject(requestBody);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
                {
                    Headers =
                    {
                        { "Authorization", $"Bearer {_apiKey}" }
                    },
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic responseJson = JsonConvert.DeserializeObject(responseContent);

                return responseJson.choices[0].message.content.ToString();
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }
    }
}
