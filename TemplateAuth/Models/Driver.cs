using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TemplateAuth.Models.Vehicle;

namespace TemplateAuth.Models
{
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Car")]
        public Guid CarId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Car Car { get; set; }
    }
}