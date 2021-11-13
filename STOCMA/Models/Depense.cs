using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class Depense
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string Titre { get; set; } = "";

        public virtual ICollection<DepenseItem> DepenseItems { get; set; }

    }
}
