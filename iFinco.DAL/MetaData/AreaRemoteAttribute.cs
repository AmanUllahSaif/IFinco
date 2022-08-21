using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFinco.DAL
{
    public class AreaRemoteAttribute : RemoteAttribute
    {
        public AreaRemoteAttribute(string action, string controller, string area)
            : base(action, controller, area)
        {
            this.RouteData["area"] = area;
        }
    }


}