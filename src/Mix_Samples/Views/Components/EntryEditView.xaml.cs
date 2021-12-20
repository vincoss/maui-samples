
using MauiSharedLibrary.ViewModels;
using Microsoft.Maui.Controls;

namespace Mix_Samples.Views.Components;

public partial class EntryEditView : ContentPage
{
    public EntryEditView()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void Button_Clicked_1(object sender, System.EventArgs e)
    {
        //// service.Validate(ModelState)

        //if(ModelState.IsValid) // valid
        //{
        //  // service.Save
            //await Navigation.PopModalAsync();
        //}

        // validate
        // call save

        // service.SaveAsync(ModelState)

        await Navigation.PopModalAsync();
    }
}

public class EntryEditViewModel : BaseViewModel
{
    private readonly IEditorService _editorService;

    public EntryEditViewModel(IEditorService editorService)
    {
        _editorService = editorService;
    }

    private string _editValue;

    public string EditValue
    {
        get { return _editValue; }
        set { SetProperty(ref _editValue, value); }
    }
}

public interface IEditorService
{
    void Validate();
    void Save();
}

