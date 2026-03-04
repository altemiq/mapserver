// -----------------------------------------------------------------------
// <copyright file="ClassValidator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapFile.Validation;

/// <summary>
/// The <see cref="Class"/> <see cref="FluentValidation.IValidator"/>.
/// </summary>
public sealed class ClassValidator : AbstractValidator<Class>
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ClassValidator"/> class.
    /// </summary>
    public ClassValidator()
    {
        // Expression can be raw; warn on empty strings
        this.RuleFor(c => c.Expression)
            .Must(e => !string.IsNullOrWhiteSpace(e))
            .When(c => c.Expression is not null)
            .WithMessage("Empty CLASS EXPRESSION provided.")
            .WithSeverity(Severity.Warning);

        this.RuleForEach(c => c.Styles).SetValidator(new StyleValidator());
        this.RuleForEach(c => c.Labels).SetValidator(new LabelValidator());
    }
}