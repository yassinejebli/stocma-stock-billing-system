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
using WebGrease.Css.Extensions;

namespace WebApplication1.DATA.OData
{
    [Authorize]
    public class FacturesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Facture> GetFactures()
        {
            return (IQueryable<Facture>)this.db.Factures.OrderByDescending(x => x.Date);
        }

        [EnableQuery]
        public SingleResult<Facture> GetFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<Facture>(this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>)(facture => facture.Id == key)));
        }

        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Facture newFacture)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            Facture facture = await this.db.Factures.FindAsync((object)key);
            if (facture == null)
                return (IHttpActionResult)this.NotFound();

            //-----------------------------------------------Updating document items
            facture.ModificationDate = DateTime.Now;
            facture.Date = newFacture.Date;
            facture.Ref = newFacture.Ref;
            facture.Note = newFacture.Note;
            var numBonGenerator = new DocNumberGenerator();

            facture.NumBon = numBonGenerator.getNumDocByCompany(newFacture.Ref - 1, newFacture.Date);

            //------------------------Updating bon livraisons
            var oldBonLivraisonIDs = facture.BonLivraisons.Select(x => x.Id);
            var originalBonLivraisons = db.BonLivraisons.Where(x => oldBonLivraisonIDs.Contains(x.Id));
            originalBonLivraisons.ForEach(x => x.IdFacture = null);

            var newBonLivraisonIDs = newFacture.BonLivraisons.Select(x => x.Id);
            var newOriginalBonLivraisons = db.BonLivraisons.Where(x => newBonLivraisonIDs.Contains(x.Id));
            originalBonLivraisons.ForEach(x => x.IdFacture = newFacture.Id);

            //------------------------Updating payment

            var payment = db.PaiementFactures.FirstOrDefault(x => x.IdFacture == facture.Id);
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb7";

            //espece

            var Total = newOriginalBonLivraisons
                    .SelectMany(x => x.BonLivraisonItems)
                    .Sum(x => x.Qte * x.Pu);

            if (payment != null)
            {
                payment.Debit = Total;
                payment.Date = newFacture.Date;
            }
            else
            {
                PaiementFacture paiement = new PaiementFacture()
                {
                    Id = Guid.NewGuid(),
                    IdFacture = facture.Id,
                    IdClient = newFacture.IdClient,
                    Debit = Total,
                    IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                    Date = newFacture.Date
                };
                db.PaiementFactures.Add(paiement);
            }
            
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FactureExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<Facture>(facture);
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Post(Facture facture)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);

            var bonLivraisonIDs = facture.BonLivraisons.Select(x => x.Id);
            var originalBonLivraisons = db.BonLivraisons.Where(x => bonLivraisonIDs.Contains(x.Id));
            originalBonLivraisons.ForEach(x => x.IdFacture = facture.Id);
            facture.BonLivraisons = null;
            var numBonGenerator = new DocNumberGenerator();
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.Factures.Where(x => x.Date.Year == currentYear && x.IdSite == facture.IdSite).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 0;
            facture.Ref = lastRef + 1;
            facture.NumBon = numBonGenerator.getNumDocByCompany(lastRef, facture.Date);
            facture.ClientName = db.Clients.Find(facture.IdClient).Name;
            //-----------------------------------------------Updating payment
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb7";
            var Total = originalBonLivraisons
                    .SelectMany(x => x.BonLivraisonItems)
                    .Sum(x => x.Qte * x.Pu);

            PaiementFacture paiement = new PaiementFacture()
            {
                Id = Guid.NewGuid(),
                IdFacture = facture.Id,
                IdClient = facture.IdClient,
                Debit = Total,
                IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                Date = facture.Date
            };
            db.PaiementFactures.Add(paiement);
            db.Factures.Add(facture);

            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.FactureExists(facture.Id))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            var factureWithItems = db.Factures.Where(x => x.Id == facture.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(factureWithItems));
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Facture> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            Facture facture = await this.db.Factures.FindAsync((object)key);
            if (facture == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(facture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FactureExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<Facture>(facture);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Facture facture = await this.db.Factures.FindAsync((object)key);
            if (facture == null)
                return NotFound();

            //dettach BLs
            var bonLivraisonIDs = facture.BonLivraisons.Select(x => x.Id);
            var originalBonLivraisons = db.BonLivraisons.Where(x => bonLivraisonIDs.Contains(x.Id));
            originalBonLivraisons.ForEach(x => x.IdFacture = null);

            db.PaiementFactures.RemoveRange(facture.PaiementFactures);
            db.FactureItems.RemoveRange(facture.FactureItems);
            this.db.Factures.Remove(facture);
            int num = await this.db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonLivraison>(this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>)(m => m.Id == key)).Select<Facture, BonLivraison>((Expression<Func<Facture, BonLivraison>>)(m => m.BonLivraison)));
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>)(m => m.Id == key)).Select<Facture, Client>((Expression<Func<Facture, Client>>)(m => m.Client)));
        }

        [EnableQuery]
        public IQueryable<FactureItem> GetFactureItems([FromODataUri] Guid key)
        {
            return this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>)(m => m.Id == key)).SelectMany<Facture, FactureItem>((Expression<Func<Facture, IEnumerable<FactureItem>>>)(m => m.FactureItems));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool FactureExists(Guid key)
        {
            return this.db.Factures.Count<Facture>((Expression<Func<Facture, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
