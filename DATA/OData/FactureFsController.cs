using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.OData;
using System.Web.Http.OData.Routing;
using WebApplication1.DATA;
using WebGrease.Css.Extensions;

namespace WebApplication1.DATA.OData
{
    [Authorize]
    public class FactureFsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/FactureFs
        [EnableQueryAttribute(MaxExpansionDepth = 3)]
        public IQueryable<FactureF> GetFactureFs()
        {
            return db.FactureFs;
        }

        // GET: odata/FactureFs(5)
        [EnableQueryAttribute(MaxExpansionDepth = 3)]
        public SingleResult<FactureF> GetFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FactureFs.Where(factureF => factureF.Id == key));
        }

        // PUT: odata/FactureFs(5)
        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, FactureF newFactureF)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FactureF factureF = await db.FactureFs.FindAsync(key);
            if (factureF == null)
            {
                return NotFound();
            }

            factureF.Date = newFactureF.Date;
            factureF.IdTypePaiement = newFactureF.IdTypePaiement;
            factureF.Comment = newFactureF.Comment;
            factureF.NumBon = newFactureF.NumBon;

            //------------------------Updating bon receptions
            var oldBonReceptionIDs = factureF.BonReceptions.Select(x => x.Id);
            var originalBonReceptions = db.BonReceptions.Where(x => oldBonReceptionIDs.Contains(x.Id));
            originalBonReceptions.ForEach(x => x.IdFactureF = null);

            var newBonReceptionIDs = newFactureF.BonReceptions.Select(x => x.Id);
            var newOriginalBonReceptions = db.BonReceptions.Where(x => newBonReceptionIDs.Contains(x.Id));
            newOriginalBonReceptions.ForEach(x => x.IdFactureF = newFactureF.Id);

            //------------------------Updating payment

            var payment = db.PaiementFactureFs.FirstOrDefault(x => x.IdFactureF == factureF.Id);
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb7";


            var Total = newOriginalBonReceptions
                    .SelectMany(x => x.BonReceptionItems)
                    .Sum(x => ((x.Qte * x.Pu) * (1 + (x.Article.TVA ?? 20) / 100)));

            //espece
            var company = db.Companies.FirstOrDefault();
            var ESPECE_PAYMENT_TYPE = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");

            if (company.UseVAT && newFactureF.IdTypePaiement == ESPECE_PAYMENT_TYPE)
            {
                Total *= (1 + 0.0025f);
            }

            if (payment != null)
            {
                payment.Debit = Total;
                payment.Date = newFactureF.Date;
            }
            else
            {
                PaiementFactureF paiement = new PaiementFactureF()
                {
                    Id = Guid.NewGuid(),
                    IdFactureF = factureF.Id,
                    IdFournisseur = newFactureF.IdFournisseur,
                    Debit = Total,
                    IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                    Date = newFactureF.Date
                };
                db.PaiementFactureFs.Add(paiement);
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactureFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var factureWithItems = db.FactureFs.Where(x => x.Id == factureF.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(factureWithItems));
        }

        [EnableQuery]
        public async Task<IHttpActionResult> Post(FactureF factureF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bonReceptionIDs = factureF.BonReceptions.Select(x => x.Id);
            var originalBonReceptions = db.BonReceptions.Where(x => bonReceptionIDs.Contains(x.Id));
            originalBonReceptions.ForEach(x => x.IdFactureF = factureF.Id);
            factureF.BonReceptions = null;
            //-----------------------------------------------Updating payment
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb7";
            var Total = originalBonReceptions
                    .SelectMany(x => x.BonReceptionItems)
                    .Sum(x => ((x.Qte * x.Pu)) * (1 + (x.Article.TVA ?? 20) / 100));

            //espece
            var company = db.Companies.FirstOrDefault();
            var ESPECE_PAYMENT_TYPE = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");

            if (company.UseVAT && factureF?.IdTypePaiement == ESPECE_PAYMENT_TYPE)
            {
                Total *= (1 + 0.0025f);
            }

            PaiementFactureF paiement = new PaiementFactureF()
            {
                Id = Guid.NewGuid(),
                IdFactureF = factureF.Id,
                IdFournisseur = factureF.IdFournisseur,
                Debit = Total,
                IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                Date = factureF.Date
            };
            db.PaiementFactureFs.Add(paiement);

            db.FactureFs.Add(factureF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FactureFExists(factureF.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var factureWithItems = db.FactureFs.Where(x => x.Id == factureF.Id);
            return Content(HttpStatusCode.Created, SingleResult.Create(factureWithItems));
        }

        // PATCH: odata/FactureFs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<FactureF> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FactureF factureF = await db.FactureFs.FindAsync(key);
            if (factureF == null)
            {
                return NotFound();
            }

            patch.Patch(factureF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactureFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(factureF);
        }

        // DELETE: odata/FactureFs(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            FactureF factureF = await db.FactureFs.FindAsync(key);
            if (factureF == null)
            {
                return NotFound();
            }

            //dettach BRs
            var bonReceptionIDs = factureF.BonReceptions.Select(x => x.Id);
            var originalBonReceptions = db.BonReceptions.Where(x => bonReceptionIDs.Contains(x.Id));
            originalBonReceptions.ForEach(x => x.IdFactureF = null);

            db.PaiementFactureFs.RemoveRange(factureF.PaiementFactureFs);
            db.FactureFItems.RemoveRange(factureF.FactureFItems);
            db.FactureFs.Remove(factureF);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/FactureFs(5)/BonReceptions
        [EnableQuery]
        public IQueryable<BonReception> GetBonReceptions([FromODataUri] Guid key)
        {
            return db.FactureFs.Where(m => m.Id == key).SelectMany(m => m.BonReceptions);
        }

        // GET: odata/FactureFs(5)/FactureFItems
        [EnableQuery]
        public IQueryable<FactureFItem> GetFactureFItems([FromODataUri] Guid key)
        {
            return db.FactureFs.Where(m => m.Id == key).SelectMany(m => m.FactureFItems);
        }

        // GET: odata/FactureFs(5)/Fournisseur
        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FactureFs.Where(m => m.Id == key).Select(m => m.Fournisseur));
        }

        // GET: odata/FactureFs(5)/PaiementFs
        [EnableQuery]
        public IQueryable<PaiementF> GetPaiementFs([FromODataUri] Guid key)
        {
            return db.FactureFs.Where(m => m.Id == key).SelectMany(m => m.PaiementFs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FactureFExists(Guid key)
        {
            return db.FactureFs.Count(e => e.Id == key) > 0;
        }
    }
}
