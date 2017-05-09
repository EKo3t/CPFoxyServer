using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateAuth.Models;

namespace TemplateAuth.ClientModels
{
    public class UserInfoCM
    {
        public UserInfoCM(UserInfo usrInfo)
        {
            if (usrInfo != null)
            {
                FirstName = usrInfo.FirstName;
                LastName = usrInfo.LastName;
                MiddleName = usrInfo.MiddleName;
                BirthDate = usrInfo.BirthDate;
            }
        }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get;  set; }

        public DateTime BirthDate { get; set; }
    }
}