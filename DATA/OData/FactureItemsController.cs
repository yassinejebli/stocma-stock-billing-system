// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.FactureItemsController
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
  public class FactureItemsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<FactureItem> GetFactureItems()
    {
      return (IQueryable<FactureItem>) this.db.FactureItems;
    }

    [EnableQuery]
    public SingleResult<FactureItem> GetFactureItem([FromODataUri] Guid key)
    {
      return SingleResult.Create<FactureItem>(this.db.FactureItems.Where<FactureItem>((Expression<Func<FactureItem, bool>>) (factureItem => factureItem.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<FactureItem> patch)
    {
      this.Validate<FactureItem>(patch.GetEntity());
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      FactureItem factureItem = await this.db.FactureItems.FindAsync((object) key);
      if (factureItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(factureItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.FactureItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<FactureItem>(factureItem);
    }

    public async Task<IHttpActionResult> Post(FactureItem factureItem)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.FactureItems.Add(factureItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.FactureItemExists(factureItem.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<FactureItem>(factureItem);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<FactureItem> patch)
    {
      this.Validate<FactureItem>(patch.GetEntity());
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      FactureItem factureItem = await this.db.FactureItems.FindAsync((object) key);
      if (factureItem == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(factureItem);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.FactureItemExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<FactureItem>(factureItem);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      FactureItem async = await this.db.FactureItems.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.FactureItems.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<Article> GetArticle([FromODataUri] Guid key)
    {
      return SingleResult.Create<Article>(this.db.FactureItems.Where<FactureItem>((Expression<Func<FactureItem, bool>>) (m => m.Id == key)).Select<FactureItem, Article>((Expression<Func<FactureItem, Article>>) (m => m.Article)));
    }

    [EnableQuery]
    public SingleResult<Facture> GetFacture([FromODataUri] Guid key)
    {
      return SingleResult.Create<Facture>(this.db.FactureItems.Where<FactureItem>((Expression<Func<FactureItem, bool>>) (m => m.Id == key)).Select<FactureItem, Facture>((Expression<Func<FactureItem, Facture>>) (m => m.Facture)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool FactureItemExists(Guid key)
    {
      return this.db.FactureItems.Count<FactureItem>((Expression<Func<FactureItem, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
