// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.DevisesController
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
  public class DevisesController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<Devis> GetDevises()
    {
      return (IQueryable<Devis>) this.db.Devises;
    }

    [EnableQuery]
    public SingleResult<Devis> GetDevis([FromODataUri] Guid key)
    {
      return SingleResult.Create<Devis>(this.db.Devises.Where<Devis>((Expression<Func<Devis, bool>>) (devis => devis.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Devis> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Devis devis = await this.db.Devises.FindAsync((object) key);
      if (devis == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(devis);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.DevisExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Devis>(devis);
    }

    public async Task<IHttpActionResult> Post(Devis devis)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.Devises.Add(devis);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.DevisExists(devis.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<Devis>(devis);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Devis> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Devis devis = await this.db.Devises.FindAsync((object) key);
      if (devis == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(devis);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.DevisExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Devis>(devis);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      Devis async = await this.db.Devises.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.Devises.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<Client> GetClient([FromODataUri] Guid key)
    {
      return SingleResult.Create<Client>(this.db.Devises.Where<Devis>((Expression<Func<Devis, bool>>) (m => m.Id == key)).Select<Devis, Client>((Expression<Func<Devis, Client>>) (m => m.Client)));
    }

    [EnableQuery]
    public IQueryable<DevisItem> GetDevisItems([FromODataUri] Guid key)
    {
      return this.db.Devises.Where<Devis>((Expression<Func<Devis, bool>>) (m => m.Id == key)).SelectMany<Devis, DevisItem>((Expression<Func<Devis, IEnumerable<DevisItem>>>) (m => m.DevisItems));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool DevisExists(Guid key)
    {
      return this.db.Devises.Count<Devis>((Expression<Func<Devis, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
