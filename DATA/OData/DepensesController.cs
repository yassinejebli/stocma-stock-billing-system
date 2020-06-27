using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;

namespace WebApplication1.DATA.OData
{
    [Authorize]
    public class DepensesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Depense> GetDepenses()
        {
            return (IQueryable<Depense>)this.db.Depenses.OrderByDescending(x => x.Date);
        }

        [EnableQuery]
        public SingleResult<Depense> GetDepense([FromODataUri] Guid key)
        {
            return SingleResult.Create<Depense>(this.db.Depenses.Where<Depense>((Expression<Func<Depense, bool>>)(depense => depense.Id == key)));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Depense newDepense)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            Depense depense = await this.db.Depenses.FindAsync((object)key);
            if (depense == null)
                return (IHttpActionResult)this.NotFound();

            depense.Date = newDepense.Date;
            depense.Titre = newDepense.Titre;
            //-----------------------------------------------Updating document items
            db.DepenseItems.RemoveRange(depense.DepenseItems);
            db.DepenseItems.AddRange(newDepense.DepenseItems);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.DepenseExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            var depenseWithItems = db.Depenses.Where(x => x.Id == depense.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(depenseWithItems));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Post(Depense depense)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            this.db.Depenses.Add(depense);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.DepenseExists(depense.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            var depenseWithItems = db.Depenses.Where(x => x.Id == depense.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(depenseWithItems));
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Depense> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            Depense depense = await this.db.Depenses.FindAsync((object)key);
            if (depense == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(depense);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.DepenseExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<Depense>(depense);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Depense async = await this.db.Depenses.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();

            db.DepenseItems.RemoveRange(async.DepenseItems);
            this.db.Depenses.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public IQueryable<DepenseItem> GetDepenseITems([FromODataUri] Guid key)
        {
            return this.db.Depenses.Where(m => m.Id == key).SelectMany(m => m.DepenseItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool DepenseExists(Guid key)
        {
            return this.db.Depenses.Count<Depense>((Expression<Func<Depense, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
