// -----------------------------------------------------------------------
// <copyright file="Attribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// The attribute.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This _is_ an attribute")]
public readonly struct Attribute
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Attribute"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public Attribute(string value)
    {
        var startIndex = 0;
        if (value.StartsWith('['))
        {
            startIndex++;
        }

        var length = value.Length;
        if (value.EndsWith(']'))
        {
            length--;
        }

        this.Value = value.Substring(startIndex, length);
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc/>
    public override string ToString() => $"[{this.Value}]";
}