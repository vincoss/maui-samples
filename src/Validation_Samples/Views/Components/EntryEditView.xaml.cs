
using MauiSharedLibrary.Validation;
using MauiSharedLibrary.ViewModels;
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace Validation_Samples.Views.Components;

public partial class EntryEditView : ContentPage
{
    public EntryEditView()
    {
        InitializeComponent();
    }
}

public class EntryEditViewModel : BaseValidationViewModel
{
    private readonly IEditorService _editorService;

    public EntryEditViewModel(IEditorService editorService)
    {
        _editorService = editorService;

        CancelCommand = new Command(OnCancelCommand);
        OkCommand = new Command(OnOkCommand);
    }

    #region Command methods

    private async void OnCancelCommand()
    {
        await App.Current.MainPage.Navigation.PopModalAsync();
    }

    private async void OnOkCommand()
    {
        _editorService.Validate(Value, ModelState);
        OnPropertyChanged("Item");

        if (ModelState.IsValid) // valid
        {
            _editorService.Save(Value);
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }

    #endregion

    #region Commands

    public ICommand CancelCommand { get; private set; }
    public ICommand OkCommand { get; private set; }

    #endregion

    private string _value;

    public string Value
    {
        get { return _value; }
        set { SetProperty(ref _value, value); }
    }
}

public interface IEditorService
{
    void Validate(string value, ModelStateDictionary modelState);
    void Save(string value);
}

