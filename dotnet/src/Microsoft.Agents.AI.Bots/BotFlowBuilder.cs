// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Agents.AI.Workflows;

namespace Microsoft.Agents.AI.Bots;

#pragma warning disable CS1591
public static partial class BotFlowBuilder
{
    public static BotFlow BuildSequential(params IEnumerable<Bot> bots)
    {
        return new BotFlow(AgentWorkflowBuilder.BuildSequential(bots.Select(b => b.Agent)));
    }

    public static BotFlow BuildSequential(params Bot[] bots)
    {
        return new BotFlow(AgentWorkflowBuilder.BuildSequential(bots.Select(b => b.Agent)));
    }
}
