// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.Client
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApplication1.DATA
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();


        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Name { get; set; }

        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        [DefaultValue(0)]
        public float Plafond { get; set; }

        public string Adresse { get; set; }
        public string ICE { get; set; }

        [NotMapped]
        public float Solde
        {
            get { return (Paiements != null) ? Paiements.Sum(x => x.Debit - x.Credit) : 0; }
        }

        public bool Disabled { get; set; } = false;

        public DateTime? DateCreation { get; set; }

        public Guid? IdRevendeur { get; set; }

        public Revendeur Revendeur { get; set; }

        public virtual ICollection<BonLivraison> BonLivraisons { get; set; }

        public virtual ICollection<Devis> Devises { get; set; }

        public virtual ICollection<BonAvoirC> BonAvoirCs { get; set; }

        public virtual ICollection<Paiement> Paiements { get; set; }

        public virtual ICollection<Facture> Factures { get; set; }

        public virtual ICollection<FakeFacture> FakeFactures { get; set; }

        public virtual ICollection<Rdb> Rdbs { get; set; }

        public virtual ICollection<Dgb> Dgbs { get; set; }

    }
}
