using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class BranchAccessVM
    {
        public long UID { get; set; }
        public List<long> Branches { get; set; }
    }
}