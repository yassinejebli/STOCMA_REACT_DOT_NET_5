
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class RdbItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdRdb { get; set; }

        public float Qte { get; set; }

        public Guid IdArticle { get; set; }

        public virtual Rdb Rdb { get; set; }

        public virtual Article Article { get; set; }
    }
}
