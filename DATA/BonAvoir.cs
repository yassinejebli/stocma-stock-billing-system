﻿// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.BonAvoir
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class BonAvoir
    {
        [Key]
        public Guid Id { get; set; }
        public int? IdSite { get; set; }

        public string NumBon { get; set; }

        public int Ref { get; set; }

        public DateTime Date { get; set; }

        public Guid IdFournisseur { get; set; }

        public Guid? IdBonReception { get; set; }

        public virtual Fournisseur Fournisseur { get; set; }

        public virtual BonReception BonReception { get; set; }
        public virtual Site Site { get; set; }

        public virtual ICollection<BonAvoirItem> BonAvoirItems { get; set; }
        public virtual ICollection<PaiementFactureF> PaiementFactureFs { get; set; }
        public virtual ICollection<PaiementF> PaiementFs { get; set; }
    }
}
