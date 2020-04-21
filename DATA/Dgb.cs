using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class Dgb
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Ref { get; set; }

        public string NumBon { get; set; }

        public DateTime Date { get; set; }

        public Guid IdClient { get; set; }

        public string User { get; set; }

        public string CinRcn { get; set; }

        public string DatePaiement { get; set; }

        public string ModeConsignation { get; set; }

        public string TypeReglement { get; set; }//cheque espece

        public string Banque { get; set; }

        public string NumCheque { get; set; }

        public virtual Client Client { get; set; }

        public virtual ICollection<DgbItem> DgbItems { get; set; }
    }
}
