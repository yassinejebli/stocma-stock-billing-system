using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;

namespace WebApplication1.DATA.OData
{
    [Authorize]
    public class StockMouvementsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/StockMouvements
        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<StockMouvement> GetStockMouvements()
        {
            return db.StockMouvements.OrderByDescending(x => x.Date);
        }

        // GET: odata/StockMouvements(5)
        [EnableQuery]
        public SingleResult<StockMouvement> GetStockMouvement([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.StockMouvements.Where(stockMouvement => stockMouvement.Id == key));
        }

        // PUT: odata/StockMouvements(5)
        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, StockMouvement newStockMouvement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StockMouvement stockMouvement = await db.StockMouvements.FindAsync(key);
            if (stockMouvement == null)
            {
                return NotFound();
            }

            //----------------------------------------------Updating QteStock
            foreach(var diOld in stockMouvement.StockMouvementItems)
            {
                var articleSiteFrom = db.ArticleSites.FirstOrDefault(x => x.IdArticle == diOld.IdArticle && x.IdSite == stockMouvement.IdSiteFrom);
                var articleSiteTo = db.ArticleSites.FirstOrDefault(x => x.IdArticle == diOld.IdArticle && x.IdSite == stockMouvement.IdSiteTo);
                articleSiteFrom.QteStock += diOld.Qte;
                articleSiteTo.QteStock -= diOld.Qte;
                articleSiteTo.Disabled = false;
            }
            foreach (var diNew in newStockMouvement.StockMouvementItems)
            {
                var articleSiteFrom = db.ArticleSites.FirstOrDefault(x => x.IdArticle == diNew.IdArticle && x.IdSite == stockMouvement.IdSiteFrom);
                var articleSiteTo = db.ArticleSites.FirstOrDefault(x => x.IdArticle == diNew.IdArticle && x.IdSite == stockMouvement.IdSiteTo);
                articleSiteFrom.QteStock -= diNew.Qte;
                articleSiteTo.QteStock += diNew.Qte;
            }


            //-----------------------------------------------Updating document items
            db.StockMouvementItems.RemoveRange(stockMouvement.StockMouvementItems);
            db.StockMouvementItems.AddRange(newStockMouvement.StockMouvementItems);

            stockMouvement.Date = newStockMouvement.Date;
            stockMouvement.Comment = newStockMouvement.Comment;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockMouvementExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var stockMouvementWithItems = db.StockMouvements.Where(x => x.Id == stockMouvement.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(stockMouvementWithItems));
        }

        // POST: odata/StockMouvements
        [EnableQuery]
        public async Task<IHttpActionResult> Post(StockMouvement stockMouvement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-------------------------- Updating stock
            foreach (var di in stockMouvement.StockMouvementItems)
            {
                var articleSiteFrom = db.ArticleSites.FirstOrDefault(x => x.IdArticle == di.IdArticle && x.IdSite == stockMouvement.IdSiteFrom);
                var articleSiteTo = db.ArticleSites.FirstOrDefault(x => x.IdArticle == di.IdArticle && x.IdSite == stockMouvement.IdSiteTo);
                articleSiteFrom.QteStock -= di.Qte;
                articleSiteTo.QteStock += di.Qte;
                articleSiteTo.Disabled = false;
            }
            db.StockMouvements.Add(stockMouvement);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StockMouvementExists(stockMouvement.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var stockMouvementWithItems = db.StockMouvements.Where(x => x.Id == stockMouvement.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(stockMouvementWithItems));
        }

        // PATCH: odata/StockMouvements(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<StockMouvement> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StockMouvement stockMouvement = await db.StockMouvements.FindAsync(key);
            if (stockMouvement == null)
            {
                return NotFound();
            }

            patch.Patch(stockMouvement);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockMouvementExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(stockMouvement);
        }

        // DELETE: odata/StockMouvements(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            StockMouvement stockMouvement = await db.StockMouvements.FindAsync(key);
            if (stockMouvement == null)
            {
                return NotFound();
            }

            //--------------------------updating QteStock
            foreach (var di in stockMouvement.StockMouvementItems)
            {
                var articleSiteFrom = db.ArticleSites.FirstOrDefault(x => x.IdArticle == di.IdArticle && x.IdSite == stockMouvement.IdSiteFrom);
                var articleSiteTo = db.ArticleSites.FirstOrDefault(x => x.IdArticle == di.IdArticle && x.IdSite == stockMouvement.IdSiteTo);
                articleSiteFrom.QteStock += di.Qte;
                articleSiteTo.QteStock -= di.Qte;
            }

            db.StockMouvementItems.RemoveRange(stockMouvement.StockMouvementItems);

            db.StockMouvements.Remove(stockMouvement);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/StockMouvements(5)/StockMouvementItems
        [EnableQuery]
        public IQueryable<StockMouvementItem> GetStockMouvementItems([FromODataUri] Guid key)
        {
            return db.StockMouvements.Where(m => m.Id == key).SelectMany(m => m.StockMouvementItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StockMouvementExists(Guid key)
        {
            return db.StockMouvements.Count(e => e.Id == key) > 0;
        }
    }
}
