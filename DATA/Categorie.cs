// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.Article
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
    public class Categorie
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Index(IsUnique = false)]
        [StringLength(200)]
        public string Name { get; set; }

        public Guid? IdFamille { get; set; }
        public virtual Famille Famille { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

    }
}
