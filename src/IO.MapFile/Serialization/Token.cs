// -----------------------------------------------------------------------
// <copyright file="Token.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Serialization;

/// <summary>
/// Represents a token.
/// </summary>
/// <param name="type">The type.</param>
/// <param name="lexeme">The lexeme.</param>
/// <param name="line">The line.</param>
/// <param name="column">The column.</param>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly ref struct Token(TokenType type, ReadOnlySpan<char> lexeme, int line, int column)
{
    /// <summary>
    /// Gets the token type.
    /// </summary>
    public TokenType Type { get; } = type;

    /// <summary>
    /// Gets the lexeme.
    /// </summary>
    public ReadOnlySpan<char> Lexeme { get; } = lexeme;

    /// <summary>
    /// Gets the line.
    /// </summary>
    public int Line { get; } = line;

    /// <summary>
    /// Gets the column.
    /// </summary>
    public int Column { get; } = column;

    /// <inheritdoc/>
    public override string ToString() => $"{this.Type}('{this.Lexeme}')[{this.Line}:{this.Column}]";
}