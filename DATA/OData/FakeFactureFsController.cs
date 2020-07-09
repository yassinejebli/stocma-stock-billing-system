using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.AspNet.OData;

namespace WebApplication1.DATA.OData
{
    [Authorize]
    public class FakeFactureFsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/FakeFactureFs
        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<FakeFactureF> GetFakeFactureFs()
        {
            return db.FakeFacturesF.OrderByDescending(x=>x.Date);
        }

        // GET: odata/FakeFactureFs(5)
        [EnableQuery]
        public SingleResult<FakeFactureF> GetFakeFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FakeFacturesF.Where(fakeFactureF => fakeFactureF.Id == key));
        }

        // PUT: odata/FakeFactureFs(5)
        [EnableQuery]
        public IHttpActionResult Put([FromODataUri] Guid key, FakeFactureF newFakeFactureF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FakeFactureF fakeFactureF = db.FakeFacturesF.Find(key);
            if (fakeFactureF == null)
            {
                return NotFound();
            }

            fakeFactureF.Date = newFakeFactureF.Date;
            fakeFactureF.NumBon = newFakeFactureF.NumBon;
            fakeFactureF.IdTypePaiement = newFakeFactureF.IdTypePaiement;
            fakeFactureF.Comment = newFakeFactureF.Comment;

            //----------------------------------------------Updating QteStock
            foreach (var fiOld in fakeFactureF.FakeFactureFItems)
            {
                var article = db.ArticleFactures.Find(fiOld.IdArticleFacture);
                article.QteStock -= fiOld.Qte;
            }
            foreach (var fiNew in newFakeFactureF.FakeFactureFItems)
            {
                var article = db.ArticleFactures.Find(fiNew.IdArticleFacture);
                article.QteStock += fiNew.Qte;
            }

            //-----------------------------------------------Updating document items
            db.FakeFactureFItems.RemoveRange(fakeFactureF.FakeFactureFItems);
            db.FakeFactureFItems.AddRange(newFakeFactureF.FakeFactureFItems);

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
            var fakeFactureFWithItems = db.FakeFacturesF.Where(x => x.Id == fakeFactureF.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(fakeFactureFWithItems));
        }

        [EnableQuery]
        public IHttpActionResult Post(FakeFactureF fakeFactureF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-------------------------------------------updating QteStock
            foreach (var fi in fakeFactureF.FakeFactureFItems)
            {
                var article = db.ArticleFactures.Find(fi.IdArticleFacture);
                article.QteStock += fi.Qte;
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

            var fakeFactureFWithItems = db.FakeFacturesF.Where(x => x.Id == fakeFactureF.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(fakeFactureFWithItems));
        }

        // PATCH: odata/FakeFactureFs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<FakeFactureF> patch)
        {
            

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
            foreach (var fi in fakeFactureF.FakeFactureFItems)
            {
                var article = db.ArticleFactures.Find(fi.IdArticleFacture);
                article.QteStock -= fi.Qte;
            }

            db.FakeFactureFItems.RemoveRange(fakeFactureF.FakeFactureFItems);
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
