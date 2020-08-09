using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using WebGrease.Css.Extensions;

namespace WebApplication1.DATA.OData
{
    [Authorize]
    public class InventairesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Inventaire> GetInventaires()
        {
            return (IQueryable<Inventaire>)this.db.Inventaires.OrderByDescending(x => x.Date);
        }

        [EnableQuery]
        public SingleResult<Inventaire> GetInventaire([FromODataUri] Guid key)
        {
            return SingleResult.Create<Inventaire>(this.db.Inventaires.Where<Inventaire>((Expression<Func<Inventaire, bool>>)(inventaire => inventaire.Id == key)));
        }

        //[EnableQuery]
        //public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Inventaire newInventaire)
        //{
        //    if (!this.ModelState.IsValid)
        //        return (IHttpActionResult)this.BadRequest(this.ModelState);
        //    Inventaire inventaire = await this.db.Inventaires.FindAsync((object)key);
        //    if (inventaire == null)
        //        return (IHttpActionResult)this.NotFound();

        //    inventaire.Date = newInventaire.Date;
        //    inventaire.Titre = newInventaire.Titre;
        //    //-----------------------------------------------Updating document items
        //    db.InventaireItems.RemoveRange(inventaire.InventaireItems);
        //    db.InventaireItems.AddRange(newInventaire.InventaireItems);
        //    try
        //    {
        //        int num = await this.db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        if (!this.InventaireExists(key))
        //            return (IHttpActionResult)this.NotFound();
        //        throw;
        //    }
        //    var inventaireWithItems = db.Inventaires.Where(x => x.Id == inventaire.Id);
        //    return Content(HttpStatusCode.Created, SingleResult.Create(inventaireWithItems));
        //}

        [EnableQuery]
        public async Task<IHttpActionResult> Post(Inventaire inventaire)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);

            //Updating stock
            inventaire.InventaireItems.ForEach(x =>
            {
                var articleSite = db.ArticleSites.FirstOrDefault(y => y.IdSite == inventaire.IdSite && x.IdArticle == y.IdArticle);
                articleSite.QteStock = x.QteStockReel;
                //articleSite.Article.IdCategorie = "";
            });

            this.db.Inventaires.Add(inventaire);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.InventaireExists(inventaire.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            var inventaireWithItems = db.Inventaires.Where(x => x.Id == inventaire.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(inventaireWithItems));
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Inventaire> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            Inventaire inventaire = await this.db.Inventaires.FindAsync((object)key);
            if (inventaire == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(inventaire);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.InventaireExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<Inventaire>(inventaire);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Inventaire async = await this.db.Inventaires.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();

            db.InventaireItems.RemoveRange(async.InventaireItems);
            this.db.Inventaires.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public IQueryable<InventaireItem> GetInventaireITems([FromODataUri] Guid key)
        {
            return this.db.Inventaires.Where(m => m.Id == key).SelectMany(m => m.InventaireItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool InventaireExists(Guid key)
        {
            return this.db.Inventaires.Count<Inventaire>((Expression<Func<Inventaire, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
