// -----------------------------------------------------------------------
// <copyright file="SetExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Serialization;

using System;
using System.Collections.Generic;

internal static class SetExtensions
{
    public static bool Contains(this IReadOnlySet<string> set, ReadOnlySpan<char> value)
    {
        ArgumentNullException.ThrowIfNull(set);

        foreach (var item in set)
        {
            if (value.Equals(item.AsSpan(), StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}