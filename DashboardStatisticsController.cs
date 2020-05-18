using System.Web.Mvc;
using WebApplication1.Statistics;

namespace WebApplication1
{
    public class DashboardStatisticsController : Controller
    {
        public ActionResult MonthlyProfitAndTurnover(int IdSite = 1)
        {
            DashboardStatistics statistics = new DashboardStatistics();
            return Json(statistics.MonthlyProfitAndTurnover(IdSite), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DailyProfitAndTurnover(int IdSite = 1)
        {
            DashboardStatistics statistics = new DashboardStatistics();
            return Json(statistics.DailyProfitAndTurnover(IdSite), JsonRequestBehavior.AllowGet);
        }
    }
}