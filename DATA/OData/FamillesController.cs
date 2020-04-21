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
    builder.EntitySet<Famille>("Familles");
    builder.EntitySet<Categorie>("Categories"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FamillesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/Familles
        [EnableQuery]
        public IQueryable<Famille> GetFamilles()
        {
            return db.Familles;
        }

        // GET: odata/Familles(5)
        [EnableQuery]
        public SingleResult<Famille> GetFamille([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Familles.Where(famille => famille.Id == key));
        }

        // PUT: odata/Familles(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Famille> patch)
        {
            
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Famille famille = await db.Familles.FindAsync(key);
            
            if (famille == null)
            {
                return NotFound();
            }

            patch.Put(famille);

            try
            {
                await db.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!FamilleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(famille);
        }

        // POST: odata/Familles
        public async Task<IHttpActionResult> Post(Famille famille)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Familles.Add(famille);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FamilleExists(famille.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(famille);
        }

        // PATCH: odata/Familles(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Famille> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Famille famille = await db.Familles.FindAsync(key);
            if (famille == null)
            {
                return NotFound();
            }

            patch.Patch(famille);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(famille);
        }

        // DELETE: odata/Familles(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Famille famille = await db.Familles.FindAsync(key);
            if (famille == null)
            {
                return NotFound();
            }

            db.Familles.Remove(famille);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Familles(5)/Categories
        [EnableQuery]
        public IQueryable<Categorie> GetCategories([FromODataUri] Guid key)
        {
            return db.Familles.Where(m => m.Id == key).SelectMany(m => m.Categories);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FamilleExists(Guid key)
        {
            return db.Familles.Count(e => e.Id == key) > 0;
        }
    }
}
