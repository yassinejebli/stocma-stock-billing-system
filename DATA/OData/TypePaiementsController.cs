// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.TypePaiementsController
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

namespace WebApplication1.DATA.OData
{
    [Authorize]
    public class TypePaiementsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery]
        public IQueryable<TypePaiement> GetTypePaiements()
        {
            return (IQueryable<TypePaiement>)this.db.TypePaiements;
        }

        [EnableQuery]
        public SingleResult<TypePaiement> GetTypePaiement([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypePaiement>(this.db.TypePaiements.Where<TypePaiement>((Expression<Func<TypePaiement, bool>>)(typePaiement => typePaiement.Id == key)));
        }

        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<TypePaiement> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            TypePaiement typePaiement = await this.db.TypePaiements.FindAsync((object)key);
            if (typePaiement == null)
                return (IHttpActionResult)this.NotFound();
            patch.Put(typePaiement);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TypePaiementExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<TypePaiement>(typePaiement);
        }

        public async Task<IHttpActionResult> Post(TypePaiement typePaiement)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            this.db.TypePaiements.Add(typePaiement);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.TypePaiementExists(typePaiement.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            return (IHttpActionResult)this.Created<TypePaiement>(typePaiement);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<TypePaiement> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            TypePaiement typePaiement = await this.db.TypePaiements.FindAsync((object)key);
            if (typePaiement == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(typePaiement);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TypePaiementExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<TypePaiement>(typePaiement);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            TypePaiement async = await this.db.TypePaiements.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();
            this.db.TypePaiements.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public IQueryable<Paiement> GetPaiements([FromODataUri] Guid key)
        {
            return this.db.TypePaiements.Where<TypePaiement>((Expression<Func<TypePaiement, bool>>)(m => m.Id == key)).SelectMany<TypePaiement, Paiement>((Expression<Func<TypePaiement, IEnumerable<Paiement>>>)(m => m.Paiements));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool TypePaiementExists(Guid key)
        {
            return this.db.TypePaiements.Count<TypePaiement>((Expression<Func<TypePaiement, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
