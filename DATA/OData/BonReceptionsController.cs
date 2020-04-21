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
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApplication1.DATA;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<BonReception>("BonReceptions");
    builder.EntitySet<BonAvoir>("BonAvoirs"); 
    builder.EntitySet<BonReceptionItem>("BonReceptionItems"); 
    builder.EntitySet<FactureF>("FactureFs"); 
    builder.EntitySet<Fournisseur>("Fournisseurs"); 
    builder.EntitySet<PaiementF>("PaiementFs"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class BonReceptionsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/BonReceptions
        [EnableQuery]
        public IQueryable<BonReception> GetBonReceptions()
        {
            return db.BonReceptions;
        }

        // GET: odata/BonReceptions(5)
        [EnableQuery]
        public SingleResult<BonReception> GetBonReception([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BonReceptions.Where(bonReception => bonReception.Id == key));
        }

        // PUT: odata/BonReceptions(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<BonReception> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BonReception bonReception = await db.BonReceptions.FindAsync(key);
            if (bonReception == null)
            {
                return NotFound();
            }

            patch.Put(bonReception);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BonReceptionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(bonReception);
        }

        // POST: odata/BonReceptions
        public async Task<IHttpActionResult> Post(BonReception bonReception)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BonReceptions.Add(bonReception);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BonReceptionExists(bonReception.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(bonReception);
        }

        // PATCH: odata/BonReceptions(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonReception> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BonReception bonReception = await db.BonReceptions.FindAsync(key);
            if (bonReception == null)
            {
                return NotFound();
            }

            patch.Patch(bonReception);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BonReceptionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(bonReception);
        }

        // DELETE: odata/BonReceptions(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            BonReception bonReception = await db.BonReceptions.FindAsync(key);
            if (bonReception == null)
            {
                return NotFound();
            }

            db.BonReceptions.Remove(bonReception);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/BonReceptions(5)/BonAvoirs
        [EnableQuery]
        public IQueryable<BonAvoir> GetBonAvoirs([FromODataUri] Guid key)
        {
            return db.BonReceptions.Where(m => m.Id == key).SelectMany(m => m.BonAvoirs);
        }

        // GET: odata/BonReceptions(5)/BonReceptionItems
        [EnableQuery]
        public IQueryable<BonReceptionItem> GetBonReceptionItems([FromODataUri] Guid key)
        {
            return db.BonReceptions.Where(m => m.Id == key).SelectMany(m => m.BonReceptionItems);
        }

        // GET: odata/BonReceptions(5)/FactureF
        [EnableQuery]
        public SingleResult<FactureF> GetFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BonReceptions.Where(m => m.Id == key).Select(m => m.FactureF));
        }

        // GET: odata/BonReceptions(5)/Fournisseur
        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BonReceptions.Where(m => m.Id == key).Select(m => m.Fournisseur));
        }

        // GET: odata/BonReceptions(5)/PaiementFs
        [EnableQuery]
        public IQueryable<PaiementF> GetPaiementFs([FromODataUri] Guid key)
        {
            return db.BonReceptions.Where(m => m.Id == key).SelectMany(m => m.PaiementFs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BonReceptionExists(Guid key)
        {
            return db.BonReceptions.Count(e => e.Id == key) > 0;
        }
    }
}
