using Microsoft.Maui.Controls;

using System;

namespace Mix_Samples.Views
{
    public partial class StringFormatView : ContentPage
    {
        public StringFormatView()
        {
            InitializeComponent();

            BindingContext = new StringFormatViewModel();
        }
    }

    public class StringFormatViewModel
    {
        public int TotalRecords { get; set; } = 100;
        public DateTime Birthday { get; set; } = DateTime.Now.AddYears(-44);
        public DateTime? ModifiedDate { get; set; } = DateTime.Now.AddDays(-3);
        public string FirstName { get; set; } = "Ferdinand";
        public double Value { get; set; } = Math.PI;
    }

}
