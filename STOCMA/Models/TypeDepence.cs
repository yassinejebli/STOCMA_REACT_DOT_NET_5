
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class TypeDepence
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Depence> Depences { get; set; }
    }
}
