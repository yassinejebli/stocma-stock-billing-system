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
    public class PaiementFacturesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<PaiementFacture> GetPaiementFactures()
        {
            return db.PaiementFactures.OrderByDescending(x => new { x.Date, x.Id });
        }

        [EnableQuery]
        public SingleResult<PaiementFacture> GetPaiementFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<PaiementFacture>(this.db.PaiementFactures.Where<PaiementFacture>((Expression<Func<PaiementFacture, bool>>)(paiementFacture => paiementFacture.Id == key)));
        }

        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<PaiementFacture> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            PaiementFacture paiementFacture = await this.db.PaiementFactures.FindAsync((object)key);
            if (paiementFacture == null)
                return (IHttpActionResult)this.NotFound();
            patch.Put(paiementFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.PaiementFactureExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<PaiementFacture>(paiementFacture);
        }

        public async Task<IHttpActionResult> Post(PaiementFacture paiementFacture)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            this.db.PaiementFactures.Add(paiementFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.PaiementFactureExists(paiementFacture.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            return (IHttpActionResult)this.Created<PaiementFacture>(paiementFacture);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<PaiementFacture> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            PaiementFacture paiementFacture = await this.db.PaiementFactures.FindAsync((object)key);
            if (paiementFacture == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(paiementFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.PaiementFactureExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<PaiementFacture>(paiementFacture);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            PaiementFacture async = await this.db.PaiementFactures.FindAsync((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();
            this.db.PaiementFactures.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public SingleResult<Facture> GetFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<Facture>(this.db.PaiementFactures.Where<PaiementFacture>((Expression<Func<PaiementFacture, bool>>)(m => m.Id == key)).Select<PaiementFacture, Facture>((Expression<Func<PaiementFacture, Facture>>)(m => m.Facture)));
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.PaiementFactures.Where<PaiementFacture>((Expression<Func<PaiementFacture, bool>>)(m => m.Id == key)).Select<PaiementFacture, Client>((Expression<Func<PaiementFacture, Client>>)(m => m.Client)));
        }

        [EnableQuery]
        public SingleResult<TypePaiement> GetTypePaiementFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypePaiement>(this.db.PaiementFactures.Where<PaiementFacture>((Expression<Func<PaiementFacture, bool>>)(m => m.Id == key)).Select<PaiementFacture, TypePaiement>((Expression<Func<PaiementFacture, TypePaiement>>)(m => m.TypePaiement)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool PaiementFactureExists(Guid key)
        {
            return this.db.PaiementFactures.Count<PaiementFacture>((Expression<Func<PaiementFacture, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
