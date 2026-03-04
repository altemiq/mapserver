// -----------------------------------------------------------------------
// <copyright file="Attribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

using System;
using System.Collections.Generic;
using System.Text;

public readonly struct Attribute
{
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

    public string Value { get; }

    public override string ToString() => $"[{this.Value}]";
}