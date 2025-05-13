/* using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Infrastructure;
using project_wildfire_web.Services;

namespace project_wildfire_tests.UnitTests;

[TestFixture]
public class OpenAIServiceTests
{
    [Test]
    public async Task GetChatAsync_WithMockTransport_ReturnsExpectedResponse()
    {
        // Arrange: fake completions response
        var fakeCompletions = new ChatCompletions(
            choices: new[] {
                new ChatChoice(new ChatMessage(ChatRole.Assistant, "Mock response"))
            },
            usage: null
        );
                // Mock transport to intercept the HTTP call
        var mockTransport = new MockTransport()
            .AddMethod(
                HttpMethod.Post,
                "/v1/chat/completions",
                new MockResponse
                {
                    Status = 200,
                    Response = fakeCompletions
                }
            );

        // Create OpenAIClient with the mock transport
        var clientOptions = new OpenAIClientOptions
        {
            ApiKey = "test-key",
            Transport = mockTransport
        };
        var mockClient = new OpenAIClient(clientOptions);

        // Inject into service using the test-model
        var service = new OpenAIService(mockClient, "test-model");

        // Act
        var result = await service.GetChatAsync("test prompt");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Choices.Count(), Is.EqualTo(1));
        Assert.That(result.Choices.First().Message.Content, Is.EqualTo("Mock response"));
    }
}
 */