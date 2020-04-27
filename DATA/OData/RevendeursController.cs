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
using System.Web.OData;
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
    builder.EntitySet<Revendeur>("Revendeurs");
    builder.EntitySet<Client>("Clients"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RevendeursController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/Revendeurs
        [EnableQuery]
        public IQueryable<Revendeur> GetRevendeurs()
        {
            return db.Revendeurs;
        }

        // GET: odata/Revendeurs(5)
        [EnableQuery]
        public SingleResult<Revendeur> GetRevendeur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Revendeurs.Where(revendeur => revendeur.Id == key));
        }

        // PUT: odata/Revendeurs(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Revendeur> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Revendeur revendeur = await db.Revendeurs.FindAsync(key);
            if (revendeur == null)
            {
                return NotFound();
            }

            patch.Put(revendeur);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RevendeurExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(revendeur);
        }

        // POST: odata/Revendeurs
        public async Task<IHttpActionResult> Post(Revendeur revendeur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Revendeurs.Add(revendeur);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RevendeurExists(revendeur.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(revendeur);
        }

        // PATCH: odata/Revendeurs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Revendeur> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Revendeur revendeur = await db.Revendeurs.FindAsync(key);
            if (revendeur == null)
            {
                return NotFound();
            }

            patch.Patch(revendeur);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RevendeurExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(revendeur);
        }

        // DELETE: odata/Revendeurs(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Revendeur revendeur = await db.Revendeurs.FindAsync(key);
            if (revendeur == null)
            {
                return NotFound();
            }

            db.Revendeurs.Remove(revendeur);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Revendeurs(5)/Clients
        [EnableQuery]
        public IQueryable<Client> GetClients([FromODataUri] Guid key)
        {
            return db.Revendeurs.Where(m => m.Id == key).SelectMany(m => m.Clients);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RevendeurExists(Guid key)
        {
            return db.Revendeurs.Count(e => e.Id == key) > 0;
        }
    }
}
