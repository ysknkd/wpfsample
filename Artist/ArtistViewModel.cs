using C1.WPF.FlexGrid;
using Common;
using Common.Messenger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Artist
{
    class ArtistViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private ArtistsModel _artists = new ArtistsModel();

        public string ArtistNameA
        {
            get { return _artists.ArtistNameA; }
            set { _artists.ArtistNameA = value; }
        }
        public string ArtistNameB
        {
            get { return _artists.ArtistNameB; }
            set { _artists.ArtistNameB = value; }
        }
        public string ArtistNameC
        {
            get { return _artists.ArtistNameC; }
            set { _artists.ArtistNameC = value; }
        }
        public ObservableCollection<Artist> Artists
        {
            get { return _artists.Artists; }
            set { _artists.Artists = value; }
        }

        private Messenger _errorMessenger = new Messenger();
        public Messenger ErrorMessenger
        {
            get
            {
                return _errorMessenger;
            }
        }

        public ICommand AddArtistCommand { get; private set; }
        public ICommand FlexGridReloadedRows { get; private set; }

        private bool CanExecuteAddArtist(object state)
        {
            return true;
        }

        private async void ExecuteAddArtist(object state)
        {
            TaskResult result = await _artists.Add();

            if (result.result == TaskResultType.EFAILED)
            {
                MessageBox.Show(Resources("MSG_DIALOG_REQUIRE"), Resources("MSG_DIALOG_TITLE_ERROR"));
                ErrorMessenger.Raise(new Message("エラーメッセージ"), (m) => { });
            }
        }

        private bool CanExecuteFlexGridReloadedRows(object flexgrid)
        {
            return true;
        }

        private void ExecuteFlexGridReloadedRows(object flexGridRows)
        {
            RowCollection rows = flexGridRows as RowCollection;
            List<Artist> list;

            list = rows.Select(r => (Artist)r.DataItem).ToList();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ArtistViewModel()
        {
            Model = _artists;
            AddArtistCommand = CreateCommand(ExecuteAddArtist, CanExecuteAddArtist);
            FlexGridReloadedRows = CreateCommand(ExecuteFlexGridReloadedRows, CanExecuteFlexGridReloadedRows);

            _artists.PropertyChanged += (sender, e) =>
            {
                RaisePropertyChanged(e.PropertyName);
            };

            _artists.ErrorsChanged += (sender, e) =>
            {
                ErrorsChanged?.Invoke(this, e);
            };
        }

        #region == implement of INotifyDataErrorInfo ==
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            // Model で格納しているメッセージはあくまでもリソースのIDなので、変換する必要がある。
            IEnumerable errors = ((INotifyDataErrorInfo)Model).GetErrors(propertyName);
            HashSet<string> errorsForView = new HashSet<string>();

            foreach(var MSG_ID in errors)
            {
                errorsForView.Add(Resources((string)MSG_ID));
            }

            return errorsForView.ToList().AsReadOnly();
        }

        public bool HasErrors
        {
            get
            {
                return ((INotifyDataErrorInfo)Model).HasErrors;
            }
        }
        #endregion
    }
}
