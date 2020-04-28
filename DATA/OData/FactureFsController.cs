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
using Microsoft.AspNet.OData;
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
    builder.EntitySet<FactureF>("FactureFs");
    builder.EntitySet<BonReception>("BonReceptions"); 
    builder.EntitySet<FactureFItem>("FactureFItems"); 
    builder.EntitySet<Fournisseur>("Fournisseurs"); 
    builder.EntitySet<PaiementF>("PaiementFs"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FactureFsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/FactureFs
        [EnableQueryAttribute(MaxExpansionDepth = 3)]
        public IQueryable<FactureF> GetFactureFs()
        {
            return db.FactureFs;
        }

        // GET: odata/FactureFs(5)
        [EnableQueryAttribute(MaxExpansionDepth = 3)]
        public SingleResult<FactureF> GetFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FactureFs.Where(factureF => factureF.Id == key));
        }

        // PUT: odata/FactureFs(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<FactureF> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FactureF factureF = await db.FactureFs.FindAsync(key);
            if (factureF == null)
            {
                return NotFound();
            }

            patch.Put(factureF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactureFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(factureF);
        }

        // POST: odata/FactureFs
        public async Task<IHttpActionResult> Post(FactureF factureF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FactureFs.Add(factureF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FactureFExists(factureF.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(factureF);
        }

        // PATCH: odata/FactureFs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<FactureF> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FactureF factureF = await db.FactureFs.FindAsync(key);
            if (factureF == null)
            {
                return NotFound();
            }

            patch.Patch(factureF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactureFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(factureF);
        }

        // DELETE: odata/FactureFs(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            FactureF factureF = await db.FactureFs.FindAsync(key);
            if (factureF == null)
            {
                return NotFound();
            }

            db.FactureFs.Remove(factureF);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/FactureFs(5)/BonReceptions
        [EnableQuery]
        public IQueryable<BonReception> GetBonReceptions([FromODataUri] Guid key)
        {
            return db.FactureFs.Where(m => m.Id == key).SelectMany(m => m.BonReceptions);
        }

        // GET: odata/FactureFs(5)/FactureFItems
        [EnableQuery]
        public IQueryable<FactureFItem> GetFactureFItems([FromODataUri] Guid key)
        {
            return db.FactureFs.Where(m => m.Id == key).SelectMany(m => m.FactureFItems);
        }

        // GET: odata/FactureFs(5)/Fournisseur
        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FactureFs.Where(m => m.Id == key).Select(m => m.Fournisseur));
        }

        // GET: odata/FactureFs(5)/PaiementFs
        [EnableQuery]
        public IQueryable<PaiementF> GetPaiementFs([FromODataUri] Guid key)
        {
            return db.FactureFs.Where(m => m.Id == key).SelectMany(m => m.PaiementFs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FactureFExists(Guid key)
        {
            return db.FactureFs.Count(e => e.Id == key) > 0;
        }
    }
}
