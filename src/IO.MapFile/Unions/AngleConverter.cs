// -----------------------------------------------------------------------
// <copyright file="AngleConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Unions;

/// <summary>
/// The <see cref="Angle"/> <see cref="System.ComponentModel.TypeConverter"/>.
/// </summary>
public sealed class AngleConverter : System.ComponentModel.TypeConverter
{
    /// <inheritdoc/>
    public override object? ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        if (value is string text)
        {
            if (text is nameof(None))
            {
                return (Angle)None.Empty;
            }

            if (text is nameof(Auto))
            {
                return (Angle)Auto.Empty;
            }

            if (double.TryParse(text, culture, out var angle))
            {
                return (Angle)angle;
            }

            return (Angle)new Attribute(text);
        }

        return base.ConvertFrom(context, culture, value);
    }
}