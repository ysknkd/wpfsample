using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Common
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        // @see http://qiita.com/hugo-sb/items/f07ef53dea817d198475

        #region == implement of INotifyPropertyChanged ==
        // INotifyPropertyChanged.PropertyChanged の実装
        public event PropertyChangedEventHandler PropertyChanged;

        // INotifyPropertyChanged.PropertyChanged イベントの発行
        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region == implement of IDataErrorInfo ==
        // IDataErrorInfo 用のエラーメッセージを保持する辞書
        private Dictionary<string, string> _ErrorMessages = new Dictionary<string, string>();

        // IDataErrorInfo.Error の実装
        string IDataErrorInfo.Error
        {
            get { return (_ErrorMessages.Count > 0) ? "Has Error" : null; }
        }

        // IDataErrorInfo.Item の実装
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (_ErrorMessages.ContainsKey(columnName))
                {
                    return _ErrorMessages[columnName];
                }
                else
                {
                    return null;
                }
            }
        }

        // エラーメッセージのセット
        protected void SetError(string propertyName, string errorMessage)
        {
            _ErrorMessages[propertyName] = errorMessage;
        }

        // エラーメッセージのクリア
        protected void ClearError(string propertyName)
        {
            if (_ErrorMessages.ContainsKey(propertyName))
            {
                _ErrorMessages.Remove(propertyName);
            }
        }
        #endregion

        #region == implement of ICommand Helper ==

        #region ** Class : _DelegateCommand
        // ICommand 実装用のヘルパークラス
        private class _DelegateCommand : ICommand
        {
            private Action<object> _Command;
            private Func<object, bool> _CanExecute;

            public _DelegateCommand(Action<object> command, Func<object, bool> canExecute)
            {
                if (command == null)
                {
                    throw new ArgumentNullException();
                }

                _Command = command;
                _CanExecute = canExecute;
            }

            void ICommand.Execute(object parameter)
            {
                _Command(parameter);
            }

            bool ICommand.CanExecute(object parameter)
            {
                if (_CanExecute != null)
                {
                    return _CanExecute(parameter);
                }
                else
                {
                    return true;
                }
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
        #endregion

        protected ICommand CreateCommand(Action<object> command, Func<object, bool> canExecute)
        {
            return new _DelegateCommand(command, canExecute);
        }
        #endregion

        #region == implement for Resources ==
        public Func<string, object> FindResources;

        public string Resources(string key)
        {
            return (string)FindResources(key);
        }
        #endregion
    }
}
