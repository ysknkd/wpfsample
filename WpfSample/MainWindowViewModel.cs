using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfSample
{
    public class TabItem
    {
        public string TabName { get; set; }
        public UIElement TabContent { get; set; }
    }

    public class MainWindowViewModel : ViewModelBase
    {
        private MainWindow _mainWindow;

        private ObservableCollection<TabItem> _tabItems = new ObservableCollection<TabItem>();
        public ObservableCollection<TabItem> TabItems
        {
            get { return _tabItems; }
            set {
                _tabItems = value;
                RaisePropertyChanged();
            }
        }

        public ICommand AddTabCommand { get; set; }

        public MainWindowViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            Model = new MainWindowModel();

            AddTabCommand = CreateCommand(ExecuteAddTab, CanExecuteAddTab);
        }

        private bool CanExecuteAddTab(object state)
        {
            return true;
        }

        private void ExecuteAddTab(object state)
        {
            TabItem item = new TabItem();
            item.TabName = "Artist";
            item.TabContent = new Artist.ArtistView();

            TabItems.Add(item);
        }
    }
}
