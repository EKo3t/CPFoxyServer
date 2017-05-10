using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateAuth.ClientModels
{
    public class OrderCM
    {
        public DateTime OrderTime { get; set; }

        public string StartAddress { get; set; }

        public string EndAddress { get; set; }
    }
}