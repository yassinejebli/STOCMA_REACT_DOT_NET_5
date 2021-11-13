
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class RdbFItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdRdbF { get; set; }

        public float Qte { get; set; }

        public Guid IdArticle { get; set; }

        public virtual RdbF RdbF { get; set; }

        public virtual Article Article { get; set; }
    }
}
