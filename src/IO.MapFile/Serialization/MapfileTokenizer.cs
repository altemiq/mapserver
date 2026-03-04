// -----------------------------------------------------------------------
// <copyright file="MapfileTokenizer.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Serialization;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <summary>
/// Span-based Mapfile tokenizer. Works over ReadOnlySpan{char} and avoids indexing strings directly.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public ref struct MapfileTokenizer
{
    // For classifying words quickly (case-insensitive).
    private static readonly HashSet<string> Keywords = new(StringComparer.OrdinalIgnoreCase)
    {
        // Sections
        "MAP", "END", "WEB", "METADATA", "OUTPUTFORMAT", "PROJECTION",
        "LEGEND", "QUERYMAP", "SCALEBAR", "LAYER", "CLASS", "STYLE", "LABEL", "VALIDATION",

        // Directives
        "NAME", "STATUS", "SIZE", "UNITS", "IMAGECOLOR", "FONTSET", "SYMBOLSET",
        "IMAGETYPE", "MAXSIZE", "FOOTER", "HEADER", "IMAGEPATH", "IMAGEURL",
        "CONFIG", "MIMETYPE", "DRIVER", "EXTENSION", "IMAGEMODE", "TRANSPARENT",
        "INTERVALS", "OFFSET", "SHADOWSIZE", "TYPE", "KEYSIZE", "KEYSPACING",
        "TEMPLATE", "TOLERANCE", "TOLERANCEUNITS", "OPACITY", "OUTLINECOLOR",
        "WIDTH", "COLOR", "DATA", "FILTER", "FILTERITEM", "CONNECTIONTYPE",
        "CONNECTION", "PROCESSING",

        // Common value-like keywords
        "RGB", "HILITE", "BITMAP",
    };

    private static readonly HashSet<string> Booleans = new(StringComparer.OrdinalIgnoreCase)
    {
        "TRUE",
        "FALSE",
        "ON",
        "OFF",
    };

    private readonly ReadOnlySpan<char> text;
    private int position;
    private int line;
    private int column;

    /// <summary>
    /// Initialises a new instance of the <see cref="MapfileTokenizer"/> struct.
    /// </summary>
    /// <param name="text">The text.</param>
    public MapfileTokenizer(ReadOnlySpan<char> text)
    {
        this.text = text;
        this.position = 0;
        this.line = 1;
        this.column = 1;
    }

    /// <summary>
    /// Pull-based API (recommended for pipelines). Returns false at end of input.
    /// </summary>
    /// <param name="token">The extracted token.</param>
    /// <returns><see langword="true"/> if a token was extracted; otherwise <see langword="false"/>.</returns>
    public bool TryReadNext(out Token token)
    {
        this.SkipWhitespaceAndComments();

        if (this.IsEOF())
        {
            token = default;
            return false;
        }

        var startLine = this.line;
        var startCol = this.column;
        var c = this.Peek();

        // Strings
        if (c is '"' || c is '\'')
        {
            token = new Token(TokenType.String, this.ReadSpan(c), startLine, startCol);
            return true;
        }

        // Bracket expression
        if (c is '[')
        {
            var span = this.ReadBracketExpression();
            token = new Token(TokenType.Attribute, span, startLine, startCol);
            return true;
        }

        // Punctuation
        if (c is '(')
        {
            int start = this.position;
            this.Advance();
            token = new Token(TokenType.LParen, this.Slice(start, 1), startLine, startCol);
            return true;
        }

        if (c is ')')
        {
            int start = this.position;
            this.Advance();
            token = new Token(TokenType.RParen, this.Slice(start, 1), startLine, startCol);
            return true;
        }

        if (c is ',')
        {
            int start = this.position;
            this.Advance();
            token = new Token(TokenType.Comma, this.Slice(start, 1), startLine, startCol);
            return true;
        }

        if (c is '=')
        {
            int start = this.position;
            this.Advance();
            token = new Token(TokenType.Equals, this.Slice(start, 1), startLine, startCol);
            return true;
        }

        // Number
        if (this.IsNumberStart(c))
        {
            var span = this.ReadNumber();
            token = new Token(TokenType.Number, span, startLine, startCol);
            return true;
        }

        // Identifier / Keyword / Boolean
        if (IsIdentifierStart(c))
        {
            var span = this.ReadIdentifier();

            if (Booleans.Contains(span))
            {
                token = new Token(TokenType.Boolean, span, startLine, startCol);
            }
            else if (Keywords.Contains(span))
            {
                token = new Token(TokenType.Keyword, span, startLine, startCol);
            }
            else
            {
                token = new Token(TokenType.Identifier, span, startLine, startCol);
            }

            return true;
        }

        // Unknown char outside strings/comments: consume 1 char to avoid infinite loop.
        // (You can also throw if you prefer strictness.)
        this.Advance();
        token = new Token(TokenType.Identifier, this.Slice(this.position - 1, 1), startLine, startCol);
        return true;
    }

    /// <summary>
    /// Enumerator-returning API so you can use foreach without allocations.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public TokenEnumerator GetEnumerator() => new(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ReadOnlySpan<char> TrimWhitespace(ReadOnlySpan<char> s)
    {
        int start = 0;
        int end = s.Length - 1;

        while (start <= end && char.IsWhiteSpace(s[start]))
        {
            start++;
        }

        while (end >= start && char.IsWhiteSpace(s[end]))
        {
            end--;
        }

        return start > end ? [] : s.Slice(start, end - start + 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsIdentifierStart(char c) => char.IsLetter(c) || c is '_';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsIdentifierPart(char c) => char.IsLetterOrDigit(c) || c is '_' or '/';

    private void SkipWhitespaceAndComments()
    {
        while (!this.IsEOF())
        {
            char c = this.Peek();

            // Whitespace
            if (c is ' ' || c is '\t' || c is '\r' || c is '\n')
            {
                this.Advance();
                continue;
            }

            // '#' : single-line comment to end-of-line
            if (c is '#')
            {
                while (!this.IsEOF() && this.Peek() != '\n')
                {
                    this.Advance();
                }

                // next loop iteration will consume the newline (if any)
                continue;
            }

            // '/* ... */' : C-style block comment (non-nested)
            if (c is '/' && this.Peek(1) is '*')
            {
                this.SkipBlockComment();
                continue;
            }

            // Not whitespace or a supported comment start
            break;
        }
    }

    private ReadOnlySpan<char> ReadSpan(char quote)
    {
        // consume opening quote
        this.Advance();
        var start = this.position;
        var escaped = false;

        while (!this.IsEOF())
        {
            var c = this.Peek();

            if (escaped)
            {
                escaped = false;
                this.Advance();
                continue;
            }

            if (c is '\\')
            {
                escaped = true;
                this.Advance();
                continue;
            }

            if (c == quote)
            {
                var length = this.position - start;

                // consume closing quote
                this.Advance();

                return this.Slice(start, length);
            }

            this.Advance();
        }

        throw new InvalidOperationException($"Unterminated string literal (started with {quote}) at line {this.line}, column {this.column}.");
    }

    private ReadOnlySpan<char> ReadBracketExpression()
    {
        var startLine = this.line;
        var startCol = this.column;

        this.Advance(); // consume '['
        var start = this.position;

        while (!this.IsEOF())
        {
            var c = this.Peek();
            if (c is ']')
            {
                var length = this.position - start;
                var inner = this.Slice(start, length);
                this.Advance(); // consume ']'
                return TrimWhitespace(inner);
            }

            this.Advance();
        }

        throw new InvalidOperationException($"Unterminated bracket expression '[' started at line {startLine}, column {startCol}.");
    }

    private ReadOnlySpan<char> ReadNumber()
    {
        var start = this.position;

        if (this.Peek() is '-')
        {
            this.Advance();
        }

        var seenDot = false;

        while (!this.IsEOF())
        {
            var c = this.Peek();
            if (char.IsDigit(c))
            {
                this.Advance();
            }
            else if (c is '.' && !seenDot)
            {
                seenDot = true;
                this.Advance();
            }
            else
            {
                break;
            }
        }

        return this.SliceFrom(start);
    }

    private ReadOnlySpan<char> ReadIdentifier()
    {
        var start = this.position;

        while (!this.IsEOF())
        {
            var c = this.Peek();
            if (IsIdentifierPart(c))
            {
                this.Advance();
            }
            else
            {
                break;
            }
        }

        return this.SliceFrom(start);
    }

    private readonly bool IsNumberStart(char c)
    {
        if (char.IsDigit(c))
        {
            return true;
        }

        if (c is '-')
        {
            return char.IsDigit(this.Peek(1));
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly bool IsEOF() => this.position >= this.text.Length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly char Peek(int lookahead = 0) =>
        this.position + lookahead is >= 0 and var peekPosition && peekPosition < this.text.Length
            ? this.text[peekPosition]
            : '\0';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Advance()
    {
        if (this.IsEOF())
        {
            return;
        }

        char c = this.text[this.position++];
        if (c is '\n')
        {
            this.line++;
            this.column = 1;
        }
        else
        {
            this.column++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly ReadOnlySpan<char> Slice(int start, int length)
    {
        if (start < 0)
        {
            start = 0;
        }

        if (length < 0)
        {
            length = 0;
        }

        if (start > this.text.Length)
        {
            start = this.text.Length;
        }

        if (start + length > this.text.Length)
        {
            length = this.text.Length - start;
        }

        return this.text.Slice(start, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly ReadOnlySpan<char> SliceFrom(int start) => this.Slice(start, this.position - start);

    private void SkipBlockComment()
    {
        // Remember where the comment started for nice error messages
        var startLine = this.line;
        var startCol = this.column;

        // Consume '/*'
        this.Advance(); // '/'
        this.Advance(); // '*'

        // Scan until we see closing '*/'
        while (!this.IsEOF())
        {
            var c = this.Peek();

            // If we see '*', we might be at the end of the comment
            if (c is '*')
            {
                this.Advance(); // consume '*'
                if (this.Peek() is '/')
                {
                    this.Advance(); // consume '/'
                    return;         // comment closed
                }

                // Not a '/', keep scanning (the '*' was part of the comment body)
                continue;
            }

            // Otherwise just advance through the comment body.
            this.Advance();
        }

        // If we drop out, there was no terminating '*/'
        throw new InvalidOperationException($"Unterminated block comment '/*' started at line {startLine}, column {startCol}.");
    }

    /// <summary>
    /// Stack-only enumerator of Token.
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public ref struct TokenEnumerator
    {
        private MapfileTokenizer lexer;
        private Token current;

        /// <summary>
        /// Initialises a new instance of the <see cref="TokenEnumerator"/> struct.
        /// </summary>
        /// <param name="lexer">The lexer.</param>
        internal TokenEnumerator(MapfileTokenizer lexer)
        {
            this.lexer = lexer;   // copy; enumerator maintains its own cursor
            this.current = default;
        }

        /// <inheritdoc cref="IEnumerator{T}.Current" />
        public Token Current => this.current;

        /// <inheritdoc cref="System.Collections.IEnumerator.MoveNext" />
        public bool MoveNext()
        {
            if (this.lexer.TryReadNext(out var t))
            {
                this.current = t;
                return true;
            }

            return false;
        }

        /// <inheritdoc cref="System.Collections.IEnumerable.GetEnumerator" />
        public TokenEnumerator GetEnumerator() => this;
    }
}