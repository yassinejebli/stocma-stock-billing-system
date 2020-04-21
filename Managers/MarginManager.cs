using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DATA;

namespace WebApplication1.Managers
{
    public class MarginManager
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        public async Task<int> UpdateMarginBL(Guid IdBonLivraison)
        {
            var bonLivraison = db.BonLivraisons.Find(IdBonLivraison);
            bonLivraison.Marge = bonLivraison.BonLivraisonItems.Sum(x => x.Qte * (x.Pu - x.Article.PA));

            return await db.SaveChangesAsync();
        }

        public async Task<int> UpdateMarginBAC(Guid IdBonAvoirC)
        {
            var bonAvoirC = db.BonAvoirCs.Find(IdBonAvoirC);
            bonAvoirC.Marge = bonAvoirC.BonAvoirCItems.Sum(x => x.Qte * (x.Pu - x.Article.PA));

            return await db.SaveChangesAsync();
        }

    }
}