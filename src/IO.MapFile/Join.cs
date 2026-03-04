// -----------------------------------------------------------------------
// <copyright file="Join.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// JOIN block – post-query attribute table join configuration.
/// </summary>
public sealed class Join
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = "join";

    /// <summary>
    /// Gets or sets the filename or table name.
    /// </summary>
    public string? Table { get; set; }

    /// <summary>
    /// Gets or sets join column in dataset.
    /// </summary>
    public string? From { get; set; }

    /// <summary>
    /// Gets or sets join column in lookup table.
    /// </summary>
    public string? To { get; set; }

    /// <summary>
    /// Gets or sets used for one-to-many outputs.
    /// </summary>
    public string? Template { get; set; }

    /// <summary>
    /// Gets or sets the header.
    /// </summary>
    public string? Header { get; set; }

    /// <summary>
    /// Gets or sets the footer.
    /// </summary>
    public string? Footer { get; set; }

    /// <summary>Gets or sets csv|mysql|postgresql (DBF/CSV may omit CONNECTIONTYPE &amp; CONNECTION).</summary>
    public string? ConnectionType { get; set; }

    /// <summary>Gets or sets dB connection string (for DB joins).</summary>
    public string? Connection { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is a one-to-many.
    /// </summary>
    public bool OneToMany { get; set; }
}