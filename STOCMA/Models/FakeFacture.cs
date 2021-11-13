
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class FakeFacture
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Ref { get; set; }

        public string NumBon { get; set; }

        public string Note { get; set; }

        public DateTime Date { get; set; }
        public DateTime? DateEcheance { get; set; }
        public Guid? IdTypePaiement { get; set; }

        public bool WithDiscount { get; set; } = false;

        public string ClientName { get; set; }
        public string ClientICE { get; set; }

        public Guid IdClient { get; set; }

        public string Comment { get; set; }

        public string User { get; set; }

        public virtual Client Client { get; set; }
        public virtual TypePaiement TypePaiement { get; set; }

        public virtual ICollection<FakeFactureItem> FakeFactureItems { get; set; }
    }
}
