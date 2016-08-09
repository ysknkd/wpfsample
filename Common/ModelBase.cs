using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Common
{
    /// <summary>
    /// Modelの基底クラス
    /// </summary>
    public abstract class ModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region == Implement INotifyPropertyChanged ==
        // INotifyPropertyChanged.PropertyChanged の実装
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, value)) { return false; }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region == Implement INotifyDataErrorInfo ==
        private Dictionary<string, HashSet<string>> _errors = new Dictionary<string, HashSet<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected bool AddError(string propertyName, string error)
        {
            bool result = false;

            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new HashSet<string>();
            }

            result = _errors[propertyName].Add(error);
            if (result)
            {
                //RaiseErrorsChanged(propertyName);
            }

            return result;
        }

        protected bool RemoveError(string propertyName)
        {
            bool result = _errors.Remove(propertyName);
            if (result)
            {
                RaiseErrorsChanged(propertyName);
            }
            return result;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                return _errors[propertyName].ToList().AsReadOnly();
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        public bool HasErrors => _errors.Any();

        protected virtual void RaiseErrorsChanged([CallerMemberName]string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion
    }
}
