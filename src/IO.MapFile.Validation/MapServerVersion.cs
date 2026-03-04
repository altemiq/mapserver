// -----------------------------------------------------------------------
// <copyright file="MapServerVersion.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// The mapserver version.
/// </summary>
/// <param name="Major">The major version.</param>
/// <param name="Minor">The minor version.</param>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public readonly record struct MapServerVersion(int Major, int Minor) : IComparable<MapServerVersion>
{
    /// <summary>
    /// Implements the less than operator.
    /// </summary>
    /// <param name="left">The left hand side.</param>
    /// <param name="right">The right hand size.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <(MapServerVersion left, MapServerVersion right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Implements the less than or equals operator.
    /// </summary>
    /// <param name="left">The left hand side.</param>
    /// <param name="right">The right hand size.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <=(MapServerVersion left, MapServerVersion right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Implements the greater than operator.
    /// </summary>
    /// <param name="left">The left hand side.</param>
    /// <param name="right">The right hand size.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >(MapServerVersion left, MapServerVersion right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Implements the greater than or equals operator.
    /// </summary>
    /// <param name="left">The left hand side.</param>
    /// <param name="right">The right hand size.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >=(MapServerVersion left, MapServerVersion right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public int CompareTo(MapServerVersion other) => this.Major != other.Major ? this.Major.CompareTo(other.Major) : this.Minor.CompareTo(other.Minor);

    /// <summary>
    /// Determines if this instance is equal to or greater than the version specified by <paramref name="major"/> and <paramref name="minor"/>.
    /// </summary>
    /// <param name="major">The major version.</param>
    /// <param name="minor">The minor version.</param>
    /// <returns><see langword="true"/> if this instance is equal to or greater than the version specified by <paramref name="major"/> and <paramref name="minor"/>; otherwise <see langword="false"/>.</returns>
    public bool IsAtLeast(int major, int minor) => this >= new MapServerVersion(major, minor);

    /// <inheritdoc/>
    public override string ToString() => $"{this.Major}.{this.Minor}";
}