using System;
using System.Collections.Generic;
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
    public class TypeDepensesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery]
        public IQueryable<TypeDepense> GetTypeDepenses()
        {
            return (IQueryable<TypeDepense>)this.db.TypeDepenses;
        }

        [EnableQuery]
        public SingleResult<TypeDepense> GetTypeDepense([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypeDepense>(this.db.TypeDepenses.Where<TypeDepense>((Expression<Func<TypeDepense, bool>>)(typeDepense => typeDepense.Id == key)));
        }

        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<TypeDepense> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            TypeDepense typeDepense = await this.db.TypeDepenses.FindAsync((object)key);
            if (typeDepense == null)
                return (IHttpActionResult)this.NotFound();
            patch.Put(typeDepense);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TypeDepenseExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<TypeDepense>(typeDepense);
        }

        public async Task<IHttpActionResult> Post(TypeDepense typeDepense)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            this.db.TypeDepenses.Add(typeDepense);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.TypeDepenseExists(typeDepense.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            return (IHttpActionResult)this.Created<TypeDepense>(typeDepense);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<TypeDepense> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            TypeDepense typeDepense = await this.db.TypeDepenses.FindAsync((object)key);
            if (typeDepense == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(typeDepense);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TypeDepenseExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<TypeDepense>(typeDepense);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            TypeDepense async = await this.db.TypeDepenses.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();
            this.db.TypeDepenses.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool TypeDepenseExists(Guid key)
        {
            return this.db.TypeDepenses.Count<TypeDepense>((Expression<Func<TypeDepense, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
