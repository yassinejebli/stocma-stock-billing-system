// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.BonAvoirCItemsController
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
  public class BonAvoirCItemsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<BonAvoirCItem> GetBonAvoirCItems()
    {
      return (IQueryable<BonAvoirCItem>) this.db.BonAvoirCItems;
    }

    [EnableQuery]
    public SingleResult<BonAvoirCItem> GetBonAvoirCItem([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonAvoirCItem>(this.db.BonAvoirCItems.Where<BonAvoirCItem>((Expression<Func<BonAvoirCItem, bool>>) (bonAvoirCItem => bonAvoirCItem.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<BonAvoirCItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonAvoirCItem bonAvoirCItem = await this.db.BonAvoirCItems.FindAsync((object) key);
      if (bonAvoirCItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(bonAvoirCItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonAvoirCItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonAvoirCItem>(bonAvoirCItem);
    }

    public async Task<IHttpActionResult> Post(BonAvoirCItem bonAvoirCItem)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.BonAvoirCItems.Add(bonAvoirCItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.BonAvoirCItemExists(bonAvoirCItem.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<BonAvoirCItem>(bonAvoirCItem);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonAvoirCItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonAvoirCItem bonAvoirCItem = await this.db.BonAvoirCItems.FindAsync((object) key);
      if (bonAvoirCItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(bonAvoirCItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonAvoirCItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonAvoirCItem>(bonAvoirCItem);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      BonAvoirCItem async = await this.db.BonAvoirCItems.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.BonAvoirCItems.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<Article> GetArticle([FromODataUri] Guid key)
    {
      return SingleResult.Create<Article>(this.db.BonAvoirCItems.Where<BonAvoirCItem>((Expression<Func<BonAvoirCItem, bool>>) (m => m.Id == key)).Select<BonAvoirCItem, Article>((Expression<Func<BonAvoirCItem, Article>>) (m => m.Article)));
    }

    [EnableQuery]
    public SingleResult<BonAvoirC> GetBonAvoirC([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonAvoirC>(this.db.BonAvoirCItems.Where<BonAvoirCItem>((Expression<Func<BonAvoirCItem, bool>>) (m => m.Id == key)).Select<BonAvoirCItem, BonAvoirC>((Expression<Func<BonAvoirCItem, BonAvoirC>>) (m => m.BonAvoirC)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool BonAvoirCItemExists(Guid key)
    {
      return this.db.BonAvoirCItems.Count<BonAvoirCItem>((Expression<Func<BonAvoirCItem, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
