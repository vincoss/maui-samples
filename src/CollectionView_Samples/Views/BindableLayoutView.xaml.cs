using ShortMvvm.ViewModels;
using Microsoft.Maui.Controls;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CollectionView_Samples.Views
{
    public partial class BindableLayoutView : ContentPage
    {
        private BindableLayoutViewModel _model = new BindableLayoutViewModel();

        public BindableLayoutView()
        {
            InitializeComponent();
            BindingContext = _model;
        }
    }

    public class BindableLayoutViewModel
    {

        public BindableLayoutViewModel()
        {
            Items = Enumerable.Repeat("BindableLayoutViewModel (50)", 200);

        }

        public IEnumerable<string> Items { get; set; }

    }
}
