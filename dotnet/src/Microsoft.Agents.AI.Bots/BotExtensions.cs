// Copyright (c) Microsoft. All rights reserved.

using System;

#pragma warning disable CA2007
#pragma warning disable IL2026
#pragma warning disable IL3050
#pragma warning disable IL2075
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable RCS1124
#pragma warning disable CA1040
namespace Microsoft.Agents.AI.Bots;

public static class BotExtensions
{
    public static bool TryGetStructureOutputType(this Bot bot, out Type? structureType)
    {
        structureType = null;

        var currentType = bot.GetType();

        while (currentType != null && currentType != typeof(Bot))
        {
            if (currentType.IsGenericType &&
                currentType.GetGenericTypeDefinition() == typeof(Bot<>))
            {
                structureType = currentType.GetGenericArguments()[0];
                return true;
            }

            currentType = currentType.BaseType;
        }

        return structureType != null;
    }

    public static Type? GetStructureOutputType(this Bot bot)
    {
        return bot.TryGetStructureOutputType(out var type) ? type : null;
    }
}
