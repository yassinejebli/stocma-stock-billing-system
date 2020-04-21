// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.BonAvoirItemsController
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
using System.Web.Http.OData;

namespace WebApplication1.DATA.OData
{
  public class BonAvoirItemsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<BonAvoirItem> GetBonAvoirItems()
    {
      return (IQueryable<BonAvoirItem>) this.db.BonAvoirItems;
    }

    [EnableQuery]
    public SingleResult<BonAvoirItem> GetBonAvoirItem([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonAvoirItem>(this.db.BonAvoirItems.Where<BonAvoirItem>((Expression<Func<BonAvoirItem, bool>>) (bonAvoirItem => bonAvoirItem.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<BonAvoirItem> patch)
    {
      this.Validate<BonAvoirItem>(patch.GetEntity());
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonAvoirItem bonAvoirItem = await this.db.BonAvoirItems.FindAsync((object) key);
      if (bonAvoirItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(bonAvoirItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonAvoirItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonAvoirItem>(bonAvoirItem);
    }

    public async Task<IHttpActionResult> Post(BonAvoirItem bonAvoirItem)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.BonAvoirItems.Add(bonAvoirItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.BonAvoirItemExists(bonAvoirItem.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<BonAvoirItem>(bonAvoirItem);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonAvoirItem> patch)
    {
      this.Validate<BonAvoirItem>(patch.GetEntity());
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonAvoirItem bonAvoirItem = await this.db.BonAvoirItems.FindAsync((object) key);
      if (bonAvoirItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(bonAvoirItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonAvoirItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonAvoirItem>(bonAvoirItem);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      BonAvoirItem async = await this.db.BonAvoirItems.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.BonAvoirItems.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<Article> GetArticle([FromODataUri] Guid key)
    {
      return SingleResult.Create<Article>(this.db.BonAvoirItems.Where<BonAvoirItem>((Expression<Func<BonAvoirItem, bool>>) (m => m.Id == key)).Select<BonAvoirItem, Article>((Expression<Func<BonAvoirItem, Article>>) (m => m.Article)));
    }

    [EnableQuery]
    public SingleResult<BonAvoir> GetBonAvoir([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonAvoir>(this.db.BonAvoirItems.Where<BonAvoirItem>((Expression<Func<BonAvoirItem, bool>>) (m => m.Id == key)).Select<BonAvoirItem, BonAvoir>((Expression<Func<BonAvoirItem, BonAvoir>>) (m => m.BonAvoir)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool BonAvoirItemExists(Guid key)
    {
      return this.db.BonAvoirItems.Count<BonAvoirItem>((Expression<Func<BonAvoirItem, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
