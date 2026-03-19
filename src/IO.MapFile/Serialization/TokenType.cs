// -----------------------------------------------------------------------
// <copyright file="TokenType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Serialization;

/// <summary>
/// The token type.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is by design.")]
public enum TokenType
{
    // High level types

    /// <summary>
    /// Keyword.
    /// </summary>
    Keyword,

    /// <summary>
    /// Identifier.
    /// </summary>
    Identifier,

    /// <summary>
    /// Number.
    /// </summary>
    Number,

    /// <summary>
    /// String.
    /// </summary>
    String,

    /// <summary>
    /// Boolean.
    /// </summary>
    Boolean,

    /// <summary>
    /// Attribute.
    /// </summary>
    /// <example><c>[colour_rgb]</c>.</example>
    Attribute,

    // Punctuation/Operators (only if they appear outside strings)

    /// <summary>
    /// Left paranthesis.
    /// </summary>
    LParen,

    /// <summary>
    /// Right paranthesis.
    /// </summary>
    RParen,

    /// <summary>
    /// Comma.
    /// </summary>
    Comma,

    /// <summary>
    /// Equals.
    /// </summary>
    Equals,

    // Bookkeeping

    /// <summary>
    /// End of file.
    /// </summary>
    EOF,
}