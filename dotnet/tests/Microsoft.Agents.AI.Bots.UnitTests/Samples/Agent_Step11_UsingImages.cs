// Copyright (c) Microsoft. All rights reserved.

using System.Threading.Tasks;
using Microsoft.Extensions.AI;

// ReSharper disable All
#pragma warning disable IDE0059

namespace Microsoft.Agents.AI.Bots.UnitTests.Samples;

public class Agent_Step11_UsingImages : Bot
{
    public Agent_Step11_UsingImages(BotModel model) : base(model)
    {
        this.Name = "VisionAgent";
        this.Description = "A bot that can analyze images.";
        this.Instruction = "You are a helpful agent that can analyze images.";
    }
}

public class Agent_Step11_UsingImages_Test
{
    [Fact]
    public async Task Running_Test_Async()
    {
        var model = new OpenAiBotModel(Constants.ModelName,
            Constants.EndPoint,
            Constants.DeploymentName,
            Constants.ApiKey);

        var agent = new Agent_Step01_Running(model);

        ChatMessage message = new(ChatRole.User, [
            new TextContent("What do you see in this image?"),
            new UriContent("https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg", "image/jpeg")
        ]);

        var response = await agent.RunAsync(message);

        await foreach (var update in agent.RunStreamingAsync(message))
        {
        }
    }
}
