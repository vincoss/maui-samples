using System.Text;

namespace MauiApp1;

public partial class ListAllFilesInAppInstallPathView : ContentPage
{
	public ListAllFilesInAppInstallPathView()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		var directory = AppContext.BaseDirectory;
        var parent1 = Directory.GetParent(directory);
        var parent2 = Directory.GetParent(directory);

        var files = new List<string>();

        DirSearch(files, parent2.FullName);
        DirSearch(files, parent1.FullName);
        DirSearch(files, directory);
        var sb = new StringBuilder();

        foreach (var file in files)
        {
            sb.AppendLine(file);
        }

        editorList.Text = sb.ToString();
        //await this.DisplayAlert("Files", sb.ToString(), "OK");
    }

    private void DirSearch(List<string> files, string directory)
    {
        files.Add(directory);

        try
        {
            foreach (string f in Directory.GetFiles(directory))
            {
                files.Add(f);
            }
            foreach (string d in Directory.GetDirectories(directory))
            {
                DirSearch(files, d);
            }
        }
        catch (System.Exception ex)
        {
            files.Add(ex.Message);
        }
    }
}