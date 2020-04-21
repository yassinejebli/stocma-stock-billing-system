// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.FakeFacturesController
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
  public class FakeFacturesController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<FakeFacture> GetFakeFactures()
    {
      return (IQueryable<FakeFacture>) this.db.FakeFactures;
    }

    [EnableQuery]
    public SingleResult<FakeFacture> GetFakeFacture([FromODataUri] Guid key)
    {
      return SingleResult.Create<FakeFacture>(this.db.FakeFactures.Where<FakeFacture>((Expression<Func<FakeFacture, bool>>) (fakeFacture => fakeFacture.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<FakeFacture> patch)
    {
      this.Validate<FakeFacture>(patch.GetEntity());
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      FakeFacture fakeFacture = await this.db.FakeFactures.FindAsync((object) key);
      if (fakeFacture == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(fakeFacture);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.FakeFactureExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<FakeFacture>(fakeFacture);
    }

    public async Task<IHttpActionResult> Post(FakeFacture fakeFacture)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.FakeFactures.Add(fakeFacture);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.FakeFactureExists(fakeFacture.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<FakeFacture>(fakeFacture);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<FakeFacture> patch)
    {
      this.Validate<FakeFacture>(patch.GetEntity());
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      FakeFacture fakeFacture = await this.db.FakeFactures.FindAsync((object) key);
      if (fakeFacture == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(fakeFacture);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.FakeFactureExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<FakeFacture>(fakeFacture);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      FakeFacture async = await this.db.FakeFactures.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.FakeFactures.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<Client> GetClient([FromODataUri] Guid key)
    {
      return SingleResult.Create<Client>(this.db.FakeFactures.Where<FakeFacture>((Expression<Func<FakeFacture, bool>>) (m => m.Id == key)).Select<FakeFacture, Client>((Expression<Func<FakeFacture, Client>>) (m => m.Client)));
    }

    [EnableQuery]
    public IQueryable<FakeFactureItem> GetFakeFactureItems([FromODataUri] Guid key)
    {
      return this.db.FakeFactures.Where<FakeFacture>((Expression<Func<FakeFacture, bool>>) (m => m.Id == key)).SelectMany<FakeFacture, FakeFactureItem>((Expression<Func<FakeFacture, IEnumerable<FakeFactureItem>>>) (m => m.FakeFactureItems));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool FakeFactureExists(Guid key)
    {
      return this.db.FakeFactures.Count<FakeFacture>((Expression<Func<FakeFacture, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
