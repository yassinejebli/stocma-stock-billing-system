// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.FakeFacturesController
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using WebApplication1.Generators;

namespace WebApplication1.DATA.OData
{
    [Authorize]
    public class FakeFacturesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<FakeFacture> GetFakeFactures()
        {
            return (IQueryable<FakeFacture>)this.db.FakeFactures.OrderByDescending(x=>x.Date);
        }

        [EnableQuery]
        public SingleResult<FakeFacture> GetFakeFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<FakeFacture>(this.db.FakeFactures.Where<FakeFacture>((Expression<Func<FakeFacture, bool>>)(fakeFacture => fakeFacture.Id == key)));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, FakeFacture newFakeFacture)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            FakeFacture fakeFacture = await this.db.FakeFactures.FindAsync((object)key);
            if (fakeFacture == null)
                return (IHttpActionResult)this.NotFound();

            db.FakeFactureItems.RemoveRange(fakeFacture.FakeFactureItems);
            db.FakeFactureItems.AddRange(newFakeFacture.FakeFactureItems);
            fakeFacture.Date = newFakeFacture.Date;
            fakeFacture.Ref = newFakeFacture.Ref;
            fakeFacture.Note = newFakeFacture.Note;
            fakeFacture.IdTypePaiement = newFakeFacture.IdTypePaiement;
            fakeFacture.WithDiscount = newFakeFacture.WithDiscount;
            fakeFacture.Comment = newFakeFacture.Comment;
            fakeFacture.ClientName = newFakeFacture.ClientName;

            var numBonGenerator = new DocNumberGenerator();

            fakeFacture.NumBon = numBonGenerator.getNumDocByCompany(newFakeFacture.Ref - 1, newFakeFacture.Date);

            //----------------------------------------------Updating QteStock
            foreach (var fiOld in fakeFacture.FakeFactureItems)
            {
                var article = db.ArticleFactures.Find(fiOld.IdArticleFacture);
                article.QteStock += fiOld.Qte;
            }
            foreach (var fiNew in newFakeFacture.FakeFactureItems)
            {
                var article = db.ArticleFactures.Find(fiNew.IdArticleFacture);
                article.QteStock -= fiNew.Qte;
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FakeFactureExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            var fakeFactureWithItems = db.FakeFactures.Where(x => x.Id == fakeFacture.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(fakeFactureWithItems));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Post(FakeFacture fakeFacture)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);


            var numBonGenerator = new DocNumberGenerator();
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.FakeFactures.Where(x => x.Date.Year == currentYear).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 0;
            fakeFacture.Ref = lastRef + 1;
            fakeFacture.NumBon = numBonGenerator.getNumDocByCompany(lastRef, fakeFacture.Date);
            //-------------------------------------------updating QteStock
            foreach (var fi in fakeFacture.FakeFactureItems)
            {
                var article = db.ArticleFactures.Find(fi.IdArticleFacture);
                article.QteStock -= fi.Qte;
            }

            this.db.FakeFactures.Add(fakeFacture);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.FakeFactureExists(fakeFacture.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            var fakeFactureWithItems = db.FakeFactures.Where(x => x.Id == fakeFacture.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(fakeFactureWithItems));
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<FakeFacture> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            FakeFacture fakeFacture = await this.db.FakeFactures.FindAsync((object)key);
            if (fakeFacture == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(fakeFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FakeFactureExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<FakeFacture>(fakeFacture);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            FakeFacture async = await this.db.FakeFactures.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();

            //--------------------------updating QteStock
            foreach (var fi in async.FakeFactureItems)
            {
                var article = db.ArticleFactures.Find(fi.IdArticleFacture);
                article.QteStock += fi.Qte;
            }

            db.FakeFactureItems.RemoveRange(async.FakeFactureItems);
            db.FakeFactures.Remove(async);
            await db.SaveChangesAsync();
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.FakeFactures.Where<FakeFacture>((Expression<Func<FakeFacture, bool>>)(m => m.Id == key)).Select<FakeFacture, Client>((Expression<Func<FakeFacture, Client>>)(m => m.Client)));
        }

        [EnableQuery]
        public IQueryable<FakeFactureItem> GetFakeFactureItems([FromODataUri] Guid key)
        {
            return this.db.FakeFactures.Where<FakeFacture>((Expression<Func<FakeFacture, bool>>)(m => m.Id == key)).SelectMany<FakeFacture, FakeFactureItem>((Expression<Func<FakeFacture, IEnumerable<FakeFactureItem>>>)(m => m.FakeFactureItems));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool FakeFactureExists(Guid key)
        {
            return this.db.FakeFactures.Count<FakeFacture>((Expression<Func<FakeFacture, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
