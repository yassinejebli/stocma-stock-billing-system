using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DATA;

namespace WebApplication1.Controllers
{
    public class SalesHistoryController : Controller
    {
        MySaniSoftContext db = new MySaniSoftContext();

        // GET: SalesHistory
        public ActionResult getPriceLastSale(Guid IdClient, Guid IdArticle)
        {
            var price = 0.0f;
            var bi = db.BonLivraisonItems
                .Where((x => x.IdArticle == IdArticle && x.BonLivraison.IdClient == IdClient))
                .OrderByDescending(q => q.BonLivraison.Date).Take(1).FirstOrDefault();
            if (bi != null)
                price = bi.Pu;
            else
                price = db.Articles.Find(IdArticle).PVD ?? 0;

            return this.Json(price, JsonRequestBehavior.AllowGet);
        }
    }
}