﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TemplateAuth.Models
{
    public class UserToInfo
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}