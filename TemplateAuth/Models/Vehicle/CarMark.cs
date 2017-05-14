using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TemplateAuth.Models.Vehicle
{
    public class CarMark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Mark { get; set; }

        public virtual ICollection<CarModel> CarModel { get; set; }
    }
}