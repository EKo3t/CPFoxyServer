using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TemplateAuth.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime orderTime { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual Status Status { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}