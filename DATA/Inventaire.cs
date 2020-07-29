using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class Inventaire
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int IdSite { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public string Titre { get; set; } = "";
        public virtual Site Site { get; set; }

        public virtual ICollection<InventaireItem> InventaireItems { get; set; }
    }
}
