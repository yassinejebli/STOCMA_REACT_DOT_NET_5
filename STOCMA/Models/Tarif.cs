
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class Tarif
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Ref { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<TarifItem> TarifItems { get; set; }
    }
}
