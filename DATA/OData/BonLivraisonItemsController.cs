﻿
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using WebApplication1.PaimentManager;

namespace WebApplication1.DATA.OData
{
    [Authorize]
    public class BonLivraisonItemsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();
        private PaymentManager pm = new PaymentManager();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonLivraisonItem> GetBonLivraisonItems()
        {
            return db.BonLivraisonItems.OrderByDescending(x=>x.BonLivraison.Date);
        }

        [EnableQuery]
        public SingleResult<BonLivraisonItem> GetBonLivraisonItem([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonLivraisonItem>(this.db.BonLivraisonItems.Where<BonLivraisonItem>((Expression<Func<BonLivraisonItem, bool>>)(bonLivraisonItem => bonLivraisonItem.Id == key)));
        }

        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<BonLivraisonItem> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            BonLivraisonItem bonLivraisonItem = await this.db.BonLivraisonItems.FindAsync((object)key);
            if (bonLivraisonItem == null)
                return (IHttpActionResult)this.NotFound();
            patch.Put(bonLivraisonItem);
            try
            {
                int num = await this.db.SaveChangesAsync();
                await pm.UpdatePaiementBL(bonLivraisonItem.IdBonLivraison);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonLivraisonItemExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<BonLivraisonItem>(bonLivraisonItem);
        }

        public async Task<IHttpActionResult> Post(BonLivraisonItem bonLivraisonItem)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            db.BonLivraisonItems.Add(bonLivraisonItem);
            try
            {
                await db.SaveChangesAsync();
                await pm.UpdatePaiementBL(bonLivraisonItem.IdBonLivraison);
            }
            catch (DbUpdateException ex)
            {
                if (this.BonLivraisonItemExists(bonLivraisonItem.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            return (IHttpActionResult)this.Created<BonLivraisonItem>(bonLivraisonItem);
        }


        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonLivraisonItem> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            BonLivraisonItem bonLivraisonItem = await this.db.BonLivraisonItems.FindAsync((object)key);
            if (bonLivraisonItem == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(bonLivraisonItem);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonLivraisonItemExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<BonLivraisonItem>(bonLivraisonItem);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            BonLivraisonItem async = await this.db.BonLivraisonItems.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();
            db.BonLivraisonItems.Remove(async);
            await db.SaveChangesAsync();
            await pm.UpdatePaiementBL(async.IdBonLivraison);
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create<Article>(this.db.BonLivraisonItems.Where<BonLivraisonItem>((Expression<Func<BonLivraisonItem, bool>>)(m => m.Id == key)).Select<BonLivraisonItem, Article>((Expression<Func<BonLivraisonItem, Article>>)(m => m.Article)));
        }

        [EnableQuery]
        public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonLivraison>(this.db.BonLivraisonItems.Where<BonLivraisonItem>((Expression<Func<BonLivraisonItem, bool>>)(m => m.Id == key)).Select<BonLivraisonItem, BonLivraison>((Expression<Func<BonLivraisonItem, BonLivraison>>)(m => m.BonLivraison)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool BonLivraisonItemExists(Guid key)
        {
            return this.db.BonLivraisonItems.Count<BonLivraisonItem>((Expression<Func<BonLivraisonItem, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
