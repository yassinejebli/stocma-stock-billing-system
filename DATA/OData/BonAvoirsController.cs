// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.BonAvoirsController
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;

namespace WebApplication1.DATA.OData
{
    public class BonAvoirsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonAvoir> GetBonAvoirs()
        {
            return (IQueryable<BonAvoir>)this.db.BonAvoirs.OrderByDescending(x=>x.Date);
        }

        [EnableQuery]
        public SingleResult<BonAvoir> GetBonAvoir([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonAvoir>(this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>)(bonAvoir => bonAvoir.Id == key)));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, BonAvoir newBonAvoir)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            BonAvoir bonAvoir = await db.BonAvoirs.FindAsync((object)key);
            if (bonAvoir == null)
                return (IHttpActionResult)this.NotFound();

            //----------------------------------------------Updating QteStock
            foreach (var biOld in bonAvoir.BonAvoirItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == bonAvoir.IdSite && x.IdArticle == biOld.IdArticle);
                articleSite.QteStock += biOld.Qte;
            }
            foreach (var biNew in newBonAvoir.BonAvoirItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == newBonAvoir.IdSite && x.IdArticle == biNew.IdArticle);
                articleSite.QteStock -= biNew.Qte;
            }


            //-----------------------------------------------Updating document items
            db.BonAvoirItems.RemoveRange(bonAvoir.BonAvoirItems);
            db.BonAvoirItems.AddRange(newBonAvoir.BonAvoirItems);

            bonAvoir.Date = newBonAvoir.Date;
            bonAvoir.NumBon = newBonAvoir.NumBon;


            //-----------------------------------------------Updating payment
            var company = StatistiqueController.getCompany();
            var AVOIR_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb8";
            if (company.UseVAT)
            {
                var Total = newBonAvoir.BonAvoirItems.Sum(x => (x.Qte * x.Pu) * (1 + (db.Articles.Find(x.IdArticle).TVA ?? 20) / 100));
                var payment = db.PaiementFactureFs.FirstOrDefault(x => x.IdBonAvoir == bonAvoir.Id);
                if (payment != null)
                {
                    payment.Credit = Total;
                    payment.Date = bonAvoir.Date;
                    payment.Comment = "Avoir " + bonAvoir.NumBon;
                }
                else
                {
                    PaiementFactureF paiement = new PaiementFactureF()
                    {
                        Id = Guid.NewGuid(),
                        IdBonAvoir = newBonAvoir.Id,
                        IdFournisseur = newBonAvoir.IdFournisseur,
                        Credit = Total,
                        IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                        Date = newBonAvoir.Date,
                        Comment = "Avoir " + newBonAvoir.NumBon
                };
                    db.PaiementFactureFs.Add(paiement);
                }
            }
            else
            {
                var payment = db.Paiements.FirstOrDefault(x => x.IdBonAvoirC == bonAvoir.Id);
                var Total = newBonAvoir.BonAvoirItems.Sum(x => x.Qte * x.Pu);
                if (payment != null)
                {
                    payment.Credit = Total;
                    payment.Date = bonAvoir.Date;
                    payment.Comment = "Avoir " + bonAvoir.NumBon;
                }
                else
                {
                    PaiementF paiement = new PaiementF()
                    {
                        Id = Guid.NewGuid(),
                        IdBonAvoir = newBonAvoir.Id,
                        IdFournisseur = newBonAvoir.IdFournisseur,
                        Credit = Total,
                        IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                        Date = newBonAvoir.Date,
                        Comment = "Avoir " + newBonAvoir.NumBon
                    };
                    db.PaiementFs.Add(paiement);
                }
            }
            ////////////////////////////////////////////


            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonAvoirExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            var bonAvoirWithItems = db.BonAvoirs.Where(x => x.Id == bonAvoir.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(bonAvoirWithItems));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Post(BonAvoir bonAvoir)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);


            //---------------------------Updating Qte stock
            foreach (var bi in bonAvoir.BonAvoirItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdArticle == bi.IdArticle && x.IdSite == bonAvoir.IdSite);
                articleSite.QteStock -= bi.Qte;
            }

            //----------------------------Transaction
            var company = StatistiqueController.getCompany();
            var AVOIR_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb8";

            if (company.UseVAT)
            {
                var Total = bonAvoir.BonAvoirItems.Sum(x => (x.Qte * x.Pu) * (1 + (db.Articles.Find(x.IdArticle).TVA ?? 20) / 100));
                PaiementFactureF paiement = new PaiementFactureF()
                {
                    Id = Guid.NewGuid(),
                    IdBonAvoir = bonAvoir.Id,
                    IdFournisseur = bonAvoir.IdFournisseur,
                    Credit = Total,
                    IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                    Date = bonAvoir.Date,
                    Comment = "Avoir " + bonAvoir.NumBon
                };
                db.PaiementFactureFs.Add(paiement);
            }
            else
            {
                var Total = bonAvoir.BonAvoirItems.Sum(x => x.Qte * x.Pu);
                PaiementF paiement = new PaiementF()
                {
                    Id = Guid.NewGuid(),
                    IdBonAvoir = bonAvoir.Id,
                    IdFournisseur = bonAvoir.IdFournisseur,
                    Credit = Total,
                    IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                    Date = bonAvoir.Date,
                    Comment = "Avoir " + bonAvoir.NumBon
                };
                db.PaiementFs.Add(paiement);
            }

            db.BonAvoirs.Add(bonAvoir);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.BonAvoirExists(bonAvoir.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            var bonAvoirWithItems = db.BonAvoirs.Where(x => x.Id == bonAvoir.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(bonAvoirWithItems));
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonAvoir> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            BonAvoir bonAvoir = await this.db.BonAvoirs.FindAsync((object)key);
            if (bonAvoir == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(bonAvoir);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonAvoirExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated(bonAvoir);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            BonAvoir async = await db.BonAvoirs.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();

            //--------------------------updating QteStock
            foreach (var bi in async.BonAvoirItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == async.IdSite && x.IdArticle == bi.IdArticle);
                articleSite.QteStock += bi.Qte;
            }

            db.PaiementFs.RemoveRange(async.PaiementFs);
            db.PaiementFactureFs.RemoveRange(async.PaiementFactureFs);
            db.BonAvoirItems.RemoveRange(async.BonAvoirItems);
            db.BonAvoirs.Remove(async);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public IQueryable<BonAvoirItem> GetBonAvoirItems([FromODataUri] Guid key)
        {
            return this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>)(m => m.Id == key)).SelectMany<BonAvoir, BonAvoirItem>((Expression<Func<BonAvoir, IEnumerable<BonAvoirItem>>>)(m => m.BonAvoirItems));
        }

        [EnableQuery]
        public SingleResult<BonReception> GetBonReception([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonReception>(this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>)(m => m.Id == key)).Select<BonAvoir, BonReception>((Expression<Func<BonAvoir, BonReception>>)(m => m.BonReception)));
        }

        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create<Fournisseur>(this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>)(m => m.Id == key)).Select<BonAvoir, Fournisseur>((Expression<Func<BonAvoir, Fournisseur>>)(m => m.Fournisseur)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool BonAvoirExists(Guid key)
        {
            return this.db.BonAvoirs.Count<BonAvoir>((Expression<Func<BonAvoir, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
