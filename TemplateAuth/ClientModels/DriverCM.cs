using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateAuth.ClientModels
{
    public class DriverCM
    {
        public UserInfoCM UserDetails { get; set; }

        public CarCM Car { get; set; }
    }
}