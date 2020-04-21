// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.Facture
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

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

        [Required]
        public int Ref { get; set; }

        public string NumBon { get; set; }

        public DateTime Date { get; set; }

        public Guid IdFournisseur { get; set; }

        public string Comment { get; set; }

        public string TypeReglement { get; set; }

        public string User { get; set; }

        public virtual Fournisseur Fournisseur { get; set; }
    
        public virtual ICollection<BonReception> BonReceptions { get; set; }
        public virtual ICollection<PaiementF> PaiementFs { get; set; }
        public virtual ICollection<FactureFItem> FactureFItems { get; set; }
    }
}
