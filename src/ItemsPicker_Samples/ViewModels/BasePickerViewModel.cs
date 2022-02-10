using MauiSharedLibrary.Dto;
using MauiSharedLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ItemsPicker_Samples.ViewModels
{
    public abstract class BasePickerViewModel : BaseViewModel
    {
        private string _search;

        public BasePickerViewModel()
        {
            CancelCommand = new Command(OnCancelCommand);
            OkCommand = new Command(OnOkCommand, OnCanOkCommand);
            ItemTapCommand = new Command<SelectListItem>(OnItemTapCommand);

            IsSingleSelection = true;
            ItemsSource = new ObservableCollection<SelectListItem>();
        }

        #region Command methods

        private async void OnCancelCommand()
        {
            throw new NotImplementedException();
         //   await App..ShellPushAsync("..");//.PopModalAsync();
        }

        protected bool OnCanOkCommand()
        {
            // Message center raise
            // navigate back
            return true;
        }

        protected abstract void OnOkCommand();

        private void OnRefreshCommand()
        {
            Initialize();
        }

        private void OnSearchCommand(string search)
        {
            _search = search;
            Initialize();
        }

        private void OnItemTapCommand(SelectListItem dto)
        {
            var flag = dto.IsSelected;
            if (IsSingleSelection)
            {
                ClearSelection();
            }
            dto.IsSelected = !flag;
            SetSelection(MapSelected(ItemsSource));
        }
        private void ClearSelection()
        {
            foreach (var item in ItemsSource)
            {
                item.IsSelected = false;
            }
        }

        protected void Select(IEnumerable<KeyDataIntString> items)
        {
            foreach (var item in ItemsSource)
            {
                item.IsSelected = items.Any(x => x.Key == item.Key);
            }
        }

        private IEnumerable<KeyDataIntString> MapSelected(IEnumerable<SelectListItem> items)
        {
            return (from x in items
                    where x.IsSelected
                    select new KeyDataIntString
                    {
                        Key = x.Key,
                        Value = x.Value
                    }).ToArray();
        }

        private IEnumerable<SelectListItem> MapIncoming(IEnumerable<KeyDataIntString> items, bool check)
        {
            return (from x in items
                    select new SelectListItem
                    {
                        Key = x.Key,
                        Value = x.Value,
                        IsSelected = check,
                    }).ToArray();
        }

        #endregion

        #region Commands

        public ICommand CancelCommand { get; private set; }
        public ICommand OkCommand { get; private set; }

        public ICommand SearchCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand ItemTapCommand { get; private set; }

        #endregion

        private bool _isSingleSelection;

        public bool IsSingleSelection
        {
            get { return _isSingleSelection; }
            set { SetProperty(ref _isSingleSelection, value); }
        }

        public ObservableCollection<SelectListItem> ItemsSource { get; private set; }
    }

    public class SelectListItem : BasePropertyChanged
    {
        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private int _key;
        public int Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }
    }
}
