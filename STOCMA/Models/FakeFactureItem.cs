
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class FakeFactureItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdFakeFacture { get; set; }

        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public float? Discount { get; set; } = 0;

        public bool PercentageDiscount { get; set; } = false;

        public Guid IdArticleFacture { get; set; }

        public float TotalHT { get; set; }

        public virtual FakeFacture FakeFacture { get; set; }

        public virtual ArticleFacture ArticleFacture { get; set; }
    }
}
