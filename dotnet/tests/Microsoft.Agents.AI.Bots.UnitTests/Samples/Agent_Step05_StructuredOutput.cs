// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// ReSharper disable All
#pragma warning disable IDE0059

namespace Microsoft.Agents.AI.Bots.UnitTests.Samples;

/// <summary>
/// Represents information about a person, including their name, age, and occupation, matched to the JSON schema used in the agent.
/// </summary>
[Description("Information about a person including their name, age, and occupation")]
public class PersonInfo
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("age")]
    public int? Age { get; set; }

    [JsonPropertyName("occupation")]
    public string? Occupation { get; set; }
}

public class Agent_Step05_StructuredOutput : Bot<PersonInfo>
{
    public Agent_Step05_StructuredOutput(BotModel model) : base(model)
    {
        this.Name = "HelpfulAssistant";
        this.Description = "A bot that is a helpful assistant.";
        this.Instruction = "You are a helpful assistant.";
    }
}

public class Agent_Step05_StructuredOutput_Test
{
    [Fact]
    public async Task Running_Test_Async()
    {
        var model = new OpenAiBotModel(Constants.ModelName,
            Constants.EndPoint,
            Constants.DeploymentName,
            Constants.ApiKey);

        var agent = new Agent_Step05_StructuredOutput(model);

        // Invoke the agent with some unstructured input, to extract the structured information from.
        var response = await agent.RunOutputAsync("Please provide information about John Smith, who is a 35-year-old software engineer.");
    }
}
