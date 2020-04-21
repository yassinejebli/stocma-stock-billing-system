using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DATA
{
    public class FakeFactureFItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdFakeFactureF { get; set; }

        public float Qte { get; set; }

        [DefaultValue(0)]
        public float Pu { get; set; }

        public Guid IdArticleFacture { get; set; }

        public float TotalHT { get; set; }

        public string NumBR { get; set; }

        public virtual FakeFactureF FakeFactureF { get; set; }

        public virtual ArticleFacture ArticleFacture { get; set; }
    }
}
