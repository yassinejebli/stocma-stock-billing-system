// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.Devis
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class Rdb
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Ref { get; set; }

        public string NumBon { get; set; }

        public DateTime Date { get; set; }

        public Guid IdClient { get; set; }

        public string User { get; set; }

        public string Type { get; set; }
        public virtual Client Client { get; set; }

        public virtual ICollection<RdbItem> RdbItems { get; set; }
    }
}
