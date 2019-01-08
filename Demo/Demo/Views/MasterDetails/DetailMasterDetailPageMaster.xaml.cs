using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Demo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Demo.Views.MasterDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public DetailMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new DetailMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

       
    }
}