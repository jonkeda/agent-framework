// Copyright (c) Microsoft. All rights reserved.

#pragma warning disable CA2007
#pragma warning disable IL2026
#pragma warning disable IL3050
#pragma warning disable IL2075
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable RCS1124
#pragma warning disable CA1040

namespace Microsoft.Agents.AI.Bots;

public interface IBotHasMemory<T>
    where T : IBotMemory
{ }
