using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using WebApplication1.DATA;

namespace WebApplication1.DATA.OData
{
    /*
    La classe WebApiConfig peut exiger d'autres modifications pour ajouter un itinéraire à ce contrôleur. Fusionnez ces instructions dans la méthode Register de la classe WebApiConfig, le cas échéant. Les URL OData sont sensibles à la casse.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApplication1.DATA;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Client>("Clients");
    builder.EntitySet<BonAvoirC>("BonAvoirCs"); 
    builder.EntitySet<BonLivraison>("BonLivraisons"); 
    builder.EntitySet<Devis>("Devises"); 
    builder.EntitySet<Dgb>("Dgbs"); 
    builder.EntitySet<Facture>("Factures"); 
    builder.EntitySet<FakeFacture>("FakeFactures"); 
    builder.EntitySet<Paiement>("Paiements"); 
    builder.EntitySet<Rdb>("Rdbs"); 
    builder.EntitySet<Revendeur>("Revendeurs"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ClientsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/Clients
        [EnableQuery]
        public IQueryable<Client> GetClients()
        {
            return db.Clients;
        }

        // GET: odata/Clients(5)
        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Clients.Where(client => client.Id == key));
        }

        // PUT: odata/Clients(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Client> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Client client = await db.Clients.FindAsync(key);
            if (client == null)
            {
                return NotFound();
            }

            patch.Put(client);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(client);
        }

        // POST: odata/Clients
        public async Task<IHttpActionResult> Post(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clients.Add(client);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientExists(client.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(client);
        }

        // PATCH: odata/Clients(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Client> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Client client = await db.Clients.FindAsync(key);
            if (client == null)
            {
                return NotFound();
            }

            patch.Patch(client);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(client);
        }

        // DELETE: odata/Clients(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Client client = await db.Clients.FindAsync(key);
            if (client == null)
            {
                return NotFound();
            }

            db.Clients.Remove(client);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Clients(5)/BonAvoirCs
        [EnableQuery]
        public IQueryable<BonAvoirC> GetBonAvoirCs([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.BonAvoirCs);
        }

        // GET: odata/Clients(5)/BonLivraisons
        [EnableQuery]
        public IQueryable<BonLivraison> GetBonLivraisons([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.BonLivraisons);
        }

        // GET: odata/Clients(5)/Devises
        [EnableQuery]
        public IQueryable<Devis> GetDevises([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.Devises);
        }

        // GET: odata/Clients(5)/Dgbs
        [EnableQuery]
        public IQueryable<Dgb> GetDgbs([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.Dgbs);
        }

        // GET: odata/Clients(5)/Factures
        [EnableQuery]
        public IQueryable<Facture> GetFactures([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.Factures);
        }

        // GET: odata/Clients(5)/FakeFactures
        [EnableQuery]
        public IQueryable<FakeFacture> GetFakeFactures([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.FakeFactures);
        }

        // GET: odata/Clients(5)/Paiements
        [EnableQuery]
        public IQueryable<Paiement> GetPaiements([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.Paiements);
        }

        // GET: odata/Clients(5)/Rdbs
        [EnableQuery]
        public IQueryable<Rdb> GetRdbs([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.Rdbs);
        }

        // GET: odata/Clients(5)/Revendeur
        [EnableQuery]
        public SingleResult<Revendeur> GetRevendeur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Clients.Where(m => m.Id == key).Select(m => m.Revendeur));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(Guid key)
        {
            return db.Clients.Count(e => e.Id == key) > 0;
        }
    }
}
