using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TemplateAuth.Models.Vehicle
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Color { get; set; }

        [ForeignKey("CarModel")]
        public int CarModelId { get; set; }

        public virtual CarModel CarModel { get; set; }
    }
}