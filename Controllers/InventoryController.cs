using System;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;
using WebApplication1.DATA;

namespace WebApplication1.Controllers
{
    public class InventoryController : Controller
    {
        MySaniSoftContext db = new MySaniSoftContext();

        public dynamic InventaireList(int idSite, int limit)
        {
            var dateBefore3Months = DateTime.Now.AddDays(-90);
            var articles = db.ArticleSites.Where(x => x.IdSite == idSite && !x.Disabled).ToList().Select(x =>
            {
                var lastInvetoryDate = x.Article.InventaireItems.Where(y => y.Inventaire.IdSite == idSite)
                .OrderByDescending(y => y.Inventaire.Date).FirstOrDefault();
                return new
                {
                    Article = new
                    {
                        x.Article.Id,
                        x.Article.Designation,
                        x.Article.Ref,
                        x.QteStock,
                        x.Article.Unite,
                        x.Article.BarCode,
                        x.Article.Categorie,
                    },
                    Count = x.Article.BonLivraisonItems.Where(y => y.BonLivraison.Date >= dateBefore3Months).Count(),
                    Date = lastInvetoryDate != null ? lastInvetoryDate.Inventaire.Date.Millisecond : 0
                };
            }).OrderBy(x => x.Date).ThenByDescending(x => x.Count).Take(limit);

            return articles;
        }

        public ActionResult getInventaireList(int idSite, int limit)
        {
            return this.Json(InventaireList(idSite, limit), JsonRequestBehavior.AllowGet);
        }
    }
}
 