// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Shared.Diagnostics;
using System.Text.Json;

namespace Microsoft.Agents.AI.Bots;

#pragma warning disable CA2007
#pragma warning disable IL2026
#pragma warning disable IL3050
#pragma warning disable IL2075
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable RCS1124
#pragma warning disable CA1040
public abstract class Bot<T> : Bot
{
    // TODO: Make covariant in T
    protected Bot(BotModel model) : base(model)
    {
    }

    public async Task<T> RunOutputAsync(
        string message,
        AgentThread? thread = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _ = Throw.IfNullOrWhitespace(message);

        // Invoke the agent with some unstructured input, to extract the structured information from.
        var response = await this.RunAsync(new ChatMessage(ChatRole.User, message), options, cancellationToken);

        // Deserialize the response into the PersonInfo class.
        return response.Deserialize<T>(JsonSerializerOptions.Web);
    }
}

public enum BotRunType
{
    MultiTurn,
    SingleTurn
}

public abstract class Bot
{
    private readonly BotModel _model;

    #region Constructors

    protected Bot(BotModel model)
    {
        this._model = model;
    }

    #endregion

    #region Properties

    public string Name { get; protected set; } = null!;

    public string Description { get; protected set; } = null!;

    public string Instruction { get; protected set; } = null!;

    public BotRunType RunType { get; protected set; } = BotRunType.MultiTurn;

    public AIAgent Agent
    {
        get
        {
            this.InitializeAgent();
            return this._agent!;
        }
    }

    #endregion

    #region Methods

    private AIAgent? _agent;
    private AgentThread? _agentThread;

    protected void InitializeAgent()
    {
        if (this._agent == null)
        {
            this._agent = this._model.CreateAgent(this);
        }
    }

    protected void InitializeThread()
    {
        this.InitializeAgent();
        if (this._agentThread == null)
        {
            if (this.RunType == BotRunType.MultiTurn)
            {
                this._agentThread = this._agent!.GetNewThread();
            }
            else
            {
                this._agentThread = null;
            }
        }
    }

    public Task<AgentRunResponse> RunAsync(
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default) =>
        this.RunAsync([], options, cancellationToken);

    public Task<AgentRunResponse> RunAsync(
        string message,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _ = Throw.IfNullOrWhitespace(message);

        return this.RunAsync(new ChatMessage(ChatRole.User, message), options, cancellationToken);
    }

    public Task<AgentRunResponse> RunAsync(
        ChatMessage message,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _ = Throw.IfNull(message);

        return this.RunAsync([message], options, cancellationToken);
    }

    public Task<AgentRunResponse> RunAsync(
        IEnumerable<ChatMessage> messages,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        this.InitializeThread();
        return this._agent!.RunAsync(messages, this._agentThread, options, cancellationToken);
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> RunStreamingAsync(
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default) =>
        this.RunStreamingAsync([], options, cancellationToken);

    public IAsyncEnumerable<AgentRunResponseUpdate> RunStreamingAsync(
        string message,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _ = Throw.IfNullOrWhitespace(message);

        return this.RunStreamingAsync(new ChatMessage(ChatRole.User, message), options, cancellationToken);
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> RunStreamingAsync(
        ChatMessage message,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _ = Throw.IfNull(message);

        return this.RunStreamingAsync([message], options, cancellationToken);
    }

    public IAsyncEnumerable<AgentRunResponseUpdate> RunStreamingAsync(
        IEnumerable<ChatMessage> messages,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        this.InitializeThread();
        return this._agent!.RunStreamingAsync(messages, this._agentThread, options, cancellationToken);
    }

    public JsonElement Serialize(JsonSerializerOptions? jsonSerializerOptions = null)
    {
        if (this.RunType == BotRunType.SingleTurn)
        {
            throw new System.InvalidOperationException("Cannot serialize a single-turn bot.");
        }
        this.InitializeThread();
        return this._agentThread!.Serialize(jsonSerializerOptions);
    }

    public void SerializeToFile(string fileName, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var jsonElement = this.Serialize(jsonSerializerOptions);
        File.WriteAllText(fileName, JsonSerializer.Serialize(jsonElement));
    }

    public void Deserialize(JsonElement serializedThread)
    {
        this.InitializeThread();
        this._agentThread = this._agent!.DeserializeThread(serializedThread);
    }

    public void Deserialize(string fileName)
    {
        this.InitializeThread();
        var serializedThread = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(fileName));
        this._agentThread = this._agent!.DeserializeThread(serializedThread);
    }

    #endregion

    public AITool AsAIFunction()
    {
        this.InitializeThread();
        return this._agent!.AsAIFunction(null, this._agentThread);
    }
}
