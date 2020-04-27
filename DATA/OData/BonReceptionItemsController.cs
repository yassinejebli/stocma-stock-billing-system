// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.BonReceptionItemsController
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
  public class BonReceptionItemsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<BonReceptionItem> GetBonReceptionItems()
    {
      return (IQueryable<BonReceptionItem>) this.db.BonReceptionItems;
    }

    [EnableQuery]
    public SingleResult<BonReceptionItem> GetBonReceptionItem([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonReceptionItem>(this.db.BonReceptionItems.Where<BonReceptionItem>((Expression<Func<BonReceptionItem, bool>>) (bonReceptionItem => bonReceptionItem.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<BonReceptionItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonReceptionItem bonReceptionItem = await this.db.BonReceptionItems.FindAsync((object) key);
      if (bonReceptionItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(bonReceptionItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonReceptionItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonReceptionItem>(bonReceptionItem);
    }

    public async Task<IHttpActionResult> Post(BonReceptionItem bonReceptionItem)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.BonReceptionItems.Add(bonReceptionItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.BonReceptionItemExists(bonReceptionItem.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<BonReceptionItem>(bonReceptionItem);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonReceptionItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonReceptionItem bonReceptionItem = await this.db.BonReceptionItems.FindAsync((object) key);
      if (bonReceptionItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(bonReceptionItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonReceptionItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonReceptionItem>(bonReceptionItem);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      BonReceptionItem async = await this.db.BonReceptionItems.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.BonReceptionItems.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<Article> GetArticle([FromODataUri] Guid key)
    {
      return SingleResult.Create<Article>(this.db.BonReceptionItems.Where<BonReceptionItem>((Expression<Func<BonReceptionItem, bool>>) (m => m.Id == key)).Select<BonReceptionItem, Article>((Expression<Func<BonReceptionItem, Article>>) (m => m.Article)));
    }

    [EnableQuery]
    public SingleResult<BonReception> GetBonReception([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonReception>(this.db.BonReceptionItems.Where<BonReceptionItem>((Expression<Func<BonReceptionItem, bool>>) (m => m.Id == key)).Select<BonReceptionItem, BonReception>((Expression<Func<BonReceptionItem, BonReception>>) (m => m.BonReception)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool BonReceptionItemExists(Guid key)
    {
      return this.db.BonReceptionItems.Count<BonReceptionItem>((Expression<Func<BonReceptionItem, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
