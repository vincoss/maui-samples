using MauiSharedLibrary.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CollectionView_Samples.Views
{
    public partial class CollectionViewAddView : ContentPage
    {
        private CollectionViewAddViewModel _model = new CollectionViewAddViewModel();

        public CollectionViewAddView()
        {
            InitializeComponent();
            BindingContext = _model;
            _model.Initialize();
        }

        private void eNewItem_Completed(object sender, EventArgs e)
        {
            var entry = sender as Entry;
            var text = entry.Text;

            if (string.IsNullOrWhiteSpace(text) == false)
            {
                _model.NewItem = text;
                entry.Text = "";
            }
        }
    }

    public class CollectionViewAddViewModel : BaseViewModel
    {
        public CollectionViewAddViewModel()
        {
            RefreshCommand = new Command(Initialize);
            AddCardCommand = new Command(OnAddCardCommand);
            ItemsSource = new ObservableCollection<ItemDto>();
            this.PropertyChanged += CollectionViewAddViewModel_PropertyChanged;
        }

        public override void Initialize()
        {
            try
            {
                IsBusy = true;

                ItemsSource.Add(new ItemDto { Name = "A" });
                ItemsSource.Add(new ItemDto { Name = "B" });

            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CollectionViewAddViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewItem))
            {
                if (string.IsNullOrWhiteSpace(NewItem) == false)
                {
                    ItemsSource.Add(new ItemDto { Name = NewItem });
                    NewItem = null;
                }
            }
        }

        #region Command methods

        public void OnAddCardCommand()
        {
            IsEditing = !IsEditing;
        }

        #endregion

        public ICommand LoadMoreCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public ICommand AddCardCommand { get; private set; }

        public ObservableCollection<ItemDto> ItemsSource { get; private set; }

        private string _newItem;

        public string NewItem
        {
            get { return _newItem; }
            set { SetProperty(ref _newItem, value); }
        }

        private bool _isEditing;

        public bool IsEditing
        {
            get { return _isEditing; }
            set { SetProperty(ref _isEditing, value); }
        }

        public class ItemDto
        {
            public string Name { get; set; }
        }
    }

    public class NextEntryBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty NextEntryProperty = 
            BindableProperty.Create(nameof(NextEntry), typeof(Entry), typeof(Entry), defaultBindingMode: BindingMode.OneTime, defaultValue: null);

        public Entry NextEntry
        {
            get => (Entry)GetValue(NextEntryProperty);
            set => SetValue(NextEntryProperty, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.Completed += Bindable_Completed;
            base.OnAttachedTo(bindable);
        }

        private void Bindable_Completed(object sender, EventArgs e)
        {
            if (NextEntry != null)
            {
                NextEntry.Focus();
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.Completed -= Bindable_Completed;
            base.OnDetachingFrom(bindable);
        }
    }
}
