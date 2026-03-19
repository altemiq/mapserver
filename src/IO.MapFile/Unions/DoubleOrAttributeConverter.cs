// -----------------------------------------------------------------------
// <copyright file="DoubleOrAttributeConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Unions;

/// <summary>
/// The <see cref="DoubleOrAttribute"/> <see cref="System.ComponentModel.TypeConverter"/>.
/// </summary>
public sealed class DoubleOrAttributeConverter : System.ComponentModel.TypeConverter
{
    /// <inheritdoc/>
    public override object? ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        if (value is string text)
        {
            if (text is nameof(None))
            {
                return (DoubleOrAttribute)None.Empty;
            }

            if (double.TryParse(text, culture, out var angle))
            {
                return (DoubleOrAttribute)angle;
            }

            return (DoubleOrAttribute)new Attribute(text);
        }

        return base.ConvertFrom(context, culture, value);
    }
}