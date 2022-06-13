using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models.ViewModel
{
    public class IndexVm
    {
        public IEnumerable<NationalPark> NationalParkList { get; set; }

        public IEnumerable<Trail> TrailList { get; set; }
    }
}
