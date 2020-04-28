// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.Fournisseur
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApplication1.DATA
{
    public class Fournisseur
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Name { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Adresse { get; set; }

        public string ICE { get; set; }
        public bool Disabled { get; set; } = false;

        [NotMapped]
        public float Solde
        {
            get { return (PaiementFs != null) ? PaiementFs.Sum(x => x.Debit - x.Credit) : 0; }
        }
        public virtual ICollection<BonReception> BonReceptions { get; set; }

        public virtual ICollection<BonAvoir> BonAvoirs { get; set; }

        public virtual ICollection<BonCommande> BonCommandes { get; set; }

        public virtual ICollection<PaiementF> PaiementFs { get; set; }
        public virtual ICollection<DgbF> DgbFs { get; set; }
        public virtual ICollection<RdbF> RdbFs { get; set; }
        public virtual ICollection<FactureF> FactureFs { get; set; }
        public virtual ICollection<FakeFactureF> FakeFactureFs { get; set; }
    }
}
