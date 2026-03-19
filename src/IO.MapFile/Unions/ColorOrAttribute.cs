// -----------------------------------------------------------------------
// <copyright file="ColorOrAttribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Unions;

/// <summary>
/// The <see cref="System.Drawing.Color"/>, <see cref="Attribute"/>, or <see cref="None"/> value.
/// </summary>
[System.ComponentModel.TypeConverter(typeof(ColorOrAttributeConverter))]
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This can contain an attribute")]
public readonly partial struct ColorOrAttribute() : IUnion<System.Drawing.Color, Attribute, None>;