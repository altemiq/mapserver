// -----------------------------------------------------------------------
// <copyright file="SetExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Serialization;

/// <summary>
/// The <see cref="IReadOnlySet{T}"/> extensions.
/// </summary>
internal static class SetExtensions
{
    /// <summary>
    /// Get a value indicating whether the set contains the value.
    /// </summary>
    /// <param name="set">The source set.</param>
    /// <param name="value">The value.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in <paramref name="set"/>; otherwise <see langword="false"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions", Justification = "ref-like value cannot be in LINQ statement")]
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