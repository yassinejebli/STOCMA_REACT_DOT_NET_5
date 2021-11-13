
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class BonReception
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string NumBon { get; set; }

        public DateTime Date { get; set; }

        public Guid IdFournisseur { get; set; }
        public Guid? IdFactureF { get; set; }
        public int? IdSite { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? ModificationDate { get; set; }
        public virtual Fournisseur Fournisseur { get; set; }
        public virtual FactureF FactureF { get; set; }
        public virtual Site Site { get; set; }

        public virtual ICollection<BonReceptionItem> BonReceptionItems { get; set; }

        public virtual ICollection<BonAvoir> BonAvoirs { get; set; }

        public virtual ICollection<PaiementF> PaiementFs { get; set; }
    }
}
