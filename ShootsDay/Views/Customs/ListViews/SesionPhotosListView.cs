using ShootsDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShootsDay.Views.Customs.ListViews
{
    class SesionPhotosListView : ListView
    {
        public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create("Items", typeof(IEnumerable<Photoshoot>), typeof(SesionPhotosListView), new List<Photoshoot>());

        public IEnumerable<Photoshoot> Items
        {
            get { return (IEnumerable<Photoshoot>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        public void NotifyItemSelected(object item)
        {
            if (ItemSelected != null)
            {
                ItemSelected(this, new SelectedItemChangedEventArgs(item));
            }
        }
    }
}
