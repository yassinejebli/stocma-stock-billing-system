// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.JournalConnexionsController
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
  public class JournalConnexionsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<JournalConnexion> GetJournalConnexions()
    {
      return (IQueryable<JournalConnexion>) this.db.JournalConnexions;
    }

    [EnableQuery]
    public SingleResult<JournalConnexion> GetJournalConnexion([FromODataUri] Guid key)
    {
      return SingleResult.Create<JournalConnexion>(this.db.JournalConnexions.Where<JournalConnexion>((Expression<Func<JournalConnexion, bool>>) (journalConnexion => journalConnexion.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<JournalConnexion> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      JournalConnexion journalConnexion = await this.db.JournalConnexions.FindAsync((object) key);
      if (journalConnexion == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(journalConnexion);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.JournalConnexionExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<JournalConnexion>(journalConnexion);
    }

    public async Task<IHttpActionResult> Post(JournalConnexion journalConnexion)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.JournalConnexions.Add(journalConnexion);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.JournalConnexionExists(journalConnexion.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<JournalConnexion>(journalConnexion);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<JournalConnexion> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      JournalConnexion journalConnexion = await this.db.JournalConnexions.FindAsync((object) key);
      if (journalConnexion == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(journalConnexion);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.JournalConnexionExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<JournalConnexion>(journalConnexion);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      JournalConnexion async = await this.db.JournalConnexions.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.JournalConnexions.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool JournalConnexionExists(Guid key)
    {
      return this.db.JournalConnexions.Count<JournalConnexion>((Expression<Func<JournalConnexion, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
