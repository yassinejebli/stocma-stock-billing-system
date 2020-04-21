// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.TypeDepencesController
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
using System.Web.Http.OData;

namespace WebApplication1.DATA.OData
{
  public class TypeDepencesController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<TypeDepence> GetTypeDepences()
    {
      return (IQueryable<TypeDepence>) this.db.TypeDepences;
    }

    [EnableQuery]
    public SingleResult<TypeDepence> GetTypeDepence([FromODataUri] Guid key)
    {
      return SingleResult.Create<TypeDepence>(this.db.TypeDepences.Where<TypeDepence>((Expression<Func<TypeDepence, bool>>) (typeDepence => typeDepence.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<TypeDepence> patch)
    {
      this.Validate<TypeDepence>(patch.GetEntity());
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      TypeDepence typeDepence = await this.db.TypeDepences.FindAsync((object) key);
      if (typeDepence == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(typeDepence);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.TypeDepenceExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<TypeDepence>(typeDepence);
    }

    public async Task<IHttpActionResult> Post(TypeDepence typeDepence)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.TypeDepences.Add(typeDepence);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.TypeDepenceExists(typeDepence.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<TypeDepence>(typeDepence);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<TypeDepence> patch)
    {
      this.Validate<TypeDepence>(patch.GetEntity());
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      TypeDepence typeDepence = await this.db.TypeDepences.FindAsync((object) key);
      if (typeDepence == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(typeDepence);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.TypeDepenceExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<TypeDepence>(typeDepence);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      TypeDepence async = await this.db.TypeDepences.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.TypeDepences.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public IQueryable<Depence> GetDepences([FromODataUri] Guid key)
    {
      return this.db.TypeDepences.Where<TypeDepence>((Expression<Func<TypeDepence, bool>>) (m => m.Id == key)).SelectMany<TypeDepence, Depence>((Expression<Func<TypeDepence, IEnumerable<Depence>>>) (m => m.Depences));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool TypeDepenceExists(Guid key)
    {
      return this.db.TypeDepences.Count<TypeDepence>((Expression<Func<TypeDepence, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
