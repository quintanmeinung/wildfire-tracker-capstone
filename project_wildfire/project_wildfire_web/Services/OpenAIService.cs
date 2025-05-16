using System.Net.Http;
using System.Text;
using System.Text.Json;
using OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.Configuration;



namespace project_wildfire_web.Services;
public class OpenAIService
{
   // private readonly HttpClient _httpClient;
    private IConfiguration _config;
    private ChatClient _chatClient;

    public OpenAIService(IConfiguration config)
    {
       // _httpClient = httpClient;
        //_config = config;
        var apiKey = config["OpenAI:ApiKey"];
        //_api = new OpenAIAPI(apiKey);
        
        _chatClient = new ChatClient(model: "gpt-4.1-nano", apiKey: apiKey);

    }

    public async Task<ChatCompletion> GetChatAsync(string prompt)
    {
        return await _chatClient.CompleteChatAsync(prompt);
    } 

    
}