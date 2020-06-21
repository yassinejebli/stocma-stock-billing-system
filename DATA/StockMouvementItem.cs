using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class StockMouvementItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdStockMouvement { get; set; }

        public float Qte { get; set; }

        public Guid IdArticle { get; set; }

        public virtual StockMouvement StockMouvement { get; set; }

        public virtual Article Article { get; set; }
    }
}
