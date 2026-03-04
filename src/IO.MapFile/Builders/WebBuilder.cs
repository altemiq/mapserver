// -----------------------------------------------------------------------
// <copyright file="WebBuilder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Builders;

/// <summary>
/// Fluent builder for <see cref="Web"/> (CGI integration and templates).
/// </summary>
public sealed class WebBuilder
{
    private readonly Web web = new();

    private WebBuilder()
    {
    }

    public static WebBuilder New() => new();

    public static WebBuilder From(Web w)
    {
        var b = New()
            .BrowseFormat(w.BrowseFormat)
            .LegendFormat(w.LegendFormat)
            .EmptyUrl(w.EmptyUrl)
            .ErrorUrl(w.ErrorUrl)
            .HeaderTemplate(w.HeaderTemplate)
            .FooterTemplate(w.FooterTemplate)
            .ImagePath(w.ImagePath)
            .ImageUrl(w.ImageUrl)
            .MaxScaleDenom(w.MaxScaleDenom)
            .MaxTemplate(w.MaxTemplate);

        foreach (var kv in w.Metadata)
        {
            b.Metadata(kv.Key, kv.Value);
        }

        foreach (var kv in w.Validation)
        {
            b.Validation(kv.Key, kv.Value);
        }

        return b;
    }

    public WebBuilder BrowseFormat(string? v)
    {
        this.web.BrowseFormat = v;
        return this;
    }

    public WebBuilder LegendFormat(string? v)
    {
        this.web.LegendFormat = v;
        return this;
    }

    public WebBuilder EmptyUrl(string? v)
    {
        this.web.EmptyUrl = v;
        return this;
    }

    public WebBuilder ErrorUrl(string? v)
    {
        this.web.ErrorUrl = v;
        return this;
    }

    public WebBuilder HeaderTemplate(string? v)
    {
        this.web.HeaderTemplate = v;
        return this;
    }

    public WebBuilder FooterTemplate(string? v)
    {
        this.web.FooterTemplate = v;
        return this;
    }

    public WebBuilder ImagePath(string? v)
    {
        this.web.ImagePath = v;
        return this;
    }

    public WebBuilder ImageUrl(string? v)
    {
        this.web.ImageUrl = v;
        return this;
    }

    public WebBuilder MaxScaleDenom(double? v)
    {
        this.web.MaxScaleDenom = v;
        return this;
    }

    public WebBuilder MaxTemplate(string? v)
    {
        this.web.MaxTemplate = v;
        return this;
    }

    /// <summary>Add or replace a WEB METADATA key/value.</summary>
    /// <param name="key">Tke key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The builder for chaining.</returns>
    public WebBuilder Metadata(string key, string value)
    {
        this.web.Metadata[key] = value;
        return this;
    }

    /// <summary>Add or replace a WEB VALIDATION key/value.</summary>
    /// <param name="key">Tke key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The builder for chaining.</returns>
    public WebBuilder Validation(string key, string value)
    {
        this.web.Validation[key] = value;
        return this;
    }

    public Web Build() => this.web;
}
