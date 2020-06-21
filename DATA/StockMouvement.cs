using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class StockMouvement
    {
        [Key]
        public Guid Id { get; set; }
        public int IdSiteFrom { get; set; }
        public int IdSiteTo { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public virtual Site SiteFrom { get; set; }
        public virtual Site SiteTo { get; set; }

        public virtual ICollection<StockMouvementItem> StockMouvementItems { get; set; }


    }
}
