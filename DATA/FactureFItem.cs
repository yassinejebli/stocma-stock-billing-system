using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class FactureFItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdFactureF { get; set; }

        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public Guid IdArticle { get; set; }

        public float TotalHT { get; set; }

        public string NumBR { get; set; }

        public virtual FactureF FactureF { get; set; }

        public virtual Article Article { get; set; }
    }
}
