using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateAuth.ClientModels
{
    public class UserCreateModel
    {
        public String Email { get; set; }
        public String Password { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}