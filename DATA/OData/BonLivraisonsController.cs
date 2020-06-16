using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using WebApplication1.Generators;

namespace WebApplication1.DATA.OData
{
    [Authorize]
    public class BonLivraisonsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();


        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonLivraison> GetBonLivraisons()
        {
            return db.BonLivraisons.OrderByDescending(x=>x.Date);
        }

        [EnableQuery]
        public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BonLivraisons.Where<BonLivraison>((Expression<Func<BonLivraison, bool>>)(bonLivraison => bonLivraison.Id == key)));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, BonLivraison newBonLivraison)
        {
            BonLivraison bonLivraison = await db.BonLivraisons.FindAsync(key);
            if (bonLivraison == null)
                return NotFound();

            /////////////////////////////////////////////
            //----------------------------------------------Updating QteStock
            foreach(var biOld in bonLivraison.BonLivraisonItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == bonLivraison.IdSite && x.IdArticle == biOld.IdArticle);
                articleSite.QteStock += biOld.Qte;
            }
            foreach (var biNew in newBonLivraison.BonLivraisonItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == bonLivraison.IdSite && x.IdArticle == biNew.IdArticle);
                articleSite.QteStock -= biNew.Qte;
            }


            //-----------------------------------------------Updating document items
            db.BonLivraisonItems.RemoveRange(bonLivraison.BonLivraisonItems);
            db.BonLivraisonItems.AddRange(newBonLivraison.BonLivraisonItems);

            bonLivraison.ModificationDate = DateTime.Now;
            bonLivraison.Date = newBonLivraison.Date;
            bonLivraison.Ref = newBonLivraison.Ref;
            bonLivraison.Note = newBonLivraison.Note;
            bonLivraison.IdTypePaiement = newBonLivraison.IdTypePaiement;
            bonLivraison.WithDiscount = newBonLivraison.WithDiscount;
            var numBonGenerator = new DocNumberGenerator();

            bonLivraison.NumBon = numBonGenerator.getNumDocByCompany(newBonLivraison.Ref - 1, newBonLivraison.Date);
            foreach (var bi in newBonLivraison.BonLivraisonItems)
            {
                var article = db.Articles.Find(bi.IdArticle);
                bi.PA = article.PA;
            }

            //-----------------------------------------------Updating payment
            var payment = db.Paiements.FirstOrDefault(x => x.IdBonLivraison == bonLivraison.Id);
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb6";
            var Total = newBonLivraison.BonLivraisonItems.Sum(x => (x.Qte * x.Pu) - (x.PercentageDiscount ? (x.Qte * x.Pu * (x.Discount ?? 0.0f) / 100) : x.Discount ?? 0.0f));
            if (payment != null)
            {
                payment.Debit = Total;
                payment.Date = newBonLivraison.Date;
            }
            else
            {
                Paiement paiement = new Paiement()
                {
                    Id = Guid.NewGuid(),
                    IdBonLivraison = bonLivraison.Id,
                    IdClient = newBonLivraison.IdClient,
                    Debit = Total,
                    IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                    Date = newBonLivraison.Date
                };
                db.Paiements.Add(paiement);
            }
            ////////////////////////////////////////////


            try
            {
                await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonLivraisonExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }

            var bonLivraisonWithItems = db.BonLivraisons.Where(x => x.Id == bonLivraison.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(bonLivraisonWithItems));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Post(BonLivraison bonLivraison)
        {
            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);
            var numBonGenerator = new DocNumberGenerator();
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.BonLivraisons.Where(x=>x.Date.Year == currentYear && x.IdSite == bonLivraison.IdSite).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 0;
            bonLivraison.Ref = lastRef + 1;
            foreach(var bi in bonLivraison.BonLivraisonItems)
            {
                var article = db.Articles.Find(bi.IdArticle);
                bi.PA = article.PA;
            }
            bonLivraison.NumBon = numBonGenerator.getNumDocByCompany(lastRef, bonLivraison.Date);
            this.db.BonLivraisons.Add(bonLivraison);

            //-----------------------------------------------Updating payment
            var payment = db.Paiements.FirstOrDefault(x => x.IdBonLivraison == bonLivraison.Id);
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb6";
            var Total = bonLivraison.BonLivraisonItems.Sum(x => (x.Qte * x.Pu) - (x.PercentageDiscount ? (x.Qte * x.Pu * (x.Discount ?? 0.0f) / 100) : x.Discount ?? 0.0f));
            if (payment != null)
            {
                payment.Debit = Total;
                payment.Date = bonLivraison.Date;
            }
            else
            {
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

            //-------------------------------------------updating QteStock
            foreach(var bi in bonLivraison.BonLivraisonItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdArticle == bi.IdArticle && x.IdSite == bonLivraison.IdSite);
                articleSite.QteStock -= bi.Qte;
            }
            //-------------------------------------------

            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.BonLivraisonExists(bonLivraison.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            var bonLivraisonWithItems = db.BonLivraisons.Where(x=>x.Id == bonLivraison.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(bonLivraisonWithItems));
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            BonLivraison async = await this.db.BonLivraisons.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();

            //--------------------------updating QteStock
            foreach (var bi in async.BonLivraisonItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == async.IdSite && x.IdArticle == bi.IdArticle);
                articleSite.QteStock += bi.Qte;
            }

            db.Paiements.RemoveRange(async.Paiements);
            db.BonLivraisonItems.RemoveRange(async.BonLivraisonItems);
            db.BonLivraisons.Remove(async);
            int num = await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public IQueryable<BonLivraisonItem> GetBonLivraisonItems([FromODataUri] Guid key)
        {
            return this.db.BonLivraisons.Where<BonLivraison>((Expression<Func<BonLivraison, bool>>)(m => m.Id == key)).SelectMany<BonLivraison, BonLivraisonItem>((Expression<Func<BonLivraison, IEnumerable<BonLivraisonItem>>>)(m => m.BonLivraisonItems));
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.BonLivraisons.Where<BonLivraison>((Expression<Func<BonLivraison, bool>>)(m => m.Id == key)).Select<BonLivraison, Client>((Expression<Func<BonLivraison, Client>>)(m => m.Client)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool BonLivraisonExists(Guid key)
        {
            return this.db.BonLivraisons.Count<BonLivraison>((Expression<Func<BonLivraison, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
