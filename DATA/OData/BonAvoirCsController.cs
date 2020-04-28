// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.BonAvoirCsController
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
  public class BonAvoirCsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<BonAvoirC> GetBonAvoirCs()
    {
      return (IQueryable<BonAvoirC>) this.db.BonAvoirCs;
    }

    [EnableQuery]
    public SingleResult<BonAvoirC> GetBonAvoirC([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonAvoirC>(this.db.BonAvoirCs.Where<BonAvoirC>((Expression<Func<BonAvoirC, bool>>) (bonAvoirC => bonAvoirC.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<BonAvoirC> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonAvoirC bonAvoirC = await this.db.BonAvoirCs.FindAsync((object) key);
      if (bonAvoirC == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(bonAvoirC);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonAvoirCExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonAvoirC>(bonAvoirC);
    }

    public async Task<IHttpActionResult> Post(BonAvoirC bonAvoirC)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.BonAvoirCs.Add(bonAvoirC);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.BonAvoirCExists(bonAvoirC.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<BonAvoirC>(bonAvoirC);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonAvoirC> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonAvoirC bonAvoirC = await this.db.BonAvoirCs.FindAsync((object) key);
      if (bonAvoirC == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(bonAvoirC);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonAvoirCExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonAvoirC>(bonAvoirC);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      BonAvoirC async = await this.db.BonAvoirCs.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.BonAvoirCs.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public IQueryable<BonAvoirCItem> GetBonAvoirCItems([FromODataUri] Guid key)
    {
      return this.db.BonAvoirCs.Where<BonAvoirC>((Expression<Func<BonAvoirC, bool>>) (m => m.Id == key)).SelectMany<BonAvoirC, BonAvoirCItem>((Expression<Func<BonAvoirC, IEnumerable<BonAvoirCItem>>>) (m => m.BonAvoirCItems));
    }

    [EnableQuery]
    public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonLivraison>(this.db.BonAvoirCs.Where<BonAvoirC>((Expression<Func<BonAvoirC, bool>>) (m => m.Id == key)).Select<BonAvoirC, BonLivraison>((Expression<Func<BonAvoirC, BonLivraison>>) (m => m.BonLivraison)));
    }

    [EnableQuery]
    public SingleResult<Client> GetClient([FromODataUri] Guid key)
    {
      return SingleResult.Create<Client>(this.db.BonAvoirCs.Where<BonAvoirC>((Expression<Func<BonAvoirC, bool>>) (m => m.Id == key)).Select<BonAvoirC, Client>((Expression<Func<BonAvoirC, Client>>) (m => m.Client)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool BonAvoirCExists(Guid key)
    {
      return this.db.BonAvoirCs.Count<BonAvoirC>((Expression<Func<BonAvoirC, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
