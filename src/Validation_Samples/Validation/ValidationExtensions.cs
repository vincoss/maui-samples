using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ShortMvvm.Validation
{
    public static class ValidationExtensions
    {
        public static void ValidateToModel(this IValidator validator, object value, ModelStateDictionary model)
        {
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            model.Clear();

            var context = new ValidationContext<object>(value);
            var result = validator.Validate(context);
            foreach (var error in result.Errors)
            {
                model.AddError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}