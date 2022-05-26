using ShortMvvm.ViewModels;
using Microsoft.Maui.Controls;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CollectionView_Samples.Views
{
    public partial class VerticalCollectionViewGroupingView : ContentPage
    {
        private VerticalCollectionViewGroupingViewModel _model = new VerticalCollectionViewGroupingViewModel();

        public VerticalCollectionViewGroupingView()
        {
            InitializeComponent();
            BindingContext = _model;
            _model.Initialize();
        }
    }

    public class VerticalCollectionViewGroupingViewModel : BaseViewModel
    {
        public VerticalCollectionViewGroupingViewModel()
        {
            ItemsSource = new ObservableCollection<ItemGroup>();
        }

        public override void Initialize()
        {
            try
            {
                IsBusy = true;

                var recent = new ItemGroup("Recent searches",
                        new List<ItemDto>
                        {
                            new ItemDto { Name = "Search a"},
                            new ItemDto { Name = "Search b"},
                        }
                    );

                var boards = new ItemGroup("Boards",
                       new List<ItemDto>
                       {
                            new ItemDto { Name = "Board A"},
                       }
                   );

                var cards = new ItemGroup("Cards",
                       new List<ItemDto>
                       {
                            new ItemDto { Name = "Card A"},
                       }
                   );

                var folders = new ItemGroup("Folders",
                     new List<ItemDto>
                     {
                            new ItemDto { Name = "Folder A"},
                     }
                 );

                var includeEmptyGroups = false; // Note is what to show empty groups

                if (includeEmptyGroups)
                {
                    var empty = new ItemGroup("Empty", new List<ItemDto>());
                    ItemsSource.Add(empty);
                }

                ItemsSource.Add(recent);
                ItemsSource.Add(boards);
                ItemsSource.Add(cards);
                ItemsSource.Add(folders);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ObservableCollection<ItemGroup> ItemsSource { get; private set; }

        public class ItemDto
        {
            public string Name { get; set; }
        }

        public class ItemGroup : List<ItemDto>
        {
            public string Name { get; private set; }

            public ItemGroup(string name, List<ItemDto> items) : base(items)
            {
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
