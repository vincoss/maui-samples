using ItemsPicker_Samples.ViewModels;

namespace ItemsPicker_Samples.Views
{
    public partial class PickerListView : ContentPage
    {
        public PickerListView()
        {
            InitializeComponent();

            var model = new KeywordPickerViewModel();
            this.BindingContext = model;
            model.Initialize();
        }
    }
}