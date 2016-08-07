using Common;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace WpfSample.Models
{
    class ArtistsModel : ModelBase
    {
        public ObservableCollection<Artist> Artists { get; private set; } = new ObservableCollection<Artist>();

        /// <summary>
        /// 選択されたアーティストのID。デフォルトは0番目
        /// </summary>
        public int SelectedArtistId { get; set; } = 0;

        /// <summary>
        /// 追加するアーティストの名前
        /// </summary>
        public string ArtistName { get; set; } = "";

        /// <summary>
        /// アーティストの追加
        /// </summary>
        /// <returns></returns>
        public async Task<TaskResult> Add()
        {
            if (string.IsNullOrEmpty(ArtistName))
            {
                return new TaskResult
                {
                    result = TaskResultType.EREQUIRED, propertyName = nameof(ArtistName)
                };
            }

            await Task.Run(() =>
            {
                // 時間のかかる処理を再現
                Thread.Sleep(200);
            });
            Artists.Add(new Artist { Id = Artists.Count, Name = ArtistName });

            return new TaskResult
            {
                result = TaskResultType.SUCCEEDED
            };
        }

        public ArtistsModel()
        {
            // テストデータの追加
            Artists.Add(new Artist { Id = Artists.Count, Name = "Nine Inch Nails" });
            Artists.Add(new Artist { Id = Artists.Count, Name = "Nirvana" });
            Artists.Add(new Artist { Id = Artists.Count, Name = "Radiohead" });
        }
    }
}
