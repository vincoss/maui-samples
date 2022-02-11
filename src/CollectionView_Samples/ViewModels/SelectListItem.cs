using MauiSharedLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CollectionView_Samples.ViewModels
{
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
