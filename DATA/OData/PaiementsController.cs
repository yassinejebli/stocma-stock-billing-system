// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.PaiementsController
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
  public class PaiementsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<Paiement> GetPaiements()
    {
      return (IQueryable<Paiement>) this.db.Paiements;
    }

    [EnableQuery]
    public SingleResult<Paiement> GetPaiement([FromODataUri] Guid key)
    {
      return SingleResult.Create<Paiement>(this.db.Paiements.Where<Paiement>((Expression<Func<Paiement, bool>>) (paiement => paiement.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Paiement> patch)
    {
      this.Validate<Paiement>(patch.GetEntity());
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Paiement paiement = await this.db.Paiements.FindAsync((object) key);
      if (paiement == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(paiement);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.PaiementExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Paiement>(paiement);
    }

    public async Task<IHttpActionResult> Post(Paiement paiement)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.Paiements.Add(paiement);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.PaiementExists(paiement.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<Paiement>(paiement);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Paiement> patch)
    {
      this.Validate<Paiement>(patch.GetEntity());
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Paiement paiement = await this.db.Paiements.FindAsync((object) key);
      if (paiement == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(paiement);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.PaiementExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Paiement>(paiement);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      Paiement async = await this.db.Paiements.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.Paiements.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    [EnableQuery]
    public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
    {
      return SingleResult.Create<BonLivraison>(this.db.Paiements.Where<Paiement>((Expression<Func<Paiement, bool>>) (m => m.Id == key)).Select<Paiement, BonLivraison>((Expression<Func<Paiement, BonLivraison>>) (m => m.BonLivraison)));
    }

    [EnableQuery]
    public SingleResult<Client> GetClient([FromODataUri] Guid key)
    {
      return SingleResult.Create<Client>(this.db.Paiements.Where<Paiement>((Expression<Func<Paiement, bool>>) (m => m.Id == key)).Select<Paiement, Client>((Expression<Func<Paiement, Client>>) (m => m.Client)));
    }

    [EnableQuery]
    public SingleResult<TypePaiement> GetTypePaiement([FromODataUri] Guid key)
    {
      return SingleResult.Create<TypePaiement>(this.db.Paiements.Where<Paiement>((Expression<Func<Paiement, bool>>) (m => m.Id == key)).Select<Paiement, TypePaiement>((Expression<Func<Paiement, TypePaiement>>) (m => m.TypePaiement)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool PaiementExists(Guid key)
    {
      return this.db.Paiements.Count<Paiement>((Expression<Func<Paiement, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
