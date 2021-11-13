
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class BonAvoir
    {
        [Key]
        public Guid Id { get; set; }
        public int? IdSite { get; set; }

        public string NumBon { get; set; }

        public int Ref { get; set; }

        public DateTime Date { get; set; }

        public Guid IdFournisseur { get; set; }

        public Guid? IdBonReception { get; set; }

        public virtual Fournisseur Fournisseur { get; set; }

        public virtual BonReception BonReception { get; set; }
        public virtual Site Site { get; set; }

        public virtual ICollection<BonAvoirItem> BonAvoirItems { get; set; }
        public virtual ICollection<PaiementFactureF> PaiementFactureFs { get; set; }
        public virtual ICollection<PaiementF> PaiementFs { get; set; }
    }
}
