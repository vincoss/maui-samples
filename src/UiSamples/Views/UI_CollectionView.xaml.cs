using MauiSharedLibrary.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace UiSamples.Views
{
    public partial class UI_CollectionView : ContentPage
    {
        private UI_CollectionViewModel _viewModel = new UI_CollectionViewModel();

        public UI_CollectionView()
        {
            InitializeComponent();

            BindingContext = _viewModel;
        }

        private void addItem_Clicked(object sender, EventArgs e)
        {
            _viewModel.Add();
        }

        private void btnAddValue_Clicked(object sender, EventArgs e)
        {
            double value;
            if (double.TryParse(doubleEntry.Text, out value))
            {
                _viewModel.Numbers.Add(value);
            }
        }

        public class UI_CollectionViewModel : BaseViewModel
        {
            public UI_CollectionViewModel()
            {
                Items = new ObservableCollection<int>();
                Numbers = new ObservableCollection<double>();

                TagColors = new List<string>
                {
                    "#ffe6e6", "#ffddcc", "#ccffcc", "#d6f5f5", "#ccddff", "#ffffcc",
                    "#e6e6e6", "#b3ccff", "#b3ffb3", "#ffc6b3", "#ffb3d9", "#9999ff"
                };
            }

            public void Add()
            {
                var item = Items.Count + 1;
                Items.Add(item);
                SelectedItem = item;
            }

            private int _value;

            public int SelectedItem
            {
                get { return _value; }
                set { SetProperty(ref _value, value); }
            }

            public ObservableCollection<int> Items { get; private set; }
            public ObservableCollection<double> Numbers { get; private set; }

            public IEnumerable<string> TagColors { get; set; }
        }
    }
}
