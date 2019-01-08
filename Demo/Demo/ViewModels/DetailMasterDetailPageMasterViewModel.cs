using System.Collections.ObjectModel;
using Demo.Views.MasterDetails;
using Forms.BuildingBlocks.ViewModels;

namespace Demo.ViewModels
{
    public class DetailMasterDetailPageMasterViewModel : BindingBase
    {
        public ObservableCollection<DetailMasterDetailPageMenuItem> MenuItems { get; set; }

        public DetailMasterDetailPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<DetailMasterDetailPageMenuItem>(new[]
            {
                new DetailMasterDetailPageMenuItem { Id = 0, Title = "Page 1" },
                new DetailMasterDetailPageMenuItem { Id = 1, Title = "Page 2" },
                new DetailMasterDetailPageMenuItem { Id = 2, Title = "Page 3" },
                new DetailMasterDetailPageMenuItem { Id = 3, Title = "Page 4" },
                new DetailMasterDetailPageMenuItem { Id = 4, Title = "Page 5" },
            });
        }
    }
}