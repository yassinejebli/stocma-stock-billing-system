using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    builder.EntitySet<FakeFactureF>("FakeFactureFs");
    builder.EntitySet<FakeFactureFItem>("FakeFactureFItems"); 
    builder.EntitySet<Fournisseur>("Fournisseurs"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FakeFactureFsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/FakeFactureFs
        [EnableQuery]
        public IQueryable<FakeFactureF> GetFakeFactureFs()
        {
            return db.FakeFacturesF;
        }

        // GET: odata/FakeFactureFs(5)
        [EnableQuery]
        public SingleResult<FakeFactureF> GetFakeFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FakeFacturesF.Where(fakeFactureF => fakeFactureF.Id == key));
        }

        // PUT: odata/FakeFactureFs(5)
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<FakeFactureF> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FakeFactureF fakeFactureF = db.FakeFacturesF.Find(key);
            if (fakeFactureF == null)
            {
                return NotFound();
            }

            patch.Put(fakeFactureF);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FakeFactureFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(fakeFactureF);
        }

        // POST: odata/FakeFactureFs
        public IHttpActionResult Post(FakeFactureF fakeFactureF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FakeFacturesF.Add(fakeFactureF);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FakeFactureFExists(fakeFactureF.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(fakeFactureF);
        }

        // PATCH: odata/FakeFactureFs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<FakeFactureF> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FakeFactureF fakeFactureF = db.FakeFacturesF.Find(key);
            if (fakeFactureF == null)
            {
                return NotFound();
            }

            patch.Patch(fakeFactureF);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FakeFactureFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(fakeFactureF);
        }

        // DELETE: odata/FakeFactureFs(5)
        public IHttpActionResult Delete([FromODataUri] Guid key)
        {
            FakeFactureF fakeFactureF = db.FakeFacturesF.Find(key);
            if (fakeFactureF == null)
            {
                return NotFound();
            }

            db.FakeFacturesF.Remove(fakeFactureF);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/FakeFactureFs(5)/FakeFactureFItems
        [EnableQuery]
        public IQueryable<FakeFactureFItem> GetFakeFactureFItems([FromODataUri] Guid key)
        {
            return db.FakeFacturesF.Where(m => m.Id == key).SelectMany(m => m.FakeFactureFItems);
        }

        // GET: odata/FakeFactureFs(5)/Fournisseur
        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FakeFacturesF.Where(m => m.Id == key).Select(m => m.Fournisseur));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FakeFactureFExists(Guid key)
        {
            return db.FakeFacturesF.Count(e => e.Id == key) > 0;
        }
    }
}
