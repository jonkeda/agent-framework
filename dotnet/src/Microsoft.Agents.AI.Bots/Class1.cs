// Copyright (c) Microsoft. All rights reserved.

using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Shared.Diagnostics;
using OpenAI;
using System.Text.Json;

#pragma warning disable CA2007
#pragma warning disable IL2026
#pragma warning disable IL3050
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable RCS1124
#pragma warning disable CA1040

namespace Microsoft.Agents.AI.Bots;

public interface IBotStructureOutput { }

public interface IBotStructureOutput<T>
    where T : IBotStructureOutput
{ }

public interface IBotMemory { }

public interface IBotHasMemory<T>
    where T : IBotMemory
{ }

public abstract class Bot<T> : Bot
{
    // TODO: Make covariant in T
    public BotAgent<T> CreateOutputAgent(BotModel model)
    {
        var agent = model.CreateAgent(this);

        return new BotAgent<T>(agent);
    }
}

public abstract class Bot
{
    #region Constructors

    protected Bot()
    { }

    #endregion

    #region Properties

    public string Name { get; protected set; } = null!;

    public string Description { get; protected set; } = null!;

    public string Instruction { get; protected set; } = null!;

    #endregion

    public BotAgent CreateAgent(BotModel model)
    {
        var agent = model.CreateAgent(this);

        return new BotAgent(agent);
    }
}

public abstract class BotModel
{
    protected BotModel(string name,
        string endPoint,
        string deploymentName,
        string apiKey)
    {
        this.Name = name;
        this.EndPoint = endPoint;
        this.DeploymentName = deploymentName;
        this.ApiKey = apiKey;
    }

    public string Name { get; }
    public string EndPoint { get; }
    public string DeploymentName { get; }
    public string ApiKey { get; }

    internal abstract AIAgent CreateAgent(Bot bot);
}

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
        ChatClientAgentOptions agentOptions = new(bot.Name, bot.Instruction)
        {
            /*
            ChatOptions = new()
            {
                ResponseFormat = ChatResponseFormat.ForJsonSchema<PersonInfo>()
            }
            */
        };

        var agent = new AzureOpenAIClient(
                new Uri(this.EndPoint),
                new ApiKeyCredential(this.ApiKey))
            .GetChatClient(this.DeploymentName)
            .CreateAIAgent(agentOptions);

        return agent;
    }
}

public class BotAgent<T> : BotAgent
{
    public BotAgent(AIAgent agent)
        : base(agent)
    { }

    public async Task<T> RunOutputAsync(
        string message,
        AgentThread? thread = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _ = Throw.IfNullOrWhitespace(message);

        // Invoke the agent with some unstructured input, to extract the structured information from.
        var response = await this.RunAsync(new ChatMessage(ChatRole.User, message), thread, options, cancellationToken);

        // Deserialize the response into the PersonInfo class.
        return response.Deserialize<T>(JsonSerializerOptions.Web);
    }
}

public class BotAgent
{
    private readonly AIAgent _agent;

    public BotAgent(AIAgent agent)
    {
        this._agent = agent;
    }

    public Task<AgentRunResponse> RunAsync(
        AgentThread? thread = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default) =>
        this.RunAsync([], thread, options, cancellationToken);

    public Task<AgentRunResponse> RunAsync(
        string message,
        AgentThread? thread = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _ = Throw.IfNullOrWhitespace(message);

        return this.RunAsync(new ChatMessage(ChatRole.User, message), thread, options, cancellationToken);
    }

    public Task<AgentRunResponse> RunAsync(
        ChatMessage message,
        AgentThread? thread = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _ = Throw.IfNull(message);

        return this.RunAsync([message], thread, options, cancellationToken);
    }

    public Task<AgentRunResponse> RunAsync(
        IEnumerable<ChatMessage> messages,
        AgentThread? thread = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return this._agent.RunAsync(messages, thread, options, cancellationToken);
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> RunStreamingAsync(
        AgentThread? thread = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default) =>
        this.RunStreamingAsync([], thread, options, cancellationToken);

    public IAsyncEnumerable<AgentRunResponseUpdate> RunStreamingAsync(
        string message,
        AgentThread? thread = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _ = Throw.IfNullOrWhitespace(message);

        return this.RunStreamingAsync(new ChatMessage(ChatRole.User, message), thread, options, cancellationToken);
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> RunStreamingAsync(
        ChatMessage message,
        AgentThread? thread = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _ = Throw.IfNull(message);

        return this.RunStreamingAsync([message], thread, options, cancellationToken);
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> RunStreamingAsync(
        IEnumerable<ChatMessage> messages,
        AgentThread? thread = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return this._agent.RunStreamingAsync(messages, thread, options, cancellationToken);
    }
}
