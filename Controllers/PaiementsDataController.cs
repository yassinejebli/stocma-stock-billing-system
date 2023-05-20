using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DATA;

namespace WebApplication1.Controllers
{
    public class PaiementsDataController : Controller
    {
        MySaniSoftContext db = new MySaniSoftContext();

        public ActionResult getBalanceBeforeDateAndRealBalance(Guid id, DateTime? date)
        {
            var soldeBeforeDate = date.HasValue ? (db.Paiements.Where(x => DbFunctions.TruncateTime(x.Date) < date.Value.Date && x.IdClient == id && !x.Client.IsClientDivers).Sum(x => (float?)(x.Debit - x.Credit)) ?? 0f) : 0f;
            var solde = db.Paiements.Where(x => x.IdClient == id).Sum(x => (float?)(x.Debit - x.Credit)) ?? 0f;
            var calculatedSolde = db.Paiements.Where(x => x.IdClient == id && x.Hide != true).Sum(x => (float?)(x.Debit - x.Credit))??0f;
            return this.Json(new { solde, soldeBeforeDate, calculatedSolde }, JsonRequestBehavior.AllowGet);
        }
    }
}