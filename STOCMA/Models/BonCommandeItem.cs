
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class BonCommandeItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdBonCommande { get; set; }

        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public Guid IdArticle { get; set; }

        public float TotalHT { get; set; }

        public virtual BonCommande BonCommande { get; set; }

        public virtual Article Article { get; set; }
    }
}
