// -----------------------------------------------------------------------
// <copyright file="DoubleOrAttribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Unions;

public readonly partial struct DoubleOrAttribute : IUnion<double, Attribute, None>
{
    public DoubleOrAttribute()
    {
    }
}