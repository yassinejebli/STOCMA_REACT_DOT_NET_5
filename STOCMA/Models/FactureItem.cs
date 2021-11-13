
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class FactureItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdFacture { get; set; }

        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public Guid IdArticle { get; set; }

        public float TotalHT { get; set; }

        public string NumBC { get; set; }

        public float? Discount { get; set; } = 0;

        public bool PercentageDiscount { get; set; } = false;
        public string NumBL { get; set; }
        public string Description { get; set; }

        public virtual Facture Facture { get; set; }

        public virtual Article Article { get; set; }
    }
}
