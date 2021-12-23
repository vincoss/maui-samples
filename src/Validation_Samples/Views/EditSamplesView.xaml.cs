using MauiSharedLibrary.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Validation_Samples.Views.Components;
using System;
using System.Windows.Input;
using MauiSharedLibrary.Validation;
using Validation_Samples.Validation;

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
        public static string EditEntryName = "Name";
        public static string EditEditorDescription = "Description";

        public EditSamplesViewModel()
        {
            TextEditCommand = new Command<string>(OnTextEditCommand);

            Name = "Edit value in Entry";
            Description = "Edit value in Editor";
        }

        #region Command methods
        
        private async void OnTextEditCommand(string value)
        {
            if(value == EditEntryName)
            {
                var page = new EntryEditView();
                var model = new EntryEditViewModel(new EditorService());
                page.BindingContext = model;
                model.Value = value;

                await App.Current.MainPage.Navigation.PushModalAsync(page);
            }
            if (value == EditEditorDescription)
            {

            }
        }

        private void OnEditorEditCommand(string value)
        {

        }

        #endregion

        #region Commands

        public ICommand TextEditCommand { get; private set; }
        
        #endregion

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        class EditorService : IEditorService
        {
            public async void Save(string value)
            {
                await App.Current.MainPage.DisplayAlert("Save", value, "OK");
            }

            public void Validate(string value, ModelStateDictionary modelState)
            {
                var data = new ValidationString
                { 
                    Value = value
                };

                var validator = new BoardNameValidator();
                validator.ValidateToModel(data, modelState);
            }
        }

    }
}
