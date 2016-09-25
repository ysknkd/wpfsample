using Common;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Artist
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class ArtistsModel : ModelBase
    {
        public ObservableCollection<Artist> Artists { get; set; } = new ObservableCollection<Artist>();

        private string _artistNameA = "";
        public string ArtistNameA
        {
            get { return _artistNameA; }
            set
            {
                _artistNameA = value;
                RemoveError(nameof(ArtistNameA));
            }
        }

        private string _artistNameB = "";
        public string ArtistNameB
        {
            get { return _artistNameB; }
            set
            {
                _artistNameB = value;
                RemoveError(nameof(ArtistNameB));
            }
        }

        private string _artistNameC = "";
        public string ArtistNameC
        {
            get { return _artistNameC; }
            set
            {
                _artistNameC = value;
                RemoveError(nameof(ArtistNameC));
            }
        }

        private bool ValidateProperty()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(ArtistNameA))
            {
                isValid = false;
                AddError(nameof(ArtistNameA), "MSG_DIALOG_REQUIRE");
            }
            if (string.IsNullOrEmpty(ArtistNameB))
            {
                isValid = false;
                AddError(nameof(ArtistNameB), "MSG_DIALOG_REQUIRE");
            }
            if (string.IsNullOrEmpty(ArtistNameC))
            {
                isValid = false;
                AddError(nameof(ArtistNameC), "MSG_DIALOG_REQUIRE");
            }

            return isValid;
        }

        public async Task<TaskResult> Add()
        {
            bool isValid = ValidateProperty();

            if (! isValid)
            {
                return new TaskResult() { result = TaskResultType.EFAILED };
            }

            await Task.Run(() =>
            {
                // 時間のかかる処理を再現
                Thread.Sleep(200);
            });

            return new TaskResult() { result = TaskResultType.SUCCEEDED };
        }

        public ArtistsModel()
        {
            Artists.Add(new Artist() { Id = 1, Name = "Massive Attack" });
            Artists.Add(new Artist() { Id = 2, Name = "Nine Inch Nails" });
            Artists.Add(new Artist() { Id = 3, Name = "Nirvana" });
            Artists.Add(new Artist() { Id = 4, Name = "RADIOHEAD" });
            Artists.Add(new Artist() { Id = 5, Name = "Arctic Monkeys" });
        }
    }
}
