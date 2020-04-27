// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.FakeFactureItemsController
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
  public class FakeFactureItemsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<FakeFactureItem> GetFakeFactureItems()
    {
      return (IQueryable<FakeFactureItem>) this.db.FakeFactureItems;
    }

    [EnableQuery]
    public SingleResult<FakeFactureItem> GetFakeFactureItem([FromODataUri] Guid key)
    {
      return SingleResult.Create<FakeFactureItem>(this.db.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>) (fakeFactureItem => fakeFactureItem.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<FakeFactureItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      FakeFactureItem fakeFactureItem = await this.db.FakeFactureItems.FindAsync((object) key);
      if (fakeFactureItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(fakeFactureItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.FakeFactureItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<FakeFactureItem>(fakeFactureItem);
    }

    public async Task<IHttpActionResult> Post(FakeFactureItem fakeFactureItem)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.FakeFactureItems.Add(fakeFactureItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.FakeFactureItemExists(fakeFactureItem.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<FakeFactureItem>(fakeFactureItem);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<FakeFactureItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      FakeFactureItem fakeFactureItem = await this.db.FakeFactureItems.FindAsync((object) key);
      if (fakeFactureItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(fakeFactureItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.FakeFactureItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<FakeFactureItem>(fakeFactureItem);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      FakeFactureItem async = await this.db.FakeFactureItems.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.FakeFactureItems.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<ArticleFacture> GetArticleFacture([FromODataUri] Guid key)
    {
      return SingleResult.Create<ArticleFacture>(this.db.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>) (m => m.Id == key)).Select<FakeFactureItem, ArticleFacture>((Expression<Func<FakeFactureItem, ArticleFacture>>) (m => m.ArticleFacture)));
    }

    [EnableQuery]
    public SingleResult<FakeFacture> GetFakeFacture([FromODataUri] Guid key)
    {
      return SingleResult.Create<FakeFacture>(this.db.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>) (m => m.Id == key)).Select<FakeFactureItem, FakeFacture>((Expression<Func<FakeFactureItem, FakeFacture>>) (m => m.FakeFacture)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool FakeFactureItemExists(Guid key)
    {
      return this.db.FakeFactureItems.Count<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
