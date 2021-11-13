
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class TarifItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdTarif { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        [DefaultValue(0)]
        public float Pu2 { get; set; }

        public Guid IdArticle { get; set; }

        public virtual Tarif Tarif { get; set; }

        public virtual Article Article { get; set; }
    }
}
