﻿using CollectionView_Samples.ViewModels;
using MauiSharedLibrary.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CollectionView_Samples.Views
{
    public partial class VerticalCollectionCheckboxView : ContentPage
    {
        private VerticalCollectionCheckboxViewModel _model = new VerticalCollectionCheckboxViewModel();

        public VerticalCollectionCheckboxView()
        {
            InitializeComponent();

            BindingContext = _model;
            _model.Initialize();
        }
    }

    public class VerticalCollectionCheckboxViewModel : BaseViewModel
    {
        public VerticalCollectionCheckboxViewModel()
        {
            ItemTapCommand = new Command<SelectListItem>(OnItemTapCommand);
            ItemsSource = new ObservableCollection<SelectListItem>();
        }

        public override void Initialize()
        {
            try
            {
                IsBusy = true;

                var a = new SelectListItem { Key = 1, Value = "Milk" };
                var b = new SelectListItem { Key = 2, Value = "Bread", IsSelected = true };
                var c = new SelectListItem { Key = 3, Value = "Butter" };

                ItemsSource.Add(a);
                ItemsSource.Add(b);
                ItemsSource.Add(c);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private void OnItemTapCommand(SelectListItem dto)
        {
            var flag = dto.IsSelected;
            if (IsSingleSelection)
            {
                ClearSelection();
            }
            dto.IsSelected = !flag;
        }

        private void ClearSelection()
        {
            foreach (var item in ItemsSource)
            {
                item.IsSelected = false;
            }
        }

        public ICommand ItemTapCommand { get; private set; }

        public ObservableCollection<SelectListItem> ItemsSource { get; private set; }

        private bool _isSingleSelection;

        public bool IsSingleSelection
        {
            get { return _isSingleSelection; }
            set { SetProperty(ref _isSingleSelection, value); }
        }

    }
}