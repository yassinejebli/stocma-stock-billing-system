using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using WebApplication1.DATA;

namespace WebApplication1.DATA.OData
{
    /*
    La classe WebApiConfig peut exiger d'autres modifications pour ajouter un itinéraire à ce contrôleur. Fusionnez ces instructions dans la méthode Register de la classe WebApiConfig, le cas échéant. Les URL OData sont sensibles à la casse.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApplication1.DATA;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Article>("Articles");
    builder.EntitySet<BonAvoirCItem>("BonAvoirCItems"); 
    builder.EntitySet<BonAvoirItem>("BonAvoirItems"); 
    builder.EntitySet<BonCommandeItem>("BonCommandeItems"); 
    builder.EntitySet<BonLivraisonItem>("BonLivraisonItems"); 
    builder.EntitySet<BonReceptionItem>("BonReceptionItems"); 
    builder.EntitySet<Categorie>("Categories"); 
    builder.EntitySet<DevisItem>("DevisItems"); 
    builder.EntitySet<FactureItem>("FactureItems"); 
    builder.EntitySet<TarifItem>("TarifItems"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ArticlesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/Articles
        [EnableQuery]
        public IQueryable<Article> GetArticles()
        {
            return db.Articles;
        }

        [EnableQuery]
        public IQueryable<Article> ArticlesGaz()
        {
            return db.Articles.Where(x => x.Ref.StartsWith("GZ"));
        }

        // GET: odata/Articles(5)
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Articles.Where(article => article.Id == key));
        }

        // PUT: odata/Articles(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Article> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Article article = await db.Articles.FindAsync(key);
            if (article == null)
            {
                return NotFound();
            }

            patch.Put(article);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(article);
        }

        // POST: odata/Articles
        public async Task<IHttpActionResult> Post(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Articles.Add(article);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArticleExists(article.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(article);
        }

        // PATCH: odata/Articles(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Article> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Article article = await db.Articles.FindAsync(key);
            if (article == null)
            {
                return NotFound();
            }

            patch.Patch(article);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(article);
        }

        // DELETE: odata/Articles(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Article article = await db.Articles.FindAsync(key);
            if (article == null)
            {
                return NotFound();
            }

            db.Articles.Remove(article);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Articles(5)/BonAvoirCItems
        [EnableQuery]
        public IQueryable<BonAvoirCItem> GetBonAvoirCItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.BonAvoirCItems);
        }

        // GET: odata/Articles(5)/BonAvoirItems
        [EnableQuery]
        public IQueryable<BonAvoirItem> GetBonAvoirItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.BonAvoirItems);
        }

        // GET: odata/Articles(5)/BonCommandeItems
        [EnableQuery]
        public IQueryable<BonCommandeItem> GetBonCommandeItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.BonCommandeItems);
        }

        // GET: odata/Articles(5)/BonLivraisonItems
        [EnableQuery]
        public IQueryable<BonLivraisonItem> GetBonLivraisonItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.BonLivraisonItems);
        }

        // GET: odata/Articles(5)/BonReceptionItems
        [EnableQuery]
        public IQueryable<BonReceptionItem> GetBonReceptionItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.BonReceptionItems);
        }

        // GET: odata/Articles(5)/Categorie
        [EnableQuery]
        public SingleResult<Categorie> GetCategorie([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Articles.Where(m => m.Id == key).Select(m => m.Categorie));
        }

        // GET: odata/Articles(5)/DevisItems
        [EnableQuery]
        public IQueryable<DevisItem> GetDevisItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.DevisItems);
        }

        // GET: odata/Articles(5)/FactureItems
        [EnableQuery]
        public IQueryable<FactureItem> GetFactureItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.FactureItems);
        }

        // GET: odata/Articles(5)/TarifItems
        [EnableQuery]
        public IQueryable<TarifItem> GetTarifItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.TarifItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(Guid key)
        {
            return db.Articles.Count(e => e.Id == key) > 0;
        }
    }
}
