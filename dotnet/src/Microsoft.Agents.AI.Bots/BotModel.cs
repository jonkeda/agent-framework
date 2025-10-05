// Copyright (c) Microsoft. All rights reserved.

#pragma warning disable CA2007
#pragma warning disable IL2026
#pragma warning disable IL3050
#pragma warning disable IL2075
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable RCS1124
#pragma warning disable CA1040

namespace Microsoft.Agents.AI.Bots;

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
