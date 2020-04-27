// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.TarifsController
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
using System.Web.OData;

namespace WebApplication1.DATA.OData
{
  public class TarifsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<Tarif> GetTarifs()
    {
      return (IQueryable<Tarif>) this.db.Tarifs;
    }

    [EnableQuery]
    public SingleResult<Tarif> GetTarif([FromODataUri] Guid key)
    {
      return SingleResult.Create<Tarif>(this.db.Tarifs.Where<Tarif>((Expression<Func<Tarif, bool>>) (tarif => tarif.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Tarif> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Tarif tarif = await this.db.Tarifs.FindAsync((object) key);
      if (tarif == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(tarif);
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

    public async Task<IHttpActionResult> Post(Tarif tarif)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.Tarifs.Add(tarif);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.TarifExists(tarif.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<Tarif>(tarif);
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
      this.db.Tarifs.Remove(async);
      int num = await this.db.SaveChangesAsync();
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
