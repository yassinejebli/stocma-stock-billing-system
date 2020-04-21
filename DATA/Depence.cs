// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.Depence
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
  public class Depence
  {
    [Key]
    public Guid Id { get; set; }

    public Guid IdTypeDepence { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string Comment { get; set; }

    [Required]
    public float Montant { get; set; }

    public virtual TypeDepence TypeDepence { get; set; }
  }
}
