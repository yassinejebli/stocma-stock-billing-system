// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.BonLivraisonItem
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using Microsoft.Ajax.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class BonLivraisonItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdBonLivraison { get; set; }

        [Range(1, float.MaxValue)]
        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public float PA { get; set; }

        public Guid IdArticle { get; set; }

        public int? Index { get; set; }

        public string NumBC { get; set; }

        public float TotalHT { get; set; }

        public float? Discount { get; set; } = 0;

        public bool PercentageDiscount { get; set; } = false;

        public virtual BonLivraison BonLivraison { get; set; }

        public virtual Article Article { get; set; }
    }
}
