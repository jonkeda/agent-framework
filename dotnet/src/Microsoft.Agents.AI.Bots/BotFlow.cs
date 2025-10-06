// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Agents.AI.Workflows;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Microsoft.Agents.AI.Bots;

public class BotFlow(Workflow workflow)
{
    public Workflow Workflow { get; } = workflow;
}
