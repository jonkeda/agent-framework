/*
// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Shared.Diagnostics;

#pragma warning disable CA2007
#pragma warning disable IL2026
#pragma warning disable IL3050
#pragma warning disable IL2075
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable RCS1124
#pragma warning disable CA1040
namespace Microsoft.Agents.AI.Bots;

public class BotAgent<T> : BotAgent
{
    public BotAgent(AIAgent agent)
        : base(agent)
    { }


}

public class BotAgent
{
    private readonly AIAgent _agent;
    private readonly AgentThread? _agentThread;

    public BotAgent(AIAgent agent, AgentThread? _agentThread)
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
*/
