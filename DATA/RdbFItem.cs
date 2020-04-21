// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.DevisItem
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
  public class RdbFItem
  {
    [Key]
    public Guid Id { get; set; }

    public Guid IdRdbF { get; set; }

    public float Qte { get; set; }

    public Guid IdArticle { get; set; }

    public virtual RdbF RdbF { get; set; }

    public virtual Article Article { get; set; }
  }
}
