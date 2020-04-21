// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.BonLivraisonsController
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
using System.Web.Http.OData;
using System.Web.UI.WebControls;
using WebApplication1.Generators;
using WebApplication1.Managers;
using WebApplication1.PaimentManager;

namespace WebApplication1.DATA.OData
{
    public class BonLivraisonsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();
        private PaymentManager pm = new PaymentManager();
        private MarginManager mm = new MarginManager();


        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonLivraison> GetBonLivraisons()
        {
            return db.BonLivraisons.OrderByDescending(x=>x.Date);
        }

        [EnableQuery]
        public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
        {
            return SingleResult.Create(this.db.BonLivraisons.Where<BonLivraison>((Expression<Func<BonLivraison, bool>>)(bonLivraison => bonLivraison.Id == key)));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, BonLivraison newBonLivraison)
        {
            BonLivraison bonLivraison = await this.db.BonLivraisons.FindAsync(key);
            if (bonLivraison == null)
                return NotFound();
            
            bonLivraison.Date = newBonLivraison.Date;
            bonLivraison.TypeReglement = newBonLivraison.TypeReglement;
            bonLivraison.User = newBonLivraison.User;
            bonLivraison.Note = newBonLivraison.Note;
            db.BonLivraisonItems.RemoveRange(bonLivraison.BonLivraisonItems);
            bonLivraison.BonLivraisonItems = newBonLivraison.BonLivraisonItems;
            bonLivraison.ModificationDate = DateTime.Now;
            bonLivraison.Ref = newBonLivraison.Ref;
            bonLivraison.NumBon = newBonLivraison.NumBon;

            try
            {
                await this.db.SaveChangesAsync();
                await pm.UpdatePaiementBL(key);
                await mm.UpdateMarginBL(key); 
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonLivraisonExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }

            var bonLivraisonWithItems = db.BonLivraisons.Where(x => x.Id == bonLivraison.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(bonLivraisonWithItems));
            //return (IHttpActionResult)this.Created<BonLivraison>(bonLivraison);
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Post(BonLivraison bonLivraison)
        {
            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);
            var user = db.Companies.FirstOrDefault();
            var numBonGenerator = new DocNumberGenerator();

            var userCompanyName = user.Name.ToUpper();
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.BonLivraisons.Where(x=>x.Date.Year == currentYear).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 1;
            bonLivraison.Ref = lastRef + 1;
            bonLivraison.NumBon = numBonGenerator.getNumDocByCompany(lastRef, userCompanyName, bonLivraison.Date);
            this.db.BonLivraisons.Add(bonLivraison);
            try
            {
                int num = await this.db.SaveChangesAsync();
                await pm.UpdatePaiementBL(bonLivraison.Id);
                await mm.UpdateMarginBL(bonLivraison.Id);
            }
            catch (DbUpdateException ex)
            {
                if (this.BonLivraisonExists(bonLivraison.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            var bonLivraisonWithItems = db.BonLivraisons.Where(x=>x.Id == bonLivraison.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(bonLivraisonWithItems));

            //return (IHttpActionResult)this.Created(bonLivraisonWithItems);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonLivraison> patch)
        {
            this.Validate<BonLivraison>(patch.GetEntity());
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            BonLivraison bonLivraison = await this.db.BonLivraisons.FindAsync((object)key);
            if (bonLivraison == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(bonLivraison);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonLivraisonExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<BonLivraison>(bonLivraison);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            BonLivraison async = await this.db.BonLivraisons.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();
            db.Paiements.RemoveRange(async.Paiements);
            db.BonLivraisonItems.RemoveRange(async.BonLivraisonItems);
            db.BonLivraisons.Remove(async);
            int num = await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public IQueryable<BonLivraisonItem> GetBonLivraisonItems([FromODataUri] Guid key)
        {
            return this.db.BonLivraisons.Where<BonLivraison>((Expression<Func<BonLivraison, bool>>)(m => m.Id == key)).SelectMany<BonLivraison, BonLivraisonItem>((Expression<Func<BonLivraison, IEnumerable<BonLivraisonItem>>>)(m => m.BonLivraisonItems));
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.BonLivraisons.Where<BonLivraison>((Expression<Func<BonLivraison, bool>>)(m => m.Id == key)).Select<BonLivraison, Client>((Expression<Func<BonLivraison, Client>>)(m => m.Client)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool BonLivraisonExists(Guid key)
        {
            return this.db.BonLivraisons.Count<BonLivraison>((Expression<Func<BonLivraison, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
