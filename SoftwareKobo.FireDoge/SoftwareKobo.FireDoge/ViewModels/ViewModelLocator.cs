using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;

namespace SoftwareKobo.FireDoge.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            // Set IoC conatiner.
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register Services, but in the app, there are no service, 233.

            // Register ViewModel.
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                // 每次都产生一个独立的 VM。
                return ServiceLocator.Current.GetInstance<MainViewModel>(Guid.NewGuid().ToString());
            }
        }

        public SettingViewModel Setting
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingViewModel>(Guid.NewGuid().ToString());
            }
        }
    }
}