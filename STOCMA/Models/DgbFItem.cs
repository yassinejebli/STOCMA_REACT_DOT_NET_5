
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class DgbFItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdDgbF { get; set; }

        public float Qte { get; set; }

        public float Pu { get; set; }
        public float TotalHT { get; set; }

        public Guid IdArticle { get; set; }

        public virtual DgbF DgbF { get; set; }

        public virtual Article Article { get; set; }
    }
}
