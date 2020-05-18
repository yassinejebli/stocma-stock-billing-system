using System.Web.Mvc;
using WebApplication1.Statistics;

namespace WebApplication1
{
    public class DashboardStatisticsController : Controller
    {
        public ActionResult ProfitAndTurnover(int IdSite = 1)
        {
            DashboardStatistics statistics = new DashboardStatistics();
            return Json(statistics.ProfitAndTurnover(IdSite), JsonRequestBehavior.AllowGet);
        }
    }
}