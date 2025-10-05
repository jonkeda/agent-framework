// Copyright (c) Microsoft. All rights reserved.

using System;
using System.ClientModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using OpenAI;

#pragma warning disable CA2007
#pragma warning disable IL2026
#pragma warning disable IL3050
#pragma warning disable IL2075
#pragma warning disable CS1591
#pragma warning disable RCS1124
#pragma warning disable CA1040

namespace Microsoft.Agents.AI.Bots;

public class OpenAiBotModel : BotModel
{
    public OpenAiBotModel(string name,
        string endPoint,
        string deploymentName,
        string apiKey)
        : base(name, endPoint, deploymentName, apiKey)
    { }

    internal override AIAgent CreateAgent(Bot bot)
    {
        ChatClientAgentOptions agentOptions = new(bot.Name, bot.Instruction);

        if (bot.TryGetStructureOutputType(out var structureType))
        {
            agentOptions.ChatOptions!.ResponseFormat = ChatResponseFormat.ForJsonSchema(structureType!);
        }

        List<AITool> tools = this.GetToolsFromBotMethods(bot);
        if (tools.Count > 0)
        {
            agentOptions.ChatOptions!.Tools = tools;
        }

        var agent = new AzureOpenAIClient(
                new Uri(this.EndPoint),
                new ApiKeyCredential(this.ApiKey))
            .GetChatClient(this.DeploymentName)
            .CreateAIAgent(agentOptions);

        return agent;
    }

    private List<AITool> GetToolsFromBotMethods(Bot bot)
    {
        var tools = new List<AITool>();

        var botType = bot.GetType();
        var methods = botType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

        foreach (var method in methods)
        {
            // Skip methods from base Object class and Bot base classes
            if (method.DeclaringType == typeof(object) ||
                method.DeclaringType == typeof(Bot) ||
                (method.DeclaringType?.IsGenericType == true && method.DeclaringType.GetGenericTypeDefinition() == typeof(Bot<>)))
            {
                continue;
            }

            // Look for methods that have a Description attribute (commonly used for AI functions)
            var descriptionAttribute = method.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                try
                {
                    // Create AITool from method
                    // For static methods, pass null as target
                    // For instance methods, pass the bot instance as target
                    var target = method.IsStatic ? null : bot;
                    var aiTool = AIFunctionFactory.Create(method, target);
                    tools.Add(aiTool);
                }
                catch (Exception)
                {
                    // Skip methods that can't be converted to AI tools
                    // This could happen if the method signature is not compatible
                    continue;
                }
            }
        }

        return tools;
    }
}
