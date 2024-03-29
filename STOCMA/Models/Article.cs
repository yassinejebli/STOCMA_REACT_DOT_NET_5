﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace STOCMA.Models
{
    public class Article
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(200)]
        public string Ref { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RefAuto { get; set; }

        [Required]
        [StringLength(200)]
        public string Designation { get; set; }

        public string Description { get; set; }

        public string Marque { get; set; }

        [DefaultValue(1)]
        public float? MinStock { get; set; } = 1;

        [DefaultValue(0)]
        public float? MaxStock { get; set; }

        //transferer
        [DefaultValue(0)]
        public float QteStock { get; set; }



        [NotMapped]
        public float QteStockSum
        {
            get
            {
                return this.ArticleSites != null ? this.ArticleSites.Sum(x => x.QteStock) : 0;
            }
        }

        [DefaultValue(0)]
        public float? QteEmballageVide { get; set; }//for suiv qtevide

        [DefaultValue(0)]
        public float? QteEmballagePleine { get; set; }//for suiv QtePleine

        [DefaultValue("U")]
        public string Unite { get; set; } = "U";

        public bool IsStocked { get; set; } = false;

        public bool IsBarCodePrintable { get; set; } = true;

        [DefaultValue(20)]
        public float? TVA { get; set; } = 20;

        [DefaultValue(0)]
        public float PA { get; set; }

        [DefaultValue(0)]
        public float? PVD { get; set; }

        [DefaultValue(0)]
        public float? PVSG { get; set; }

        [DefaultValue(0)]
        public float? PVG { get; set; }

        public Guid? IdCategorie { get; set; }

        public string Logo { get; set; }

        public string Logo2 { get; set; }

        public string Logo3 { get; set; }
        public string Image { get; set; }

        [MaxLength(18)]
        public string BarCode { get; set; }

        public DateTime? DateModification { get; set; }

        public virtual Categorie Categorie { get; set; }

        public virtual ICollection<BonReceptionItem> BonReceptionItems { get; set; }
        public virtual ICollection<ArticleSite> ArticleSites { get; set; }

        public virtual ICollection<BonLivraisonItem> BonLivraisonItems { get; set; }

        public virtual ICollection<DevisItem> DevisItems { get; set; }

        public virtual ICollection<BonAvoirItem> BonAvoirItems { get; set; }

        public virtual ICollection<BonAvoirCItem> BonAvoirCItems { get; set; }

        public virtual ICollection<BonCommandeItem> BonCommandeItems { get; set; }

        public virtual ICollection<FactureItem> FactureItems { get; set; }

        public virtual ICollection<TarifItem> TarifItems { get; set; }
        public virtual ICollection<RdbItem> RdbItems { get; set; }
        public virtual ICollection<DgbItem> DgbItems { get; set; }
        public virtual ICollection<DgbFItem> DgbFItems { get; set; }
        public virtual ICollection<RdbFItem> RdbFItems { get; set; }
        public virtual ICollection<FactureFItem> FactureFItems { get; set; }
        public virtual ICollection<StockMouvementItem> StockMouvementItems { get; set; }
        public virtual ICollection<InventaireItem> InventaireItems { get; set; }
    }
}
