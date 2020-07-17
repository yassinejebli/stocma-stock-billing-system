using System.Web.Mvc;
using WebApplication1.Statistics;

namespace WebApplication1
{
    public class DashboardStatisticsController : Controller
    {
        public ActionResult MonthlyProfitAndTurnover(int year, int IdSite = 1)
        {
            DashboardStatistics statistics = new DashboardStatistics();
            return Json(statistics.MonthlyProfitAndTurnover(IdSite, year), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DailyProfitAndTurnover(int year, int IdSite = 1)
        {
            DashboardStatistics statistics = new DashboardStatistics();
            return Json(statistics.DailyProfitAndTurnover(IdSite, year), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MonthlyProfitAndCash(int year)
        {
            DashboardStatistics statistics = new DashboardStatistics();
            return Json(statistics.MonthlyProfitAndCash(year), JsonRequestBehavior.AllowGet);
        }
    }
}