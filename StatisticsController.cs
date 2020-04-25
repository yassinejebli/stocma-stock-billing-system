using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApplication1
{
    public class StatisticsController : Controller
    {
        public ActionResult LowStockArticles(int IdSite)
        {
            ArticlesStatistics statistics = new ArticlesStatistics();
            return Json(statistics.LowStockArticlesCount(IdSite), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TotalStock(int IdSite)
        {
            ArticlesStatistics statistics = new ArticlesStatistics();
            return Json(statistics.TotalStock(IdSite), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ArticlesWithMargin(int IdSite, DateTime? From, DateTime? To)
        {
            ArticlesStatistics statistics = new ArticlesStatistics();
            return Json(statistics.ArticlesWithMargin(IdSite, From, To), JsonRequestBehavior.AllowGet);
        }
    }
}