using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public static TimeSpan CacheDuration { get; set; } = new TimeSpan(30, 0, 0, 0);

        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }
        public Page Alert { get; set; }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BaseViewModel(Page context = null)
        {
            Navigation = context == null ? (App.Current.MainPage as NavigationPage).Navigation : context.Navigation;
            Alert = context == null ? (App.Current.MainPage as NavigationPage) : context;
        }

        public bool IsBusy { get; set; }
    }
}
