using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.DATA
{
    public class Site
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Name { get; set; }
        public string Code { get; set; }

        public string Address { get; set; }

        public bool Disabled { get; set; } = false;

        public virtual ICollection<ArticleSite> ArticleSites { get; set; }
        public virtual ICollection<BonLivraison> BonLivraisons { get; set; }
        public virtual ICollection<BonLivraisonItem> BonLivraisonItems { get; set; }
        public virtual ICollection<BonAvoirCItem> BonAvoirCItems { get; set; }
        public virtual ICollection<BonAvoirItem> BonAvoirItems { get; set; }
        public virtual ICollection<BonReception> BonReceptions { get; set; }
        public virtual ICollection<Devis> Devises { get; set; }
        public virtual ICollection<Facture> Factures { get; set; }
        public virtual ICollection<BonAvoirC> BonAvoirCs { get; set; }
        public virtual ICollection<BonAvoir> BonAvoirs { get; set; }
        public virtual ICollection<StockMouvement> StockMouvementFroms { get; set; }
        public virtual ICollection<StockMouvement> StockMouvementTos { get; set; }


    }
}
