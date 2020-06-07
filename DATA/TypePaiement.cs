﻿// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.TypePaiement
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

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

        public virtual ICollection<Paiement> Paiements { get; set; }
        public virtual ICollection<Devis> Devises { get; set; }
        public virtual ICollection<BonLivraison> BonLivraisons { get; set; }
        public virtual ICollection<Facture> Factures { get; set; }
        public virtual ICollection<FakeFacture> FakeFactures { get; set; }
        public virtual ICollection<FakeFactureF> FakeFactureFs { get; set; }
        public virtual ICollection<PaiementF> PaiementFs { get; set; }
        public virtual ICollection<PaiementFacture> PaiementFactures { get; set; }
    }
}
