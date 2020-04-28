// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.BonAvoirsController
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
  public class BonAvoirsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<BonAvoir> GetBonAvoirs()
    {
      return (IQueryable<BonAvoir>) this.db.BonAvoirs;
    }

    [EnableQuery]
    public SingleResult<BonAvoir> GetBonAvoir([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonAvoir>(this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>) (bonAvoir => bonAvoir.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<BonAvoir> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonAvoir bonAvoir = await this.db.BonAvoirs.FindAsync((object) key);
      if (bonAvoir == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(bonAvoir);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonAvoirExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonAvoir>(bonAvoir);
    }

    public async Task<IHttpActionResult> Post(BonAvoir bonAvoir)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.BonAvoirs.Add(bonAvoir);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.BonAvoirExists(bonAvoir.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<BonAvoir>(bonAvoir);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonAvoir> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonAvoir bonAvoir = await this.db.BonAvoirs.FindAsync((object) key);
      if (bonAvoir == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(bonAvoir);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonAvoirExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonAvoir>(bonAvoir);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      BonAvoir async = await this.db.BonAvoirs.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.BonAvoirs.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public IQueryable<BonAvoirItem> GetBonAvoirItems([FromODataUri] Guid key)
    {
      return this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>) (m => m.Id == key)).SelectMany<BonAvoir, BonAvoirItem>((Expression<Func<BonAvoir, IEnumerable<BonAvoirItem>>>) (m => m.BonAvoirItems));
    }

    [EnableQuery]
    public SingleResult<BonReception> GetBonReception([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonReception>(this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>) (m => m.Id == key)).Select<BonAvoir, BonReception>((Expression<Func<BonAvoir, BonReception>>) (m => m.BonReception)));
    }

    [EnableQuery]
    public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
    {
      return SingleResult.Create<Fournisseur>(this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>) (m => m.Id == key)).Select<BonAvoir, Fournisseur>((Expression<Func<BonAvoir, Fournisseur>>) (m => m.Fournisseur)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool BonAvoirExists(Guid key)
    {
      return this.db.BonAvoirs.Count<BonAvoir>((Expression<Func<BonAvoir, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
