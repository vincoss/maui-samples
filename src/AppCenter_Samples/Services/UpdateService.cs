using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppCenter_ConfigurationSample.Services
{
    public interface IUpdateService
    {
        string Update { get; set; }
    }

    public class UpdateService : IUpdateService, INotifyPropertyChanged
    {
        private string _update;

        public string Update
        { 
            get { return _update; }
            set
            {
                if(_update != value)
                {
                    _update = value;
                    OnPropertyChanged(nameof(Update));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
