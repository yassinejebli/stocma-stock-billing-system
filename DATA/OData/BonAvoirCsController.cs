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
    public class BonAvoirCsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonAvoirC> GetBonAvoirCs()
        {
            return (IQueryable<BonAvoirC>)this.db.BonAvoirCs.OrderByDescending(x=>x.Date);
        }

        [EnableQuery]
        public SingleResult<BonAvoirC> GetBonAvoirC([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonAvoirC>(this.db.BonAvoirCs.Where<BonAvoirC>((Expression<Func<BonAvoirC, bool>>)(bonAvoirC => bonAvoirC.Id == key)));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, BonAvoirC newBonAvoiC)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            BonAvoirC bonAvoirC = await this.db.BonAvoirCs.FindAsync((object)key);
            if (bonAvoirC == null)
                return (IHttpActionResult)this.NotFound();

            var oldNumBon = bonAvoirC.NumBon;
            /////////////////////////////////////////////
            //----------------------------------------------Updating QteStock
            foreach (var biOld in bonAvoirC.BonAvoirCItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == biOld.IdSite && x.IdArticle == biOld.IdArticle);
                articleSite.QteStock -= biOld.Qte;
            }
            foreach (var biNew in newBonAvoiC.BonAvoirCItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == biNew.IdSite && x.IdArticle == biNew.IdArticle);
                articleSite.QteStock += biNew.Qte;
            }


            //-----------------------------------------------Updating document items
            db.BonAvoirCItems.RemoveRange(bonAvoirC.BonAvoirCItems);
            db.BonAvoirCItems.AddRange(newBonAvoiC.BonAvoirCItems);

            bonAvoirC.Date = newBonAvoiC.Date;
            bonAvoirC.Ref = newBonAvoiC.Ref;
            bonAvoirC.IdSite = newBonAvoiC.IdSite;
            bonAvoirC.Note = newBonAvoiC.Note;
            var numBonGenerator = new DocNumberGenerator();
            bonAvoirC.NumBon = numBonGenerator.getNumDocByCompany(newBonAvoiC.Ref - 1, newBonAvoiC.Date);
            foreach (var bi in newBonAvoiC.BonAvoirCItems)
            {
                var article = db.Articles.Find(bi.IdArticle);
                bi.PA = article.PA;
            }

            //-----------------------------------------------Updating payment
            var company = StatistiqueController.getCompany();
            var AVOIR_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb8";
            if (company.UseVAT)
            {
                var Total = newBonAvoiC.BonAvoirCItems.Sum(x => (x.Qte * x.Pu) * (1 + (db.Articles.Find(x.IdArticle).TVA ?? 20) / 100));
                var payment = db.PaiementFactures.FirstOrDefault(x => x.IdBonAvoirC == bonAvoirC.Id);
                if (payment != null)
                {
                    payment.Credit = Total;
                    payment.Date = bonAvoirC.Date;
                    payment.Comment = "Avoir " + bonAvoirC.NumBon;
                }
                else
                {
                    PaiementFacture paiement = new PaiementFacture()
                    {
                        Id = Guid.NewGuid(),
                        IdBonAvoirC = newBonAvoiC.Id,
                        IdClient = newBonAvoiC.IdClient,
                        Credit = Total,
                        IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                        Date = newBonAvoiC.Date,
                        Comment = "Avoir " + newBonAvoiC.NumBon
                    };
                    db.PaiementFactures.Add(paiement);
                }
            }
            else
            {
                var payment = db.Paiements.FirstOrDefault(x => x.IdBonAvoirC == bonAvoirC.Id);
                var Total = newBonAvoiC.BonAvoirCItems.Sum(x => x.Qte * x.Pu);
                if (payment != null)
                {
                    payment.Credit = Total;
                    payment.Date = bonAvoirC.Date;
                    payment.Comment = "Avoir " + bonAvoirC.NumBon;
                }
                else
                {
                    Paiement paiement = new Paiement()
                    {
                        Id = Guid.NewGuid(),
                        IdBonAvoirC = newBonAvoiC.Id,
                        IdClient = newBonAvoiC.IdClient,
                        Credit = Total,
                        IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                        Date = newBonAvoiC.Date,
                        Comment = "Avoir " + bonAvoirC.NumBon
                };
                    db.Paiements.Add(paiement);
                }

                if (db.Clients.Find(bonAvoirC.IdClient).IsClientDivers)
                {
                    var REMBOURSEMENT_PAIEMENT_TYPE_ID = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4");
                    var paymentClientDivers = db.Paiements.FirstOrDefault(x => x.Comment == "Avoir " + oldNumBon);
                    if (paymentClientDivers != null)
                    {
                        paymentClientDivers.Debit = Total;
                        paymentClientDivers.Date = newBonAvoiC.Date;
                        paymentClientDivers.Comment = "Avoir " + newBonAvoiC.NumBon;
                    }
                    else
                    {
                        Paiement paiementClientDivers = new Paiement()
                        {
                            Id = Guid.NewGuid(),
                            IdClient = bonAvoirC.IdClient,
                            Credit = Total,
                            IdTypePaiement = REMBOURSEMENT_PAIEMENT_TYPE_ID,
                            Date = newBonAvoiC.Date,
                            Comment = "Avoir " + newBonAvoiC.NumBon,
                        };
                        db.Paiements.Add(paiementClientDivers);
                    }

                }
            }

      
            ////////////////////////////////////////////


            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonAvoirCExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            var bonAvoirWithItems = db.BonAvoirCs.Where(x => x.Id == bonAvoirC.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(bonAvoirWithItems));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Post(BonAvoirC bonAvoirC)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);

            var numBonGenerator = new DocNumberGenerator();
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.BonAvoirCs.Where(x => x.Date.Year == currentYear && x.IdSite == bonAvoirC.IdSite).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 0;
            bonAvoirC.Ref = lastRef + 1;
            bonAvoirC.NumBon = numBonGenerator.getNumDocByCompany(lastRef, bonAvoirC.Date);
            foreach (var bi in bonAvoirC.BonAvoirCItems)
            {
                var article = db.Articles.Find(bi.IdArticle);
                bi.PA = article.PA;
            }
            //---------------------------Updating Qte stock
            foreach (var bi in bonAvoirC.BonAvoirCItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdArticle == bi.IdArticle && x.IdSite == bi.IdSite);
                articleSite.QteStock += bi.Qte;
            }

            //----------------------------Transaction
            var company = StatistiqueController.getCompany();
            var AVOIR_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb8";

            if (company.UseVAT)
            {
                var Total = bonAvoirC.BonAvoirCItems.Sum(x => (x.Qte * x.Pu) * (1 + (db.Articles.Find(x.IdArticle).TVA ?? 20) / 100));
                PaiementFacture paiement = new PaiementFacture()
                {
                    Id = Guid.NewGuid(),
                    IdBonAvoirC = bonAvoirC.Id,
                    IdClient = bonAvoirC.IdClient,
                    Credit = Total,
                    IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                    Date = bonAvoirC.Date,
                    Comment = "Avoir " + bonAvoirC.NumBon
                };
                db.PaiementFactures.Add(paiement);
            }
            else
            {
                var Total = bonAvoirC.BonAvoirCItems.Sum(x => x.Qte * x.Pu);
                Paiement paiement = new Paiement()
                {
                    Id = Guid.NewGuid(),
                    IdBonAvoirC = bonAvoirC.Id,
                    IdClient = bonAvoirC.IdClient,
                    Credit = Total,
                    IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                    Date = bonAvoirC.Date,
                    Comment = "Avoir " + bonAvoirC.NumBon,
                };
                db.Paiements.Add(paiement);


                if (db.Clients.Find(bonAvoirC.IdClient).IsClientDivers)
                {
                    var REMBOURSEMENT_PAIEMENT_TYPE_ID = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4");
                    Paiement paiementClientDivers = new Paiement()
                    {
                        Id = Guid.NewGuid(),
                        IdClient = bonAvoirC.IdClient,
                        Debit = Total,
                        IdTypePaiement = REMBOURSEMENT_PAIEMENT_TYPE_ID,
                        Date = bonAvoirC.Date,
                        Comment = "Avoir " + bonAvoirC.NumBon,
                    };
                    db.Paiements.Add(paiementClientDivers);
                }
            }


            db.BonAvoirCs.Add(bonAvoirC);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.BonAvoirCExists(bonAvoirC.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            var bonAvoirWithItems = db.BonAvoirCs.Where(x => x.Id == bonAvoirC.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(bonAvoirWithItems));
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonAvoirC> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            BonAvoirC bonAvoirC = await this.db.BonAvoirCs.FindAsync((object)key);
            if (bonAvoirC == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(bonAvoirC);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonAvoirCExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<BonAvoirC>(bonAvoirC);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            BonAvoirC async = await this.db.BonAvoirCs.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();

            //--------------------------updating QteStock
            foreach (var bi in async.BonAvoirCItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == bi.IdSite && x.IdArticle == bi.IdArticle);
                articleSite.QteStock -= bi.Qte;
            }

            if (db.Clients.Find(async.IdClient).IsClientDivers)
            {
                var REMBOURSEMENT_PAIEMENT_TYPE_ID = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4");
                var paymentClientDivers = db.Paiements.FirstOrDefault(x => x.Comment == "Avoir " + async.NumBon && x.IdTypePaiement == REMBOURSEMENT_PAIEMENT_TYPE_ID);
                if (paymentClientDivers != null)
                {
                    db.Paiements.Remove(paymentClientDivers);
                }
            }

            db.Paiements.RemoveRange(async.Paiements);
            db.PaiementFactures.RemoveRange(async.PaiementFactures);
            db.BonAvoirCItems.RemoveRange(async.BonAvoirCItems);
            db.BonAvoirCs.Remove(async);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public IQueryable<BonAvoirCItem> GetBonAvoirCItems([FromODataUri] Guid key)
        {
            return this.db.BonAvoirCs.Where<BonAvoirC>((Expression<Func<BonAvoirC, bool>>)(m => m.Id == key)).SelectMany<BonAvoirC, BonAvoirCItem>((Expression<Func<BonAvoirC, IEnumerable<BonAvoirCItem>>>)(m => m.BonAvoirCItems));
        }

        [EnableQuery]
        public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonLivraison>(this.db.BonAvoirCs.Where<BonAvoirC>((Expression<Func<BonAvoirC, bool>>)(m => m.Id == key)).Select<BonAvoirC, BonLivraison>((Expression<Func<BonAvoirC, BonLivraison>>)(m => m.BonLivraison)));
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.BonAvoirCs.Where<BonAvoirC>((Expression<Func<BonAvoirC, bool>>)(m => m.Id == key)).Select<BonAvoirC, Client>((Expression<Func<BonAvoirC, Client>>)(m => m.Client)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool BonAvoirCExists(Guid key)
        {
            return this.db.BonAvoirCs.Count<BonAvoirC>((Expression<Func<BonAvoirC, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
