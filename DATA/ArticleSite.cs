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

        public Guid IdArticle { get; set; }
        public int IdSite { get; set; }



    }
}