using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.DATA
{
    public class ArticleSite
    {
        [DefaultValue(0)]
        public float QteStock { get; set; }

        public virtual Article Article { get; set; }
        public virtual Site Site { get; set; }

        [Key, Column(Order = 0)]
        public Guid IdArticle { get; set; }

        [Key, Column(Order = 1)]
        public int IdSite { get; set; }

        public int Counter { get; set; } = 0;

        public bool Disabled { get; set; } = false;
    }
}