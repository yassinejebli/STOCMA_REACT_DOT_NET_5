
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class BonReceptionItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdBonReception { get; set; }

        [Range(0, float.MaxValue)]
        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public Guid IdArticle { get; set; }

        public int? Index { get; set; }

        public float TotalHT { get; set; }

        public float? TotalTTC { get; set; }

        public virtual BonReception BonReception { get; set; }

        public virtual Article Article { get; set; }
    }
}
