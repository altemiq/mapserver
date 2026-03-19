// -----------------------------------------------------------------------
// <copyright file="MapfileSerializationOptions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Serialization;

/// <summary>
/// Options to control formatting when serializing mapfiles.
/// </summary>
public sealed class MapfileSerializationOptions
{
    /// <summary>Gets or sets indent string per nesting level (default: two spaces).</summary>
    public string Indent { get; set; } = "  ";

    /// <summary>Gets or sets a value indicating whether emit hex colors ("#RRGGBB[AA]") instead of "r g b [a]" when true.</summary>
    public bool PreferHexColors { get; set; }

    /// <summary>Gets or sets newline sequence (default: "\n").</summary>
    public string NewLine { get; set; } = "\n";

    /// <summary>Gets or sets a value indicating whether if true, omit empty blocks (e.g., empty METADATA).</summary>
    public bool OmitEmptyBlocks { get; set; } = true;

    /// <summary>Gets or sets a value indicating whether if true, write expressions in quotes to enhance round-tripping.</summary>
    public bool QuoteExpressions { get; set; } = true;
}