// -----------------------------------------------------------------------
// <copyright file="OutputFormatValidator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// The <see cref="OutputFormat"/> <see cref="FluentValidation.IValidator"/>.
/// </summary>
public sealed class OutputFormatValidator : AbstractValidator<OutputFormat>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="OutputFormatValidator"/> class.
    /// </summary>
    public OutputFormatValidator()
    {
        this.RuleFor(static of => of.Name).NotEmpty().WithMessage("OUTPUTFORMAT NAME is required.");
        this.RuleFor(static of => of.Driver).NotEmpty().WithMessage("OUTPUTFORMAT DRIVER is required.");

        // FORMATOPTION: non-empty key is required
        this.RuleForEach(static of => of.FormatOptions).Custom(static (kv, context) =>
        {
            if (string.IsNullOrWhiteSpace(kv.Key))
            {
                context.AddFailure("FormatOptions", "FORMATOPTION must have a non-empty key.");
            }
        });
    }
}