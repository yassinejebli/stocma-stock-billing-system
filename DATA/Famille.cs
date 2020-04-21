// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.Famille
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.DATA
{
  public class Famille
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Index(IsUnique = true)]
    [StringLength(200)]
    public string Name { get; set; }

    public virtual ICollection<Categorie> Categories { get; set; }
  }
}
