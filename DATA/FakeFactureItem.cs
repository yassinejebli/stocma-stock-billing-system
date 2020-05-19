// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.FakeFactureItem
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class FakeFactureItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdFakeFacture { get; set; }

        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public float? Discount { get; set; } = 0;

        public bool PercentageDiscount { get; set; } = false;

        public Guid IdArticleFacture { get; set; }

        public float TotalHT { get; set; }

        public virtual FakeFacture FakeFacture { get; set; }

        public virtual ArticleFacture ArticleFacture { get; set; }
    }
}
