// -----------------------------------------------------------------------
// <copyright file="LabelPosition.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// Label placement relative to the feature, matching MapServer’s <c>POSITION</c>
/// keyword (e.g., <c>UR</c>, <c>LC</c>, <c>AUTO</c>).
/// </summary>
/// <remarks>
/// All two‑letter codes correspond exactly to MapServer’s label placement grid:
/// <code>
///     UL  UC  UR
///     CL  CC  CR
///     LL  LC  LR
/// </code>
/// <c>AUTO</c> delegates selection to MapServer.
/// </remarks>
public enum LabelPosition
{
    /// <summary>
    /// Automatic placement (<c>AUTO</c>).
    /// </summary>
    Auto,

    /// <summary>
    /// Center‑center (<c>CC</c>).
    /// </summary>
    CC,

    /// <summary>
    /// Upper‑center (<c>UC</c>).
    /// </summary>
    UC,

    /// <summary>
    /// Lower‑center (<c>LC</c>).
    /// </summary>
    LC,

    /// <summary>
    /// Center‑left (<c>CL</c>).
    /// </summary>
    CL,

    /// <summary>
    /// Center‑right (<c>CR</c>).
    /// </summary>
    CR,

    /// <summary>
    /// Upper‑left (<c>UL</c>).
    /// </summary>
    UL,

    /// <summary>
    /// Upper‑right (<c>UR</c>).
    /// </summary>
    UR,

    /// <summary>
    /// Lower‑left (<c>LL</c>).
    /// </summary>
    LL,

    /// <summary>
    /// Lower‑right (<c>LR</c>).
    /// </summary>
    LR,
}