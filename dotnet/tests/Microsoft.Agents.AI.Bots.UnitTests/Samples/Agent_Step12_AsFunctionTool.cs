// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel;
using System.Threading.Tasks;
#pragma warning disable IDE0040
#pragma warning disable IDE0059
// ReSharper disable All

namespace Microsoft.Agents.AI.Bots.UnitTests.Samples;

public class Agent_Step12_WeatherBot : Bot
{
    public Agent_Step12_WeatherBot(BotModel model) : base(model)
    {
        this.Name = "Weather";
        this.Description = "A bot that tells the weather.";
        this.Instruction = "You are a helpful assistant.";
    }

    [Description("Get the weather for a given location.")]
    public static string GetWeather([Description("The location to get the weather for.")] string location)
        => $"The weather in {location} is cloudy with a high of 15°C.";
}

public class Agent_Step12_FrenchWeatherBot : Bot
{
    public Agent_Step12_FrenchWeatherBot(BotModel model) : base(model)
    {
        this.Name = "WeatherInFrench";
        this.Description = "A bot that tells the weather in french.";
        this.Instruction = "You are a helpful assistant.";

        this.WeatherBot = new Agent_Step12_WeatherBot(model);
    }

    public Agent_Step12_WeatherBot WeatherBot { get; }
}

public class Agent_Step12_AsFunctionTool_Test
{
    [Fact]
    public async Task Running_Test_Async()
    {
        var model = new OpenAiBotModel(Constants.ModelName,
            Constants.EndPoint,
            Constants.DeploymentName,
            Constants.ApiKey);

        var agent = new Agent_Step12_FrenchWeatherBot(model);

        var response = await agent.RunAsync("What is the weather like in Amsterdam?");

        await foreach (var update in agent.RunStreamingAsync("What is the weather like in Amsterdam?"))
        {
        }
    }
}
