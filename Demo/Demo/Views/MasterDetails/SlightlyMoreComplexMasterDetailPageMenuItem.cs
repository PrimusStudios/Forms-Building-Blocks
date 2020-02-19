using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Views.MasterDetails
{
    public class SlightlyMoreComplexMasterDetailPageMenuItem
    {
        public SlightlyMoreComplexMasterDetailPageMenuItem()
        {
            TargetType = typeof(SlightlyMoreComplexMasterDetailPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
