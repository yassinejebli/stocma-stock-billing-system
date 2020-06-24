// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.FournisseursController
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
    public class FournisseursController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Fournisseur> GetFournisseurs()
        {
            return (IQueryable<Fournisseur>)this.db.Fournisseurs.OrderBy(x => new { x.Disabled, x.Name });
        }

        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create<Fournisseur>(this.db.Fournisseurs.Where<Fournisseur>((Expression<Func<Fournisseur, bool>>)(fournisseur => fournisseur.Id == key)));
        }

        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Fournisseur> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            Fournisseur fournisseur = await this.db.Fournisseurs.FindAsync((object)key);
            if (fournisseur == null)
                return (IHttpActionResult)this.NotFound();
            patch.Put(fournisseur);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FournisseurExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<Fournisseur>(fournisseur);
        }

        public async Task<IHttpActionResult> Post(Fournisseur fournisseur)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            this.db.Fournisseurs.Add(fournisseur);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.FournisseurExists(fournisseur.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            return (IHttpActionResult)this.Created<Fournisseur>(fournisseur);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Fournisseur> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            Fournisseur fournisseur = await this.db.Fournisseurs.FindAsync((object)key);
            if (fournisseur == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(fournisseur);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FournisseurExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<Fournisseur>(fournisseur);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Fournisseur async = await this.db.Fournisseurs.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();
            this.db.Fournisseurs.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public IQueryable<BonReception> GetBonReceptions([FromODataUri] Guid key)
        {
            return this.db.Fournisseurs.Where<Fournisseur>((Expression<Func<Fournisseur, bool>>)(m => m.Id == key)).SelectMany<Fournisseur, BonReception>((Expression<Func<Fournisseur, IEnumerable<BonReception>>>)(m => m.BonReceptions));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool FournisseurExists(Guid key)
        {
            return this.db.Fournisseurs.Count<Fournisseur>((Expression<Func<Fournisseur, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
