﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class BonLivraison
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Ref { get; set; }

        public string NumBon { get; set; }

        public DateTime Date { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? ModificationDate { get; set; }
        public Guid IdClient { get; set; }
        public Guid? IdFacture { get; set; }
        public Guid? IdTypePaiement { get; set; }

        public int? IdSite { get; set; }

        [DefaultValue(0)]
        public float? Marge { get; set; }

        public bool WithDiscount { get; set; } = false;

        public string TypeReglement { get; set; }

        public virtual Client Client { get; set; }
        public virtual Facture Facture { get; set; }
        public virtual Site Site { get; set; }
        public virtual TypePaiement TypePaiement { get; set; }

        public virtual float? OldSolde { get; set; }

        public string User { get; set; }

        public string IdUser { get; set; }

        public string Note { get; set; }

        public virtual ICollection<BonLivraisonItem> BonLivraisonItems { get; set; }

        public virtual ICollection<BonAvoirC> BonAvoirCs { get; set; }

        public virtual ICollection<Paiement> Paiements { get; set; }

        public virtual ICollection<Facture> Factures { get; set; }
    }
}
