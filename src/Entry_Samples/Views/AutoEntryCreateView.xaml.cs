using ShortMvvm.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Entry_Samples.Views
{
    public partial class AutoEntryCreateView : ContentPage
    {
        public AutoEntryCreateView()
        {
            InitializeComponent();

            var model = new AutoEntryCreateViewModel();
            BindingContext = model;

        }


    }

    public class AutoEntryCreateViewModel : BaseViewModel
    {
        public AutoEntryCreateViewModel()
        {
            ItemsSource = new FullyObservableCollection<ListItem>();
            ItemsSource.ItemPropertyChanged += ItemsSource_ItemPropertyChanged;

            ItemsSource.Add(new ListItem { Index = 1 });
        }

        private void ItemsSource_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            var changes = sender as FullyObservableCollection<ListItem>;
            if (changes == null || changes.Any() == false)
            {
                return;
            }

            var change = changes[e.CollectionIndex];

            if (ItemsSource.Count == 1)
            {
                // Add
                if (string.IsNullOrWhiteSpace(change.Name) == false)
                {
                    ItemsSource.Add(new ListItem { Index = ItemsSource.Count + 1 });
                }
            }

            if (ItemsSource.Count > 1)
            {
                if (string.IsNullOrWhiteSpace(change.Name))
                {
                    ItemsSource.Remove(change);
                }

                if (ItemsSource.All(x => string.IsNullOrWhiteSpace(x.Name) == false))
                {
                    ItemsSource.Add(new ListItem { Index = ItemsSource.Count + 1 });
                }
            }

            EnsureIndex();
        }

        private void EnsureIndex()
        {
            for (var i = 0; i < ItemsSource.Count; i++)
            {
                ItemsSource[i].Index = i + 1;
            }
        }

        public FullyObservableCollection<ListItem> ItemsSource { get; private set; }
    }

    public class ListItem : INotifyPropertyChanged
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return base.ToString();
            }
            return Name;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
          [CallerMemberName] string propertyName = "",
          Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}