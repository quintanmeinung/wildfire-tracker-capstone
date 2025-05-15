using Microsoft.AspNetCore.Mvc;
using project_wildfire_web.Services;

namespace project_wildfire_web.Controllers
{
    [ApiController]
    [Route("api/chatGPT")]
    public class ChatController : ControllerBase
    {
        private readonly OpenAIService _openAI;

        public ChatController(OpenAIService openAI)
        {
            _openAI = openAI;
        }

        // Expect a JSON body like { "prompt": "Hello, world!" }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt cannot be empty.");

            var completion = await _openAI.GetChatAsync(request.Prompt);
            return Ok(completion);
        }
    }

    // Lightweight DTO for incoming prompts
    public class ChatRequestDto
    {
        public string Prompt { get; set; } = string.Empty;
    }
}