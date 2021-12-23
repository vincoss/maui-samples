using FluentValidation;
using MauiSharedLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation_Samples.Validation
{
    public class BoardNameValidator : AbstractValidator<ValidationString>
    {
        public BoardNameValidator()
        {
            RuleFor(x => x.Value).NotNull().Length(8, 20);
        }
    }
}
