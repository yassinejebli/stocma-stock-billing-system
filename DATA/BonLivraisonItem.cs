using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class BonLivraisonItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdBonLivraison { get; set; }

        [Range(0, float.MaxValue)]
        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        [DefaultValue(0)]
        public float PA { get; set; }

        public Guid IdArticle { get; set; }

        public int? Index { get; set; }

        public string NumBC { get; set; }

        public float TotalHT { get; set; }

        public float? Discount { get; set; } = 0;

        public bool PercentageDiscount { get; set; } = false;

        public virtual BonLivraison BonLivraison { get; set; }

        public virtual Article Article { get; set; }
    }
}
