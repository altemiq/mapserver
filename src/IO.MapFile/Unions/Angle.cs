// -----------------------------------------------------------------------
// <copyright file="Angle.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

[UnionAliases("Number")]
public readonly partial struct Angle : IUnion<double, Attribute, Auto, CodeOfChaos.Unions.None>
{
    public Angle()
    {
    }
}