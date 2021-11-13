﻿
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class Paiement
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Date { get; set; }

        private DateTime? dateCreation = null;
        public DateTime DateCreation
        {
            get
            {
                return this.dateCreation.HasValue ? this.dateCreation.Value : DateTime.Now;
            }
            set { this.dateCreation = value; }
        }

        public DateTime? ModificationDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public Guid IdClient { get; set; }

        public Guid IdTypePaiement { get; set; }

        public Guid? IdBonLivraison { get; set; }
        public Guid? IdFacture { get; set; }
        public Guid? IdBonAvoirC { get; set; }

        [DefaultValue(0)]
        public float Debit { get; set; }

        [DefaultValue(0)]
        public float Credit { get; set; }

        public string Comment { get; set; }

        public DateTime? DateEcheance { get; set; }

        public bool? EnCaisse { get; set; }
        public bool? Hide { get; set; }

        public virtual BonLivraison BonLivraison { get; set; }
        public virtual BonAvoirC BonAvoirC { get; set; }
        public virtual Facture Facture { get; set; }

        public virtual Client Client { get; set; }

        public virtual TypePaiement TypePaiement { get; set; }
    }
}
