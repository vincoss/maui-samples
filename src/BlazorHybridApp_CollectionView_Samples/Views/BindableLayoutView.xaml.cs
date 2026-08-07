using BlazorHybridApp_CollectionView_Samples.ViewModels;
using Microsoft.Maui.Controls;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BlazorHybridApp_CollectionView_Samples.Views
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

            ItemTapCommand = new Command<string>(OnItemTapCommand);
        }

        private void OnItemTapCommand(string dto)
        {
           
        }

        public IEnumerable<string> Items { get; set; }

        public ICommand ItemTapCommand { get; private set; }
    }
}
