using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace STOCMA.Models
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();


        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        public string CodeClient { get; set; } = "";

        public string Tel { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        [DefaultValue(0)]
        public float Plafond { get; set; }

        public string Adresse { get; set; }
        public string ICE { get; set; }

        [NotMapped]
        public float Solde
        {
            get { return (Paiements != null && !IsClientDivers) ? Paiements.Sum(x => x.Debit - x.Credit) : 0; }
        }

        [NotMapped]
        public float SoldeFacture
        {
            get { return (PaiementFactures != null) ? PaiementFactures.Sum(x => x.Debit - x.Credit) : 0; }
        }

        public bool Disabled { get; set; } = false;
        public bool IsClientDivers { get; set; } = false;

        public DateTime? DateCreation { get; set; }

        public Guid? IdRevendeur { get; set; }

        public Revendeur Revendeur { get; set; }

        public virtual ICollection<BonLivraison> BonLivraisons { get; set; }

        public virtual ICollection<Devis> Devises { get; set; }

        public virtual ICollection<BonAvoirC> BonAvoirCs { get; set; }

        public virtual ICollection<Paiement> Paiements { get; set; }
        public virtual ICollection<PaiementFacture> PaiementFactures { get; set; }

        public virtual ICollection<Facture> Factures { get; set; }

        public virtual ICollection<FakeFacture> FakeFactures { get; set; }

        public virtual ICollection<Rdb> Rdbs { get; set; }

        public virtual ICollection<Dgb> Dgbs { get; set; }

    }
}
