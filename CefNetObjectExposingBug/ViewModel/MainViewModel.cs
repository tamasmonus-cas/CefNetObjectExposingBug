using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CefNetObjectExposingBug.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Tabs

        private ObservableCollection<Tab> _tabs;

        public ObservableCollection<Tab> Tabs
        {
            get { return _tabs; }
            set { Set(ref _tabs, value); }
        }

        #endregion

        #region SelectedTab

        private Tab _selectedTab;

        public Tab SelectedTab
        {
            get { return _selectedTab; }
            set { Set(ref _selectedTab, value); }
        }

        #endregion

        public RelayCommand AddTab
        {
            get {
                return new RelayCommand(() =>
                {
                    Tabs.Add(new Tab {Name = "Tab " + _tabs.Count});
                    SelectedTab = Tabs.Last();
                });
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _tabs = new ObservableCollection<Tab>
            {
                new Tab() { Name = "Tab 0"}
            };

            _selectedTab = _tabs[0];

            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
            }
        }
    }
}