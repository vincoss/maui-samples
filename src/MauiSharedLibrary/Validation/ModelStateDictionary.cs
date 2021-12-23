using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace MauiSharedLibrary.Validation
{
    public class ModelStateDictionary : IEnumerable<string>
    {
        private readonly Dictionary<string, List<string>> _stateDictionary = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        public ModelStateDictionary()
        {
        }

        public void Add(KeyValuePair<string, List<string>> pair)
        {
            Add(pair.Key, pair.Value ?? new List<string>());
        }

        public void Add(string key, List<string> items)
        {
            if (string.IsNullOrWhiteSpace(nameof(key)))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            foreach (var item in items)
            {
                AddError(key, item);
            }
        }

        public void AddError(string key, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }
            var messages = GetModelStateForKey(key);
            if (messages.Any(x => string.Equals(x, errorMessage, StringComparison.InvariantCultureIgnoreCase)) == false)
            {
                messages.Add(errorMessage);
            }
        }
        public bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            bool result = _stateDictionary.Remove(key);
            return result;
        }

        public void Clear()
        {
            var keys = Keys.ToList();
            foreach (var propertyName in keys)
            {
                Remove(propertyName);
            }
        }

        public string GetValue(string key)
        {
            if(_stateDictionary.ContainsKey(key))
            {
                return _stateDictionary[key].FirstOrDefault();
            }
            return null;
        }

        #region Private methods

        private IEnumerable<string> GetValues()
        {
            return _stateDictionary.Values.SelectMany(x => x).ToList();
        }

        private List<string> GetModelStateForKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            List<string> modelState;
            if (TryGetValue(key, out modelState) == false)
            {
                modelState = new List<string>();
                _stateDictionary[key] = modelState;
            }
            return modelState;
        }
        public bool TryGetValue(string key, out List<string> value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _stateDictionary.TryGetValue(key, out value);
        }

        #endregion

        #region Properties

        public int Count
        {
            get { return _stateDictionary.Count; }
        }

        public bool IsValid
        {
            get { return Values.Any() == false; }
        }

        public ICollection<string> Keys
        {
            get { return _stateDictionary.Keys; }
        }
        public IEnumerable<string> Values
        {
            get { return GetValues(); }
        }

        #endregion

        #region IEnumerable<string>

        public IEnumerator<string> GetEnumerator()
        {
            return GetValues().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        } 

        #endregion
    }
}
