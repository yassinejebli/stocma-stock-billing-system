// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.FacturesController
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
  public class FacturesController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<Facture> GetFactures()
    {
      return (IQueryable<Facture>) this.db.Factures;
    }

    [EnableQuery]
    public SingleResult<Facture> GetFacture([FromODataUri] Guid key)
    {
      return SingleResult.Create<Facture>(this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>) (facture => facture.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Facture> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Facture facture = await this.db.Factures.FindAsync((object) key);
      if (facture == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(facture);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.FactureExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Facture>(facture);
    }

    public async Task<IHttpActionResult> Post(Facture facture)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.Factures.Add(facture);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.FactureExists(facture.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<Facture>(facture);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Facture> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Facture facture = await this.db.Factures.FindAsync((object) key);
      if (facture == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(facture);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.FactureExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Facture>(facture);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      Facture async = await this.db.Factures.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.Factures.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonLivraison>(this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>) (m => m.Id == key)).Select<Facture, BonLivraison>((Expression<Func<Facture, BonLivraison>>) (m => m.BonLivraison)));
    }

    [EnableQuery]
    public SingleResult<Client> GetClient([FromODataUri] Guid key)
    {
      return SingleResult.Create<Client>(this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>) (m => m.Id == key)).Select<Facture, Client>((Expression<Func<Facture, Client>>) (m => m.Client)));
    }

    [EnableQuery]
    public IQueryable<FactureItem> GetFactureItems([FromODataUri] Guid key)
    {
      return this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>) (m => m.Id == key)).SelectMany<Facture, FactureItem>((Expression<Func<Facture, IEnumerable<FactureItem>>>) (m => m.FactureItems));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool FactureExists(Guid key)
    {
      return this.db.Factures.Count<Facture>((Expression<Func<Facture, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
