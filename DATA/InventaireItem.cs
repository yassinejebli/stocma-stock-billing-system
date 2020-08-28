using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class InventaireItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid IdArticle { get; set; }
        public Guid IdInvetaire { get; set; }
        public Guid? IdCategory { get; set; }

        public float QteStock { get; set; }
        public float QteStockReel { get; set; }
        public virtual Categorie Categorie { get; set; }
        public virtual Inventaire Inventaire { get; set; }
        public virtual Article Article { get; set; }
    }
}
