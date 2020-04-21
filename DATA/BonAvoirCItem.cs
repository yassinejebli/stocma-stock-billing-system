// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.BonAvoirCItem
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
  public class BonAvoirCItem
  {
    [Key]
    public Guid Id { get; set; }

    public Guid IdBonAvoirC { get; set; }

    public float Qte { get; set; }

    [DefaultValue(0)]
    public float Pu { get; set; }

    public Guid IdArticle { get; set; }

    public float TotalHT { get; set; }

        public string NumBL { get; set; }

        [DefaultValue(false)]
    public bool Casse { get; set; }

    public virtual BonAvoirC BonAvoirC { get; set; }

    public virtual Article Article { get; set; }
  }
}
