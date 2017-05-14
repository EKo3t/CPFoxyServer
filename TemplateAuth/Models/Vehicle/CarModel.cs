using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TemplateAuth.Models.Vehicle
{
    public class CarModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("CarMark")]
        public int CarMarkId { get; set; }

        public virtual CarMark CarMark { get; set; }
    }
}