using ShortMvvm.ViewModels;
using Microsoft.Maui.Controls;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarouselView_Samples.Views
{
    /*
      pull down refresh all columns cards
      auto refrehs board including, columns and cards
  */

    public partial class PageWithCarouselView : ContentPage
    {
        public PageWithCarouselView()
        {
            InitializeComponent();
            var model = new PageWithCarouselViewModel();
            BindingContext = model;
            model.Initialize();
        }
    }

    public class PageWithCarouselViewModel : BaseViewModel
    {
        public PageWithCarouselViewModel()
        {
            RefreshCommand = new Command(Initialize);
            Columns = new ObservableCollection<ColumnDto>();
        }

        public override void Initialize()
        {
            try
            {
                IsBusy = true;

                BoardTitle = "Welcome Board!!!";

                var bag = new ColumnDto[]
                {
                    new ColumnDto
                    {
                        Name = "Backlog"
                    },
                    new ColumnDto {  Name = "To Do" },
                    new ColumnDto {  Name = "Blocked" },
                    new ColumnDto {  Name = "In Progress" },
                    new ColumnDto {  Name = "Review" },
                    new ColumnDto {  Name = "Done" },
                };

                bag[0].Cards.Add(new CardListItemDto { Name = "A" });
                bag[0].Cards.Add(new CardListItemDto { Name = "B" });
                bag[0].Cards.Add(new CardListItemDto { Name = "C" });

                bag[2].Cards.Add(new CardListItemDto { Name = "A1" });

                Columns.Clear();
                foreach (var item in bag)
                {
                    Columns.Add(item);
                }

                Position = 2;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand LoadMoreCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        private string _boardTitle;

        public string BoardTitle
        {
            get { return _boardTitle; }
            set { SetProperty(ref _boardTitle, value); }
        }

        public ObservableCollection<ColumnDto> Columns { get; private set; }

        private ColumnDto _selectedItem;

        public ColumnDto SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private int _position;

        public int Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        public class ColumnDto
        {
            public ColumnDto()
            {
                Cards = new ObservableCollection<CardListItemDto>();
            }

            public string Name { get; set; }

            public int Count
            {
                get { return Cards.Count; }
            }

            public ObservableCollection<CardListItemDto> Cards { get; private set; }
        }

        public class CardListItemDto
        {
            public string Name { get; set; }

            public override string ToString()
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    return base.ToString();
                }
                return Name;
            }
        }
    }
}