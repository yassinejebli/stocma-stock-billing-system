// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.BonAvoirItem
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class BonAvoirItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdBonAvoir { get; set; }

        [Range(1, float.MaxValue)]
        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public Guid IdArticle { get; set; }

        public float TotalHT { get; set; }

        public virtual BonAvoir BonAvoir { get; set; }

        public virtual Article Article { get; set; }
    }
}
