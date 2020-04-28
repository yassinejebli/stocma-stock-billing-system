// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.DepencesController
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
  public class DepencesController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<Depence> GetDepences()
    {
      return (IQueryable<Depence>) this.db.Depences;
    }

    [EnableQuery]
    public SingleResult<Depence> GetDepence([FromODataUri] Guid key)
    {
      return SingleResult.Create<Depence>(this.db.Depences.Where<Depence>((Expression<Func<Depence, bool>>) (depence => depence.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Depence> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Depence depence = await this.db.Depences.FindAsync((object) key);
      if (depence == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(depence);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.DepenceExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Depence>(depence);
    }

    public async Task<IHttpActionResult> Post(Depence depence)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.Depences.Add(depence);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.DepenceExists(depence.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<Depence>(depence);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Depence> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Depence depence = await this.db.Depences.FindAsync((object) key);
      if (depence == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(depence);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.DepenceExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Depence>(depence);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      Depence async = await this.db.Depences.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.Depences.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<TypeDepence> GetTypeDepence([FromODataUri] Guid key)
    {
      return SingleResult.Create<TypeDepence>(this.db.Depences.Where<Depence>((Expression<Func<Depence, bool>>) (m => m.Id == key)).Select<Depence, TypeDepence>((Expression<Func<Depence, TypeDepence>>) (m => m.TypeDepence)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool DepenceExists(Guid key)
    {
      return this.db.Depences.Count<Depence>((Expression<Func<Depence, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
