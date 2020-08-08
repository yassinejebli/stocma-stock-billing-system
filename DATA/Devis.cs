using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class Devis
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Ref { get; set; }

        public string NumBon { get; set; }

        public int? IdSite { get; set; }


        //SUIV
        public string TypeReglement { get; set; }// espece, cheque, effet, au-comptant
        public string DelaiLivrasion { get; set; }
        public string TransportExpedition { get; set; }
        public string ValiditeOffre { get; set; }
        public string Note { get; set; }
        public int Validity { get; set; }
        public bool TransportFees { get; set; } = false;
        public int DeliveryTime { get; set; }
        //

        public DateTime Date { get; set; }

        public Guid IdClient { get; set; }
        public Guid? IdTypePaiement { get; set; }

        public string ClientName { get; set; }
        public string User { get; set; }

        public bool WithDiscount { get; set; } = false;
        public float Discount { get; set; } = 0;

        public bool PercentageDiscount { get; set; } = false;

        public virtual Client Client { get; set; }
        public virtual Site Site { get; set; }
        public virtual TypePaiement TypePaiement { get; set; }


        public virtual ICollection<DevisItem> DevisItems { get; set; }
    }
}
