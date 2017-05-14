using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateAuth.ClientModels
{
    public class CarCM
    {
        public Guid Id { get; set; }
        public string CarMark { get; set; }
        public string CarModel { get; set; }
        public string CarColor { get; set; }
    }
}