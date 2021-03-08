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
  public class TarifsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery(EnsureStableOrdering = false)]
    public IQueryable<Tarif> GetTarifs()
    {
      return db.Tarifs.OrderByDescending(x => x.Date); ;
    }

    [EnableQuery]
    public SingleResult<Tarif> GetTarif([FromODataUri] Guid key)
    {
      return SingleResult.Create<Tarif>(this.db.Tarifs.Where<Tarif>((Expression<Func<Tarif, bool>>) (tarif => tarif.Id == key)));
    }

    [EnableQuery]
    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Tarif newTarif)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Tarif tarif = await this.db.Tarifs.FindAsync((object) key);
      if (tarif == null)
        return (IHttpActionResult) this.NotFound();

      db.TarifItems.RemoveRange(tarif.TarifItems);
      db.TarifItems.AddRange(newTarif.TarifItems);
      tarif.Date = newTarif.Date;
      tarif.Ref = newTarif.Ref;

      try
      {
        await db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.TarifExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      var tarifWithItems = db.Tarifs.Where(x => x.Id == tarif.Id);
      return Content(HttpStatusCode.Created, SingleResult.Create(tarifWithItems));
    }

    [EnableQuery]
    public async Task<IHttpActionResult> Post(Tarif tarif)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.Tarifs.Add(tarif);
      try
      {
        await db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.TarifExists(tarif.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
        var tarifWithItems = db.Tarifs.Where(x => x.Id == tarif.Id);
        return Content(HttpStatusCode.Created, SingleResult.Create(tarifWithItems));
     }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Tarif> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Tarif tarif = await this.db.Tarifs.FindAsync((object) key);
      if (tarif == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(tarif);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.TarifExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Tarif>(tarif);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      Tarif async = await this.db.Tarifs.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      db.TarifItems.RemoveRange(async.TarifItems);
      db.Tarifs.Remove(async);
      await db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public IQueryable<TarifItem> GetTarifItems([FromODataUri] Guid key)
    {
      return this.db.Tarifs.Where<Tarif>((Expression<Func<Tarif, bool>>) (m => m.Id == key)).SelectMany<Tarif, TarifItem>((Expression<Func<Tarif, IEnumerable<TarifItem>>>) (m => m.TarifItems));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool TarifExists(Guid key)
    {
      return this.db.Tarifs.Count<Tarif>((Expression<Func<Tarif, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
