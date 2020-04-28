// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.DevisItemsController
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
  public class DevisItemsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<DevisItem> GetDevisItems()
    {
      return (IQueryable<DevisItem>) this.db.DevisItems;
    }

    [EnableQuery]
    public SingleResult<DevisItem> GetDevisItem([FromODataUri] Guid key)
    {
      return SingleResult.Create<DevisItem>(this.db.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>) (devisItem => devisItem.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<DevisItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      DevisItem devisItem = await this.db.DevisItems.FindAsync((object) key);
      if (devisItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(devisItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.DevisItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<DevisItem>(devisItem);
    }

    public async Task<IHttpActionResult> Post(DevisItem devisItem)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.DevisItems.Add(devisItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.DevisItemExists(devisItem.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<DevisItem>(devisItem);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<DevisItem> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      DevisItem devisItem = await this.db.DevisItems.FindAsync((object) key);
      if (devisItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(devisItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.DevisItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<DevisItem>(devisItem);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      DevisItem async = await this.db.DevisItems.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.DevisItems.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<Article> GetArticle([FromODataUri] Guid key)
    {
      return SingleResult.Create<Article>(this.db.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>) (m => m.Id == key)).Select<DevisItem, Article>((Expression<Func<DevisItem, Article>>) (m => m.Article)));
    }

    [EnableQuery]
    public SingleResult<Devis> GetDevis([FromODataUri] Guid key)
    {
      return SingleResult.Create<Devis>(this.db.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>) (m => m.Id == key)).Select<DevisItem, Devis>((Expression<Func<DevisItem, Devis>>) (m => m.Devis)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool DevisItemExists(Guid key)
    {
      return this.db.DevisItems.Count<DevisItem>((Expression<Func<DevisItem, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
