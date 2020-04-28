// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.BonCommandeItemsController
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
using Microsoft.AspNet.OData;

namespace WebApplication1.DATA.OData
{
  public class BonCommandeItemsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<BonCommandeItem> GetBonCommandeItems()
    {
      return (IQueryable<BonCommandeItem>) this.db.BonCommandeItems;
    }

    [EnableQuery]
    public SingleResult<BonCommandeItem> GetBonCommandeItem([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonCommandeItem>(this.db.BonCommandeItems.Where<BonCommandeItem>((Expression<Func<BonCommandeItem, bool>>) (bonCommandeItem => bonCommandeItem.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<BonCommandeItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonCommandeItem bonCommandeItem = await this.db.BonCommandeItems.FindAsync((object) key);
      if (bonCommandeItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(bonCommandeItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonCommandeItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonCommandeItem>(bonCommandeItem);
    }

    public async Task<IHttpActionResult> Post(BonCommandeItem bonCommandeItem)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.BonCommandeItems.Add(bonCommandeItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.BonCommandeItemExists(bonCommandeItem.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<BonCommandeItem>(bonCommandeItem);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<BonCommandeItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      BonCommandeItem bonCommandeItem = await this.db.BonCommandeItems.FindAsync((object) key);
      if (bonCommandeItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(bonCommandeItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.BonCommandeItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<BonCommandeItem>(bonCommandeItem);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      BonCommandeItem async = await this.db.BonCommandeItems.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.BonCommandeItems.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<Article> GetArticle([FromODataUri] Guid key)
    {
      return SingleResult.Create<Article>(this.db.BonCommandeItems.Where<BonCommandeItem>((Expression<Func<BonCommandeItem, bool>>) (m => m.Id == key)).Select<BonCommandeItem, Article>((Expression<Func<BonCommandeItem, Article>>) (m => m.Article)));
    }

    [EnableQuery]
    public SingleResult<BonCommande> GetBonCommande([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonCommande>(this.db.BonCommandeItems.Where<BonCommandeItem>((Expression<Func<BonCommandeItem, bool>>) (m => m.Id == key)).Select<BonCommandeItem, BonCommande>((Expression<Func<BonCommandeItem, BonCommande>>) (m => m.BonCommande)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool BonCommandeItemExists(Guid key)
    {
      return this.db.BonCommandeItems.Count<BonCommandeItem>((Expression<Func<BonCommandeItem, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
