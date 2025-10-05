// Copyright (c) Microsoft. All rights reserved.

using System.Threading.Tasks;

// ReSharper disable All
#pragma warning disable IDE0059

namespace Microsoft.Agents.AI.Bots.UnitTests.Samples;

public class Agent_Step01_Running : Bot
{
    public Agent_Step01_Running(BotModel model) : base(model)
    {
        this.Name = "Joker";
        this.Description = "A bot that tells jokes.";
        this.Instruction = "You are good at telling jokes.";
    }
}

public class Agent_Step01_Running_Test
{
    [Fact]
    public async Task Running_Test_Async()
    {
        var model = new OpenAiBotModel(Constants.ModelName,
            Constants.EndPoint,
            Constants.DeploymentName,
            Constants.ApiKey);

        var agent = new Agent_Step01_Running(model);

        var response = await agent.RunAsync("Tell me a joke about a pirate.");

        await foreach (var update in agent.RunStreamingAsync("Tell me a joke about a pirate."))
        {
        }
    }
}
