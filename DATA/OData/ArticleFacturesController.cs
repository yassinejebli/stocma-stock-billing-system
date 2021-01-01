// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.ArticleFacturesController
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
  public class ArticleFacturesController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery(EnsureStableOrdering = false)]
    public IQueryable<ArticleFacture> GetArticleFactures()
    {
      return (IQueryable<ArticleFacture>) this.db.ArticleFactures.OrderByDescending(x=>x.QteStock);
    }

    [EnableQuery]
    public SingleResult<ArticleFacture> GetArticleFacture([FromODataUri] Guid key)
    {
      return SingleResult.Create<ArticleFacture>(this.db.ArticleFactures.Where<ArticleFacture>((Expression<Func<ArticleFacture, bool>>) (articleFacture => articleFacture.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<ArticleFacture> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      ArticleFacture articleFacture = await this.db.ArticleFactures.FindAsync((object) key);
      if (articleFacture == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(articleFacture);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.ArticleFactureExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<ArticleFacture>(articleFacture);
    }

    public async Task<IHttpActionResult> Post(ArticleFacture articleFacture)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.ArticleFactures.Add(articleFacture);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.ArticleFactureExists(articleFacture.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<ArticleFacture>(articleFacture);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<ArticleFacture> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      ArticleFacture articleFacture = await this.db.ArticleFactures.FindAsync((object) key);
      if (articleFacture == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(articleFacture);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.ArticleFactureExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<ArticleFacture>(articleFacture);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      ArticleFacture async = await this.db.ArticleFactures.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.ArticleFactures.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public IQueryable<FakeFactureItem> GetFakeFactureItems([FromODataUri] Guid key)
    {
      return this.db.ArticleFactures.Where<ArticleFacture>((Expression<Func<ArticleFacture, bool>>) (m => m.Id == key)).SelectMany<ArticleFacture, FakeFactureItem>((Expression<Func<ArticleFacture, IEnumerable<FakeFactureItem>>>) (m => m.FakeFactureItems));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool ArticleFactureExists(Guid key)
    {
      return this.db.ArticleFactures.Count<ArticleFacture>((Expression<Func<ArticleFacture, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
