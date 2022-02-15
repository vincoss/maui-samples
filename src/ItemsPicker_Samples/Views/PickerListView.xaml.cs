using ItemsPicker_Samples.ViewModels;

namespace ItemsPicker_Samples.Views
{
    public partial class PickerListView : ContentPage
    {
        private KeywordPickerViewModel _model = new KeywordPickerViewModel();

        public PickerListView()
        {
            InitializeComponent();

            this.BindingContext = _model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _model.Initialize();
        }
    }
}