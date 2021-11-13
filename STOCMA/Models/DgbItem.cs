
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class DgbItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdDgb { get; set; }

        public float Qte { get; set; }

        public float Pu { get; set; }
        public float TotalHT { get; set; }

        public Guid IdArticle { get; set; }

        public virtual Dgb Dgb { get; set; }

        public virtual Article Article { get; set; }
    }
}
