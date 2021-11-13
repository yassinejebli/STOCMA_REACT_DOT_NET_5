using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class BonAvoirItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdBonAvoir { get; set; }

        [Range(0, float.MaxValue)]
        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public Guid IdArticle { get; set; }
        public int? IdSite { get; set; }

        public float TotalHT { get; set; }

        public virtual BonAvoir BonAvoir { get; set; }

        public virtual Article Article { get; set; }
        public virtual Site Site { get; set; }
    }
}
