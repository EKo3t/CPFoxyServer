using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TemplateAuth.Models
{
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public DateTime BirthDate { get; set; }
    }
}