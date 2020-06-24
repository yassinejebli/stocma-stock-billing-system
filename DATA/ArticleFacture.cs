// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.ArticleFacture
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.DATA
{
    public class ArticleFacture
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Index(IsUnique = false)]
        [StringLength(200)]
        public string Ref { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Designation { get; set; }

        public bool Disabled { get; set; } = false;
        public string Marque { get; set; }

        [DefaultValue(0)]
        public float? MinStock { get; set; }

        [DefaultValue(0)]
        public float? MaxStock { get; set; }

        [DefaultValue(0)]
        public float QteStock { get; set; }

        [DefaultValue("U")]
        public string Unite { get; set; }

        [DefaultValue(20)]
        public float? TVA { get; set; } = 20;

        [DefaultValue(0)]
        public float PA { get; set; }

        [DefaultValue(0)]
        public float? PVD { get; set; }

        [DefaultValue(0)]
        public float? PVSG { get; set; }

        [DefaultValue(0)]
        public float? PVG { get; set; }

        public Guid? IdFamille { get; set; }

        public string Logo { get; set; }

        public string Logo2 { get; set; }

        public string Logo3 { get; set; }

        public string BarCode { get; set; }

        public DateTime? DateModification { get; set; }

        public virtual ICollection<FakeFactureItem> FakeFactureItems { get; set; }
        public virtual ICollection<FakeFactureFItem> FakeFactureFItems { get; set; }
    }
}
