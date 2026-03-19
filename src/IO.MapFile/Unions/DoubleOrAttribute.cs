// -----------------------------------------------------------------------
// <copyright file="DoubleOrAttribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Unions;

/// <summary>
/// The <see cref="double"/>, <see cref="Attribute"/>, or <see cref="None"/> class.
/// </summary>
[System.ComponentModel.TypeConverter(typeof(DoubleOrAttributeConverter))]
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This can contain an attribute")]
public readonly partial struct DoubleOrAttribute() : IUnion<double, Attribute, None>;