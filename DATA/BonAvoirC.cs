using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class BonAvoirC
    {
        [Key]
        public Guid Id { get; set; }

        public string NumBon { get; set; }

        [Required]
        public int Ref { get; set; }

        public DateTime Date { get; set; }

        public Guid IdClient { get; set; }
        public int? IdSite { get; set; }

        public string User { get; set; }
        public string Note { get; set; }

        public Guid? IdBonLivraison { get; set; }

        [DefaultValue(0)]
        public float? Marge { get; set; }

        public virtual Client Client { get; set; }

        public virtual BonLivraison BonLivraison { get; set; }

        public virtual Site Site { get; set; }

        public virtual ICollection<BonAvoirCItem> BonAvoirCItems { get; set; }
        public virtual ICollection<Paiement> Paiements { get; set; }
    }
}
