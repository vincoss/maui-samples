using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entry_Samples.Xaml
{
    public class NumericValidationBehavior : Behavior<Entry>
    {

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        // private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {

            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                // bool isValid = !_regex.IsMatch(args.NewTextValue);
                //double value;
                // bool isValid = double.TryParse(args.NewTextValue, out value);
                bool isValid = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers

                ((Entry)sender).Text = isValid ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
            }
        }
    }
}
