using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Demo.Views.MasterDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public SimpleMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new SimpleMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class SimpleMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<SimpleMasterDetailPageMenuItem> MenuItems { get; set; }
            
            public SimpleMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<SimpleMasterDetailPageMenuItem>(new[]
                {
                    new SimpleMasterDetailPageMenuItem { Id = 0, Title = "Page 1" },
                    new SimpleMasterDetailPageMenuItem { Id = 1, Title = "Page 2" },
                    new SimpleMasterDetailPageMenuItem { Id = 2, Title = "Page 3" },
                    new SimpleMasterDetailPageMenuItem { Id = 3, Title = "Page 4" },
                    new SimpleMasterDetailPageMenuItem { Id = 4, Title = "Page 5" },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}