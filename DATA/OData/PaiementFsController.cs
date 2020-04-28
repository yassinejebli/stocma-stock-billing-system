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
    builder.EntitySet<PaiementF>("PaiementFs");
    builder.EntitySet<BonReception>("BonReceptions"); 
    builder.EntitySet<FactureF>("FactureFs"); 
    builder.EntitySet<Fournisseur>("Fournisseurs"); 
    builder.EntitySet<TypePaiement>("TypePaiements"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PaiementFsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/PaiementFs
        [EnableQuery]
        public IQueryable<PaiementF> GetPaiementFs()
        {
            return db.PaiementFs;
        }

        // GET: odata/PaiementFs(5)
        [EnableQuery]
        public SingleResult<PaiementF> GetPaiementF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.PaiementFs.Where(paiementF => paiementF.Id == key));
        }

        // PUT: odata/PaiementFs(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<PaiementF> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PaiementF paiementF = await db.PaiementFs.FindAsync(key);
            if (paiementF == null)
            {
                return NotFound();
            }

            patch.Put(paiementF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaiementFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(paiementF);
        }

        // POST: odata/PaiementFs
        public async Task<IHttpActionResult> Post(PaiementF paiementF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PaiementFs.Add(paiementF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PaiementFExists(paiementF.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(paiementF);
        }

        // PATCH: odata/PaiementFs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<PaiementF> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PaiementF paiementF = await db.PaiementFs.FindAsync(key);
            if (paiementF == null)
            {
                return NotFound();
            }

            patch.Patch(paiementF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaiementFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(paiementF);
        }

        // DELETE: odata/PaiementFs(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            PaiementF paiementF = await db.PaiementFs.FindAsync(key);
            if (paiementF == null)
            {
                return NotFound();
            }

            db.PaiementFs.Remove(paiementF);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/PaiementFs(5)/BonReception
        [EnableQuery]
        public SingleResult<BonReception> GetBonReception([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.PaiementFs.Where(m => m.Id == key).Select(m => m.BonReception));
        }

        // GET: odata/PaiementFs(5)/FactureF
        [EnableQuery]
        public SingleResult<FactureF> GetFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.PaiementFs.Where(m => m.Id == key).Select(m => m.FactureF));
        }

        // GET: odata/PaiementFs(5)/Fournisseur
        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.PaiementFs.Where(m => m.Id == key).Select(m => m.Fournisseur));
        }

        // GET: odata/PaiementFs(5)/TypePaiement
        [EnableQuery]
        public SingleResult<TypePaiement> GetTypePaiement([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.PaiementFs.Where(m => m.Id == key).Select(m => m.TypePaiement));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaiementFExists(Guid key)
        {
            return db.PaiementFs.Count(e => e.Id == key) > 0;
        }
    }
}
