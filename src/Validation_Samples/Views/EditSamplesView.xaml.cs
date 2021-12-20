using MauiSharedLibrary.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Validation_Samples.Views.Components;
using System;
using System.Windows.Input;

namespace Validation_Samples.Views
{
    public partial class EditSamplesView : ContentPage
    {
        public EditSamplesView()
        {
            InitializeComponent();

            BindingContext = new EditSamplesViewModel();
        }
    }

    public class EditSamplesViewModel : BaseViewModel
    {
        public EditSamplesViewModel()
        {
            EntryEditCommand = new Command<string>(OnEntryEditCommand);
            EditorEditCommand = new Command<string>(OnEditorEditCommand);
        }

        #region Command methods
        
        private async void OnEntryEditCommand(string value)
        {
            var page = new EntryEditView();
            var model = new EntryEditViewModel(new EditorService());
            page.BindingContext = model;
            model.EditValue = value;

            await App.Current.MainPage.Navigation.PushModalAsync(page);
        }

        private void OnEditorEditCommand(string value)
        {

        }

        #endregion

        #region Commands

        public ICommand EntryEditCommand { get; private set; }
        public ICommand EditorEditCommand { get; private set; } 
        
        #endregion

        private string _entryValue;

        public string EntryValue
        {
            get { return _entryValue; }
            set { SetProperty(ref _entryValue, value); }
        }

        class EditorService : IEditorService
        {
            public void Save()
            {
                throw new NotImplementedException();
            }

            public void Validate()
            {
                throw new NotImplementedException();
            }
        }

    }
}
