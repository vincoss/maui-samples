namespace CollectionView_Samples.Views;

public partial class Headers2View : ContentPage
{
	public Headers2View()
	{
		InitializeComponent();
        this.BindingContext = new HeadersViewModel();

    }

    public class HeadersViewModel
    {

        public HeadersViewModel()
        {
            Items = new[]
            {
                new DataModel { Title = "One", Description = "One description"},
                new DataModel { Title = "Two", Description = "Two description"}
            };
        }

        public IEnumerable<DataModel> Items { get; set; }

    }

    public class DataModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}