using CommunityToolkit.Mvvm.Input;
using Default_MauiApp.Models;

namespace Default_MauiApp.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}