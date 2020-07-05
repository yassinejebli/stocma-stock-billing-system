using System;
using System.Web.Mvc;
using WebApplication1.Statistics;

namespace WebApplication1
{
    public class UserStatisticsController : Controller
    {
        public ActionResult SalesByUser(DateTime dateFrom, DateTime dateTo)
        {
            UserStatistics statistics = new UserStatistics();
            return Json(statistics.SalesByUser(dateFrom, dateTo), JsonRequestBehavior.AllowGet);
        }
    }
}