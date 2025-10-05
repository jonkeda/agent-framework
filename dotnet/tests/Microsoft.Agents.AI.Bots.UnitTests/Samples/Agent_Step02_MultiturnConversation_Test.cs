// Copyright (c) Microsoft. All rights reserved.

using System.Threading.Tasks;

// ReSharper disable All
#pragma warning disable IDE0059

namespace Microsoft.Agents.AI.Bots.UnitTests.Samples;

public class Agent_Step02_MultiturnConversation : Bot
{
    public Agent_Step02_MultiturnConversation()
    {
        this.Name = "Joker";
        this.Description = "A bot that tells jokes.";
        this.Instruction = "You are good at telling jokes.";
    }
}

public class Agent_Step02_MultiturnConversation_Test
{
    [Fact]
    public async Task Running_Test_Async()
    {
        var model = new OpenAiBotModel(Constants.ModelName,
            Constants.EndPoint,
            Constants.DeploymentName,
            Constants.ApiKey);

        var agent = new Agent_Step02_MultiturnConversation()
            .CreateAgent(model);

        var response = await agent.RunAsync("Tell me a joke about a pirate.");

        await foreach (var update in agent.RunStreamingAsync("Tell me a joke about a pirate."))
        {
        }
    }
}
