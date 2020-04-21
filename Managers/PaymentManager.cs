using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.DATA;

namespace WebApplication1.PaimentManager
{
    public class PaymentManager
    {
        private MySaniSoftContext db = new MySaniSoftContext();
        private Guid ClientDiversId = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39");
        public async Task<int> UpdatePaiementBL(Guid Id)
        {
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb6";
            var bonLivraison = db.BonLivraisons.Find(Id);
            var Total = bonLivraison.BonLivraisonItems.Sum(x => x.Qte * x.Pu);
            var paimentObject = db.Paiements.Where((x => x.BonLivraison.Id == bonLivraison.Id)).FirstOrDefault();
            if (paimentObject != null)
            {
                paimentObject.Debit = Total;
                paimentObject.Date = bonLivraison.Date;
                paimentObject.Comment = "BL " + bonLivraison.NumBon;
            }
            else
            {
                //Add a new transaction if it doesn't exist in the db
                Paiement paiement = new Paiement()
                {
                    Id = Guid.NewGuid(),
                    IdBonLivraison = bonLivraison.Id,
                    IdClient = bonLivraison.IdClient,
                    Debit = Total,
                    IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                    Date = bonLivraison.Date
                };
                db.Paiements.Add(paiement);
            }
            int result = await db.SaveChangesAsync();
            return result;
        }

    }
}