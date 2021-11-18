using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;


namespace Entry_Samples.Views
{
    public partial class EntryView : ContentPage
    {
        public EntryView()
        {
            InitializeComponent();
        }

        void EntryCompleted(object sender, EventArgs e)
        {
            var editor = (Entry)sender;

            labelInteractivity.Text = editor.Text;
        }

        void EntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var oldText = e.OldTextValue;
            var newText = e.NewTextValue;

            labelInteractivity.Text = newText;
        }

        private void Entry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var editor = (Entry)sender;

            if (e.PropertyName == nameof(Entry.Text))
            {
                labelInteractivity.Text = editor.Text;
            }
        }

        private void Entry_FocusedAndUnFocused(object sender, FocusEventArgs e)
        {
            var editor = (Entry)sender;
            labelInteractivity.Text = $"Focused: {e.IsFocused}, Value: {editor.Text}";
        }
    }
}