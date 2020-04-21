// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.PaiementF
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class PaiementF
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();


        public DateTime Date { get; set; }

        public Guid IdFournisseur { get; set; }

        public Guid IdTypePaiement { get; set; }

        public Guid? IdBonReception { get; set; }
        public Guid? IdFactureF { get; set; }

        [DefaultValue(0)]
        public float Debit { get; set; }

        [DefaultValue(0)]
        public float Credit { get; set; }

        public string Comment { get; set; }

        public DateTime? DateEcheance { get; set; }

        public bool? EnCaisse { get; set; }

        public bool? MonCheque { get; set; }

        public virtual BonReception BonReception { get; set; }

        public virtual Fournisseur Fournisseur { get; set; }
        public virtual FactureF FactureF { get; set; }

        public virtual TypePaiement TypePaiement { get; set; }
    }
}
