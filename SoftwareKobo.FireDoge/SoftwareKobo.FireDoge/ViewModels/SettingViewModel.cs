using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SoftwareKobo.FireDoge.Datas;
using SoftwareKobo.FireDoge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareKobo.FireDoge.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        private IList<BrowserEngine> _allBrowserEngines;

        private BrowserEngine _defaultBrowserEngine;

        private string _indexPage;

        private RelayCommand _saveCommand;

        public SettingViewModel()
        {
            IndexPage = AppSetting.IndexPage;
            DefaultBrowserEngine = AppSetting.DefaultBrowserEngine;
        }

        public IList<BrowserEngine> AllBrowserEngines
        {
            get
            {
                if (_allBrowserEngines == null)
                {
                    _allBrowserEngines = Enum.GetValues(typeof(BrowserEngine)).Cast<BrowserEngine>().ToList();
                }
                return _allBrowserEngines;
            }
        }

        public BrowserEngine DefaultBrowserEngine
        {
            get
            {
                return _defaultBrowserEngine;
            }
            set
            {
                Set(ref _defaultBrowserEngine, value);
            }
        }

        public string IndexPage
        {
            get
            {
                return _indexPage;
            }
            set
            {
                Set(ref _indexPage, value);
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(() =>
                    {
                        AppSetting.IndexPage = IndexPage;
                        AppSetting.DefaultBrowserEngine = DefaultBrowserEngine;
                    });
                }
                return _saveCommand;
            }
        }
    }
}