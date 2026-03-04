// -----------------------------------------------------------------------
// <copyright file="TokenType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Serialization;

public enum TokenType
{
    // High level types
    Keyword,
    Identifier,
    Number,
    String,
    Boolean,
    Attribute, // e.g., [colour_rgb]

    // Punctuation/Operators (only if they appear outside strings)
    LParen,    // (
    RParen,    // )
    Comma,     // ,
    Equals,    // =

    // Bookkeeping
    EOF,
}