using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShortMvvm
{
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        public ObservableRangeCollection() { }

        public ObservableRangeCollection(IEnumerable<T> collection) : base(collection) { }

        private bool _suppressNotification = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
            {
                base.OnCollectionChanged(e);
            }
        }

        public void AddRange(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            if (enumerable.Any() == false)
            {
                return;
            }

            int startIndex = Count;
            _suppressNotification = true;

            foreach (T item in enumerable)
            {
                Add(item);
            }
            _suppressNotification = false;

          //  OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(enumerable), startIndex));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        public void ReplaceRange(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            base.ClearItems();
            AddRange(enumerable);
        }

        public void Sort(IComparer<T> comparison)
        {
            var sortableList = new List<T>(this);
            sortableList.Sort(comparison);

            _suppressNotification = true;

            for (int i = 0; i < sortableList.Count; i++)
            {
                this.Move(this.IndexOf(sortableList[i]), i);
            }

            _suppressNotification = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
