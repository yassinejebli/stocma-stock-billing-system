using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace WebApplication1.DATA
{
    public class FactureF
    {
        [Key]
        public Guid Id { get; set; }

        public int? Ref { get; set; }

        public string NumBon { get; set; }

        public DateTime Date { get; set; }
        public Guid? IdTypePaiement { get; set; }

        public Guid IdFournisseur { get; set; }

        public string Comment { get; set; }

        public string TypeReglement { get; set; }

        public string User { get; set; }

        public virtual Fournisseur Fournisseur { get; set; }
    
        public virtual ICollection<BonReception> BonReceptions { get; set; }
        public virtual TypePaiement TypePaiement { get; set; }

        public virtual ICollection<PaiementF> PaiementFs { get; set; }
        public virtual ICollection<PaiementFactureF> PaiementFactureFs { get; set; }
        public virtual ICollection<FactureFItem> FactureFItems { get; set; }

    }
}
