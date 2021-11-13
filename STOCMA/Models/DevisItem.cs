using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class DevisItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdDevis { get; set; }

        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public Guid IdArticle { get; set; }

        public float TotalHT { get; set; }

        public float? Discount { get; set; } = 0;

        public bool PercentageDiscount { get; set; } = false;

        public virtual Devis Devis { get; set; }

        public virtual Article Article { get; set; }
    }
}
