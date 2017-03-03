using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashGGApiWrapper
{
    class RootObject
    {
        public Entities entities { get; set; }
        public int result { get; set; }
        public string resultEntity { get; set; }
        public List<object> actionRecords { get; set; }
    }
}
