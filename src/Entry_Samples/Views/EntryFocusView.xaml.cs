using ShortMvvm.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace Entry_Samples.Views
{
    public partial class EntryFocusView : ContentPage
    {
        public EntryFocusView()
        {
            InitializeComponent();

            this.Appearing += EditorView_Appearing;
        }

        private void EditorView_Appearing(object sender, EventArgs e)
        {
            /*
            New show focus and keyboard
            Edit focus no keyboard
             */

            var model = this.BindingContext as EntryFocusViewModel;
            if (model != null && model.IsNew)
            {
                entryA.Focus();
            }
            else
            {
                entryA.CursorPosition = int.MaxValue;
            }
        }
    }

    public class EntryFocusViewModel : BaseViewModel
    {
        private bool _isNew;
        public bool IsNew
        {
            get { return _isNew; }
            set { SetProperty(ref _isNew, value); }
        }

        private string _a;
        public string A
        {
            get { return _a; }
            set { SetProperty(ref _a, value); }
        }
    }
}