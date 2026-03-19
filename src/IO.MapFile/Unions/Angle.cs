// -----------------------------------------------------------------------
// <copyright file="Angle.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// The <see cref="double"/>, <see cref="Attribute"/>, <see cref="Auto"/>, or <see cref="None"/> angle.
/// </summary>
[UnionAliases("Number")]
[System.ComponentModel.TypeConverter(typeof(AngleConverter))]
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly partial struct Angle() : IUnion<double, Attribute, Auto, None>;