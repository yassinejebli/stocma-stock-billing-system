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
    public class PaiementFactureFFsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<PaiementFactureF> GetPaiementFactureFs()
        {
            return db.PaiementFactureFs.OrderByDescending(x =>x.Date);
        }

        [EnableQuery]
        public SingleResult<PaiementFactureF> GetPaiementFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create<PaiementFactureF>(this.db.PaiementFactureFs.Where<PaiementFactureF>((Expression<Func<PaiementFactureF, bool>>)(paiementFactureF => paiementFactureF.Id == key)));
        }

        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<PaiementFactureF> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            PaiementFactureF paiementFactureF = await this.db.PaiementFactureFs.FindAsync((object)key);
            if (paiementFactureF == null)
                return (IHttpActionResult)this.NotFound();
            patch.Put(paiementFactureF);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.PaiementFactureFExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<PaiementFactureF>(paiementFactureF);
        }

        public async Task<IHttpActionResult> Post(PaiementFactureF paiementFactureF)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            this.db.PaiementFactureFs.Add(paiementFactureF);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.PaiementFactureFExists(paiementFactureF.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            return (IHttpActionResult)this.Created<PaiementFactureF>(paiementFactureF);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<PaiementFactureF> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            PaiementFactureF paiementFactureF = await this.db.PaiementFactureFs.FindAsync((object)key);
            if (paiementFactureF == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(paiementFactureF);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.PaiementFactureFExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<PaiementFactureF>(paiementFactureF);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            PaiementFactureF async = await this.db.PaiementFactureFs.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();
            this.db.PaiementFactureFs.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public SingleResult<FactureF> GetFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create<FactureF>(this.db.PaiementFactureFs.Where<PaiementFactureF>((Expression<Func<PaiementFactureF, bool>>)(m => m.Id == key)).Select<PaiementFactureF, FactureF>((Expression<Func<PaiementFactureF, FactureF>>)(m => m.FactureF)));
        }

        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create<Fournisseur>(this.db.PaiementFactureFs.Where<PaiementFactureF>((Expression<Func<PaiementFactureF, bool>>)(m => m.Id == key)).Select<PaiementFactureF, Fournisseur>((Expression<Func<PaiementFactureF, Fournisseur>>)(m => m.Fournisseur)));
        }

        [EnableQuery]
        public SingleResult<TypePaiement> GetTypePaiementFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypePaiement>(this.db.PaiementFactureFs.Where<PaiementFactureF>((Expression<Func<PaiementFactureF, bool>>)(m => m.Id == key)).Select<PaiementFactureF, TypePaiement>((Expression<Func<PaiementFactureF, TypePaiement>>)(m => m.TypePaiement)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool PaiementFactureFExists(Guid key)
        {
            return this.db.PaiementFactureFs.Count<PaiementFactureF>((Expression<Func<PaiementFactureF, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
