using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using WebApplication1.Statistics;

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

        public ActionResult TotalStockFacture()
        {
            ArticlesStatistics statistics = new ArticlesStatistics();
            return Json(statistics.TotalStockFacture(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ArticlesWithMargin(int IdSite, int Skip, string SearchText, DateTime? From, DateTime? To)
        {
            ArticlesStatistics statistics = new ArticlesStatistics();
            return Json(statistics.ArticlesWithMargin(IdSite, Skip, SearchText, From, To), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClientsProfit(int Skip, string SearchText, DateTime? From, DateTime? To)
        {
            ClientStatistics statistics = new ClientStatistics();
            return Json(statistics.ClientsProfit(Skip, SearchText, From, To), JsonRequestBehavior.AllowGet);
        }

    }
}