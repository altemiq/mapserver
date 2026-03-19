// -----------------------------------------------------------------------
// <copyright file="Join.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile;

/// <summary>
/// JOIN block — defines a post‑query attribute join between the primary
/// feature table and an external table or dataset.
/// </summary>
/// <remarks>
/// Joins occur **after** spatial querying and are used to enrich feature
/// attributes in templates or OGC outputs. Supported join sources include
/// DBF, CSV, and databases (e.g., MySQL, PostgreSQL) depending on driver support.
/// </remarks>
public sealed class Join
{
    /// <summary>
    /// Gets or sets the logical JOIN name (<c>NAME</c>).
    /// </summary>
    public string Name { get; set; } = "join";

    /// <summary>
    /// Gets or sets the external table or filename used in the join (<c>TABLE</c>).
    /// </summary>
    public string? Table { get; set; }

    /// <summary>
    /// Gets or sets the attribute from the MAP layer used as the left side of the join (<c>FROM</c>).
    /// </summary>
    public string? From { get; set; }

    /// <summary>
    /// Gets or sets the attribute from the external table used as the right side of the join (<c>TO</c>).
    /// </summary>
    public string? To { get; set; }

    /// <summary>
    /// Gets or sets the template used to output one‑to‑many joined rows (<c>TEMPLATE</c>).
    /// </summary>
    public string? Template { get; set; }

    /// <summary>
    /// Gets or sets an optional header template for one‑to‑many joined output (<c>HEADER</c>).
    /// </summary>
    public string? Header { get; set; }

    /// <summary>
    /// Gets or sets an optional footer template for one‑to‑many joined output (<c>FOOTER</c>).
    /// </summary>
    public string? Footer { get; set; }

    /// <summary>
    /// Gets or sets the connection type (e.g., <c>csv</c>, <c>mysql</c>, <c>postgresql</c>).
    /// DBF/CSV joins normally omit <c>CONNECTIONTYPE</c>.
    /// </summary>
    public string? ConnectionType { get; set; }

    /// <summary>
    /// Gets or sets the database connection string (for DB‑backed joins) (<c>CONNECTION</c>).
    /// </summary>
    public string? Connection { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this JOIN produces a one‑to‑many result set (<c>ONE-TO-MANY</c>).
    /// </summary>
    public bool OneToMany { get; set; }
}