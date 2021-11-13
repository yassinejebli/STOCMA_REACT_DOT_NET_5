using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace STOCMA.Models
{
    public class Fournisseur
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Adresse { get; set; }

        public string ICE { get; set; }
        public bool Disabled { get; set; } = false;

        [NotMapped]
        public float Solde
        {
            get { return (PaiementFs != null) ? PaiementFs.Sum(x => x.Debit - x.Credit) : 0; }
        }

        //TODO
        [NotMapped]
        public float SoldeFacture
        {
            get { return (PaiementFactureFs != null) ? PaiementFactureFs.Sum(x => x.Debit - x.Credit) : 0; }
        }
        public virtual ICollection<BonReception> BonReceptions { get; set; }

        public virtual ICollection<BonAvoir> BonAvoirs { get; set; }

        public virtual ICollection<BonCommande> BonCommandes { get; set; }

        public virtual ICollection<PaiementF> PaiementFs { get; set; }
        public virtual ICollection<PaiementFactureF> PaiementFactureFs { get; set; }
        public virtual ICollection<DgbF> DgbFs { get; set; }
        public virtual ICollection<RdbF> RdbFs { get; set; }
        public virtual ICollection<FactureF> FactureFs { get; set; }
        public virtual ICollection<FakeFactureF> FakeFactureFs { get; set; }
    }
}
