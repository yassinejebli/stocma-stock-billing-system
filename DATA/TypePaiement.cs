using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class TypePaiement
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsBankRelated { get; set; } = false;
        public bool IsDebit { get; set; } = false;

        public virtual ICollection<Paiement> Paiements { get; set; }
        public virtual ICollection<Devis> Devises { get; set; }
        public virtual ICollection<BonLivraison> BonLivraisons { get; set; }
        public virtual ICollection<Facture> Factures { get; set; }
        public virtual ICollection<FactureF> FactureFs { get; set; }
        public virtual ICollection<FakeFacture> FakeFactures { get; set; }
        public virtual ICollection<FakeFactureF> FakeFactureFs { get; set; }
        public virtual ICollection<PaiementF> PaiementFs { get; set; }
        public virtual ICollection<PaiementFacture> PaiementFactures { get; set; }
        public virtual ICollection<PaiementFactureF> PaiementFactureFs { get; set; }
    }
}
