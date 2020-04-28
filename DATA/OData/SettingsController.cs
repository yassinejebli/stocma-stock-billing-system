// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.OData.SettingsController
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
  public class SettingsController : ODataController
  {
    private MySaniSoftContext db = new MySaniSoftContext();

    [EnableQuery]
    public IQueryable<Setting> GetSettings()
    {
      return (IQueryable<Setting>) this.db.Settings;
    }

    [EnableQuery]
    public SingleResult<Setting> GetSetting([FromODataUri] int key)
    {
      return SingleResult.Create<Setting>(this.db.Settings.Where<Setting>((Expression<Func<Setting, bool>>) (setting => setting.Id == key)));
    }

    public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Setting> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Setting setting = await this.db.Settings.FindAsync((object) key);
      if (setting == null)
        return (IHttpActionResult) this.NotFound();
      patch.Put(setting);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.SettingExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Setting>(setting);
    }

    public async Task<IHttpActionResult> Post(Setting setting)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      this.db.Settings.Add(setting);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.Created<Setting>(setting);
    }

    [AcceptVerbs(new string[] {"PATCH", "MERGE"})]
    public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Setting> patch)
    {
      if (!this.ModelState.IsValid)
        return (IHttpActionResult) this.BadRequest(this.ModelState);
      Setting setting = await this.db.Settings.FindAsync((object) key);
      if (setting == null)
        return (IHttpActionResult) this.NotFound();
      patch.Patch(setting);
      try
      {
        int num = await this.db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        if (!this.SettingExists(key))
          return (IHttpActionResult) this.NotFound();
        throw;
      }
      return (IHttpActionResult) this.Updated<Setting>(setting);
    }

    public async Task<IHttpActionResult> Delete([FromODataUri] int key)
    {
      Setting async = await this.db.Settings.FindAsync((object) key);
      if (async == null)
        return (IHttpActionResult) this.NotFound();
      this.db.Settings.Remove(async);
      int num = await this.db.SaveChangesAsync();
      return (IHttpActionResult) this.StatusCode(HttpStatusCode.NoContent);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }

    private bool SettingExists(int key)
    {
      return this.db.Settings.Count<Setting>((Expression<Func<Setting, bool>>) (e => e.Id == key)) > 0;
    }
  }
}
