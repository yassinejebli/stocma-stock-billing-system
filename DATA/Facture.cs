using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class Facture
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Ref { get; set; }
        public int? IdSite { get; set; }

        public string NumBon { get; set; }

        public DateTime Date { get; set; }
        public DateTime? DateEcheance { get; set; }

        public Guid IdClient { get; set; }
        public Guid? IdTypePaiement { get; set; }

        public Guid? IdBonLivraison { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? ModificationDate { get; set; }
        public bool WithDiscount { get; set; } = false;

        public string Comment { get; set; }

        public string ClientName { get; set; }
        public string ClientICE { get; set; }


        public string TypeReglement { get; set; }

        public string User { get; set; }

        public string Note { get; set; }

        public virtual Client Client { get; set; }

        public virtual Site Site { get; set; }
        public virtual TypePaiement TypePaiement { get; set; }


        public virtual BonLivraison BonLivraison { get; set; }
        public virtual ICollection<Paiement> Paiements { get; set; }
        public virtual ICollection<PaiementFacture> PaiementFactures { get; set; }

        public virtual ICollection<FactureItem> FactureItems { get; set; }
        public virtual ICollection<BonLivraison> BonLivraisons { get; set; }
    }
}
