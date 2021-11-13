using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class TypeDepense
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        //public float MontantParDefaut { get; set; }

        public virtual ICollection<DepenseItem> DepenseItems { get; set; }
    }
}
