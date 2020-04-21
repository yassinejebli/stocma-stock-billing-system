using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class DgbF
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Ref { get; set; }

        public string NumBon { get; set; }

        public DateTime Date { get; set; }

        public Guid IdFournisseur { get; set; }

        public string User { get; set; }

        public string CinRcn { get; set; }

        public string DatePaiement { get; set; }

        public string ModeConsignation { get; set; }

        public string TypeReglement { get; set; }//cheque espece

        public string Banque { get; set; }

        public string NumCheque { get; set; }

        public virtual Fournisseur Fournisseur { get; set; }

        public virtual ICollection<DgbFItem> DgbFItems { get; set; }
    }
}
