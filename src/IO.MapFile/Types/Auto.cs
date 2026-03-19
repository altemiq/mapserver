// -----------------------------------------------------------------------
// <copyright file="Auto.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Types;

/// <summary>
/// The AUTO type.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Size = 1)]
public readonly struct Auto
{
    /// <summary>
    /// The empty instance.
    /// </summary>
    public static readonly Auto Empty;

    /// <inheritdoc/>
    public override string ToString() => "AUTO";
}