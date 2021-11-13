
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class BonCommande
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Ref { get; set; }

        public string NumBon { get; set; }

        public DateTime Date { get; set; }

        public Guid IdFournisseur { get; set; }
        public string Note { get; set; }

        public string User { get; set; }

        public virtual Fournisseur Fournisseur { get; set; }

        public virtual ICollection<BonCommandeItem> BonCommandeItems { get; set; }
    }
}
