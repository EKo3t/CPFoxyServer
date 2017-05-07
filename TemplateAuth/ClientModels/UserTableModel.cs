using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateAuth.ClientModels
{
    public class UserTableModel
    {
        public string Email { get; set; }
               
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}