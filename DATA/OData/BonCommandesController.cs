// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.BonCommandesController
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
    public class BonCommandesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonCommande> GetBonCommandes()
        {
            return (IQueryable<BonCommande>)this.db.BonCommandes.OrderByDescending(x=>x.Date);
        }

        [EnableQuery]
        public SingleResult<BonCommande> GetBonCommande([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonCommande>(this.db.BonCommandes.Where<BonCommande>((Expression<Func<BonCommande, bool>>)(bonCommande => bonCommande.Id == key)));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, BonCommande newBonCommande)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            BonCommande bonCommande = await this.db.BonCommandes.FindAsync((object)key);
            if (bonCommande == null)
                return (IHttpActionResult)this.NotFound();

            db.BonCommandeItems.RemoveRange(bonCommande.BonCommandeItems);
            db.BonCommandeItems.AddRange(newBonCommande.BonCommandeItems);
            bonCommande.Date = newBonCommande.Date;
            bonCommande.Ref = newBonCommande.Ref;
            bonCommande.Note = newBonCommande.Note;

            var numBonGenerator = new DocNumberGenerator();

            bonCommande.NumBon = numBonGenerator.getNumDocByCompany(bonCommande.Ref - 1, newBonCommande.Date);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonCommandeExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            var bonCommandeWithItems = db.BonCommandes.Where(x => x.Id == bonCommande.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(bonCommandeWithItems));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Post(BonCommande bonCommande)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);

            var numBonGenerator = new DocNumberGenerator();
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.BonCommandes.Where(x => x.Date.Year == currentYear).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 0;
            bonCommande.Ref = lastRef + 1;
            bonCommande.NumBon = numBonGenerator.getNumDocByCompany(lastRef, bonCommande.Date);

            this.db.BonCommandes.Add(bonCommande);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.BonCommandeExists(bonCommande.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            var bonCommandeWithItems = db.BonCommandes.Where(x => x.Id == bonCommande.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(bonCommandeWithItems));
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonCommande> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            BonCommande bonCommande = await this.db.BonCommandes.FindAsync((object)key);
            if (bonCommande == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(bonCommande);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonCommandeExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<BonCommande>(bonCommande);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            BonCommande bonCommande = await this.db.BonCommandes.FindAsync((object)key);
            if (bonCommande == null)
                return (IHttpActionResult)this.NotFound();

            db.BonCommandeItems.RemoveRange(bonCommande.BonCommandeItems);
            db.BonCommandes.Remove(bonCommande);
            await db.SaveChangesAsync();
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public IQueryable<BonCommandeItem> GetBonCommandeItems([FromODataUri] Guid key)
        {
            return this.db.BonCommandes.Where<BonCommande>((Expression<Func<BonCommande, bool>>)(m => m.Id == key)).SelectMany<BonCommande, BonCommandeItem>((Expression<Func<BonCommande, IEnumerable<BonCommandeItem>>>)(m => m.BonCommandeItems));
        }

        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create<Fournisseur>(this.db.BonCommandes.Where<BonCommande>((Expression<Func<BonCommande, bool>>)(m => m.Id == key)).Select<BonCommande, Fournisseur>((Expression<Func<BonCommande, Fournisseur>>)(m => m.Fournisseur)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool BonCommandeExists(Guid key)
        {
            return this.db.BonCommandes.Count<BonCommande>((Expression<Func<BonCommande, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
