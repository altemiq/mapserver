// -----------------------------------------------------------------------
// <copyright file="ColorOrAttributeConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Unions;

/// <summary>
/// The <see cref="ColorOrAttribute"/> <see cref="System.ComponentModel.TypeConverter"/>.
/// </summary>
public sealed class ColorOrAttributeConverter : System.ComponentModel.TypeConverter
{
    /// <inheritdoc/>
    public override object? ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        if (value is string text)
        {
            if (text is nameof(None))
            {
                return (ColorOrAttribute)None.Empty;
            }

            var colorConverter = new System.Drawing.ColorConverter();
            if (colorConverter.ConvertFromString(context, culture, text) is { } color)
            {
                return (ColorOrAttribute)color;
            }

            return (ColorOrAttribute)new Attribute(text);
        }

        return base.ConvertFrom(context, culture, value);
    }
}