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
    builder.EntitySet<Categorie>("Categories");
    builder.EntitySet<Article>("Articles"); 
    builder.EntitySet<Famille>("Familles"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CategoriesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/Categories
        [EnableQuery]
        public IQueryable<Categorie> GetCategories()
        {
            return db.Categories;
        }

        // GET: odata/Categories(5)
        [EnableQuery]
        public SingleResult<Categorie> GetCategorie([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Categories.Where(categorie => categorie.Id == key));
        }

        // PUT: odata/Categories(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Categorie> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Categorie categorie = await db.Categories.FindAsync(key);
            if (categorie == null)
            {
                return NotFound();
            }

            patch.Put(categorie);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(categorie);
        }

        // POST: odata/Categories
        public async Task<IHttpActionResult> Post(Categorie categorie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(categorie);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategorieExists(categorie.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(categorie);
        }

        // PATCH: odata/Categories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Categorie> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Categorie categorie = await db.Categories.FindAsync(key);
            if (categorie == null)
            {
                return NotFound();
            }

            patch.Patch(categorie);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(categorie);
        }

        // DELETE: odata/Categories(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Categorie categorie = await db.Categories.FindAsync(key);
            if (categorie == null)
            {
                return NotFound();
            }

            db.Categories.Remove(categorie);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Categories(5)/Articles
        [EnableQuery]
        public IQueryable<Article> GetArticles([FromODataUri] Guid key)
        {
            return db.Categories.Where(m => m.Id == key).SelectMany(m => m.Articles);
        }

        // GET: odata/Categories(5)/Famille
        [EnableQuery]
        public SingleResult<Famille> GetFamille([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Categories.Where(m => m.Id == key).Select(m => m.Famille));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategorieExists(Guid key)
        {
            return db.Categories.Count(e => e.Id == key) > 0;
        }
    }
}
