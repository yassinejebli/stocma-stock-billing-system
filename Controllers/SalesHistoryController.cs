using System;
using System.Linq;
using System.Web.Http.Results;
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

        public ActionResult getPriceLastPurchase(Guid IdFournisseur, Guid IdArticle)
        {
            var price = 0.0f;
            var bi = db.BonReceptionItems
                .Where((x => x.IdArticle == IdArticle && x.BonReception.IdFournisseur == IdFournisseur))
                .OrderByDescending(q => q.BonReception.Date).Take(1).FirstOrDefault();
            if (bi != null)
                price = bi.Pu;
            else
                price = db.Articles.Find(IdArticle).PA;

            return this.Json(price, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArticleByBarCode(string BarCode, Guid? IdClient)
        {
            var article = db.Articles.ToList().FirstOrDefault(x => x.BarCode == BarCode);
            if (IdClient.HasValue && article != null)
            {
                var lastSoldeTime = article.BonLivraisonItems.Where(x => x.BonLivraison.IdClient == IdClient && x.IdArticle == article.Id)
                .OrderByDescending(x => x.BonLivraison.Date).Take(1).FirstOrDefault();
                if (lastSoldeTime != null)
                    article.PVD = lastSoldeTime.Pu;
            }
            if(article == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                article.BarCode,
                article.Designation,
                article.Id,
                article.PVD,
                article.MinStock,
                article.PA,
                article.Ref,
            }, JsonRequestBehavior.AllowGet);
        }
    }
}