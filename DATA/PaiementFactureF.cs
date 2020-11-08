using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class PaiementFactureF
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Date { get; set; }

        public DateTime DateCreation { get; set; } = DateTime.Now;
        public DateTime? ModificationDate { get; set; }

        public Guid IdFournisseur { get; set; }

        public Guid IdTypePaiement { get; set; }

        public Guid? IdFactureF { get; set; }
        public Guid? IdBonAvoir { get; set; }

        [DefaultValue(0)]
        public float Debit { get; set; }

        [DefaultValue(0)]
        public float Credit { get; set; }

        public string Comment { get; set; }

        public DateTime? DateEcheance { get; set; }

        public bool? EnCaisse { get; set; }

        public bool? MonCheque { get; set; } = false;

        public bool? Hide { get; set; }

        public virtual FactureF FactureF { get; set; }
        public virtual BonAvoir BonAvoir { get; set; }

        public virtual Fournisseur Fournisseur { get; set; }

        public virtual TypePaiement TypePaiement { get; set; }
    }
}
