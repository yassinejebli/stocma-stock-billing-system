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
    public class Devis
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Ref { get; set; }

        public string NumBon { get; set; }

        public int? IdSite { get; set; }


        //SUIV
        public string TypeReglement { get; set; }// espece, cheque, effet, au-comptant
        public string DelaiLivrasion { get; set; }
        public string TransportExpedition { get; set; }
        public string ValiditeOffre { get; set; }
        public string Note { get; set; }

        //

        public DateTime Date { get; set; }

        public Guid IdClient { get; set; }

        public string User { get; set; }

        public virtual Client Client { get; set; }
        public virtual Site Site { get; set; }

        public virtual ICollection<DevisItem> DevisItems { get; set; }
    }
}
