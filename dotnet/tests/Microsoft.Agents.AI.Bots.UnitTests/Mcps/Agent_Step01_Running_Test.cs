// Copyright (c) Microsoft. All rights reserved.

using System.Threading.Tasks;

// ReSharper disable All
#pragma warning disable IDE0059

namespace Microsoft.Agents.AI.Bots.UnitTests.Samples;

public class Agent_Mcp01_Running : Bot
{
    public Agent_Mcp01_Running(BotModel model) : base(model)
    {
        this.Name = "WebSearch";
        this.Description = "A bot that can search the web.";
        this.Instruction = "You are great at search the web.";

        /*WebSearch = new McpServerDefinition
        {
            Name = "WebSearch",
            Description = "A tool that can search the web for information.",
            Endpoint = "https://mcp-websearch.azurewebsites.net/api",
            ApiKey = "YOUR_API_KEY_HERE"
        };*/
    }

    // public McpServerDefinition WebSearch { get; }
}

public class Agent_Mcp01_Running_Test
{
    [Fact]
    public async Task Running_Test_Async()
    {
        var model = new OpenAiBotModel(Constants.ModelName,
            Constants.EndPoint,
            Constants.DeploymentName,
            Constants.ApiKey);

        var agent = new Agent_Step01_Running(model);

        await agent.RunAsync("Search for pirate jonkes.");

        await foreach (var update in agent.RunStreamingAsync("Search for pirate jonkes."))
        {
        }
    }
}
