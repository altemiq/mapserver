// -----------------------------------------------------------------------
// <copyright file="ColorOrAttribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Unions;

public readonly partial struct ColorOrAttribute : IUnion<System.Drawing.Color, Attribute, None>
{
    public ColorOrAttribute()
    {
    }
}