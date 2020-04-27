// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.TarifItemsController
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace WebApplication1.DATA.OData
{
  public class TarifItemsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<TarifItem> GetTarifItems()
    {
      return (IQueryable<TarifItem>) this.db.TarifItems;
    }

    [EnableQuery]
    public SingleResult<TarifItem> GetTarifItem([FromODataUri] Guid key)
    {
      return SingleResult.Create<TarifItem>(this.db.TarifItems.Where<TarifItem>((Expression<Func<TarifItem, bool>>) (tarifItem => tarifItem.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<TarifItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      TarifItem tarifItem = await this.db.TarifItems.FindAsync((object) key);
      if (tarifItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(tarifItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.TarifItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<TarifItem>(tarifItem);
    }

    public async Task<IHttpActionResult> Post(TarifItem tarifItem)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.TarifItems.Add(tarifItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.TarifItemExists(tarifItem.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<TarifItem>(tarifItem);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<TarifItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      TarifItem tarifItem = await this.db.TarifItems.FindAsync((object) key);
      if (tarifItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(tarifItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.TarifItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<TarifItem>(tarifItem);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      TarifItem async = await this.db.TarifItems.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.TarifItems.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<Article> GetArticle([FromODataUri] Guid key)
    {
      return SingleResult.Create<Article>(this.db.TarifItems.Where<TarifItem>((Expression<Func<TarifItem, bool>>) (m => m.Id == key)).Select<TarifItem, Article>((Expression<Func<TarifItem, Article>>) (m => m.Article)));
    }

    [EnableQuery]
    public SingleResult<Tarif> GetTarif([FromODataUri] Guid key)
    {
      return SingleResult.Create<Tarif>(this.db.TarifItems.Where<TarifItem>((Expression<Func<TarifItem, bool>>) (m => m.Id == key)).Select<TarifItem, Tarif>((Expression<Func<TarifItem, Tarif>>) (m => m.Tarif)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool TarifItemExists(Guid key)
    {
      return this.db.TarifItems.Count<TarifItem>((Expression<Func<TarifItem, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
