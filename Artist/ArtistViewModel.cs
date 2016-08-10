using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Artist
{
    class ArtistViewModel : ViewModelBase
    {
        private ArtistsModel _artists = new ArtistsModel();

        /// <summary>
        /// アーティスト名の一覧
        /// </summary>
        public ObservableCollection<Artist> Artists => _artists.Artists;

        /// <summary>
        /// 選択されたアーティストのID。デフォルトは0番目
        /// </summary>
        public int SelectedArtistId {
            get { return _artists.SelectedArtistId; }
            set { _artists.SelectedArtistId = value; }
        }

        /// <summary>
        /// 追加するアーティストの名前
        /// </summary>
        public string ArtistName {
            get { return _artists.ArtistName; }
            set { _artists.ArtistName = value; }
        }

        /// <summary>
        /// アーティスト追加コマンド
        /// </summary>
        public ICommand AddArtistCommand { get; private set; }

        private bool CanExecuteAddArtist(object state)
        {
            return true;
        }

        private async void ExecuteAddArtist(object state)
        {
            TaskResult result = await _artists.Add();

            switch (result.result)
            {
                case TaskResultType.SUCCEEDED:
                    MessageBox.Show(ArtistName + Resources("MSG_DIALOG_ADDED"), Resources("MSG_DIALOG_TITLE_CONFIRM"));
                    break;
                case TaskResultType.EREQUIRED:
                    if (result.propertyName == nameof(ArtistName))
                    {
                        MessageBox.Show(Resources("MSG_DIALOG_REQUIRE"), Resources("MSG_DIALOG_TITLE_ERROR"));
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ArtistViewModel()
        {
            Model = _artists;
            AddArtistCommand = CreateCommand(ExecuteAddArtist, CanExecuteAddArtist);

            _artists.PropertyChanged += (sender, e) =>
            {
                RaisePropertyChanged(e.PropertyName);
            };
        }
    }
}
