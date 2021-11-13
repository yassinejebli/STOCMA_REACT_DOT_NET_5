
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STOCMA.Models
{
    public class Categorie
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public Guid? IdFamille { get; set; }
        public virtual Famille Famille { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<InventaireItem> InventaireItems { get; set; }

    }
}
