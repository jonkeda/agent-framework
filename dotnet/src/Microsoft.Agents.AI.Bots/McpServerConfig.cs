// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1819
#pragma warning disable CA1056

namespace Microsoft.Agents.AI.Bots;

public class McpDefinition;

public class McpHttpDefinition : McpDefinition
{
    public string? Endpoint { get; set; }
    public string? ApiKey { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);
    public string[]? IncludeTools { get; set; }
    public string[]? ExcludeTools { get; set; }
}

public class McpServerDefinition : McpDefinition
{
    public string Id { get; set; } = string.Empty;
    public string? CommandOrUrl { get; set; }
    public string[]? Args { get; set; }
    public Dictionary<string, string>? Env { get; set; }
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);
    public string[]? IncludeTools { get; set; }
    public string[]? ExcludeTools { get; set; }
}
