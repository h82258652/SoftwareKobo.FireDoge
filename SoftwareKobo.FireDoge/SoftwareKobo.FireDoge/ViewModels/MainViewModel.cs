using GalaSoft.MvvmLight;
using SoftwareKobo.FireDoge.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SoftwareKobo.FireDoge.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TabPage> _pages = new ObservableCollection<TabPage>();

        private Func<object> _newTabPage;

        public MainViewModel()
        {
        }

        public Func<object> NewTabPage
        {
            get
            {
                if (_newTabPage == null)
                {
                    _newTabPage = () => new TabPage();
                }
                return _newTabPage;
            }
        }

        public ObservableCollection<TabPage> Pages
        {
            get
            {
                return _pages;
            }
        }

        private TabPage _currentPage;

        public TabPage CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                Set(ref _currentPage, value);
            }
        }
    }
}