// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.CompaniesController
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
  public class CompaniesController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<Company> GetCompanies()
    {
      return (IQueryable<Company>) this.db.Companies;
    }

    [EnableQuery]
    public SingleResult<Company> GetCompany([FromODataUri] Guid key)
    {
      return SingleResult.Create<Company>(this.db.Companies.Where<Company>((Expression<Func<Company, bool>>) (company => company.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Company> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Company company = await this.db.Companies.FindAsync((object) key);
      if (company == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(company);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.CompanyExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Company>(company);
    }

    public async Task<IHttpActionResult> Post(Company company)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.Companies.Add(company);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        if (this.CompanyExists(company.Id))
          return (IHttpActionResult) this.Conflict();
        throw;
      }
      return (IHttpActionResult) this.Created<Company>(company);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Company> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Company company = await this.db.Companies.FindAsync((object) key);
      if (company == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(company);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.CompanyExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Company>(company);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
    {
      Company async = await this.db.Companies.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.Companies.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool CompanyExists(Guid key)
    {
      return this.db.Companies.Count<Company>((Expression<Func<Company, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
