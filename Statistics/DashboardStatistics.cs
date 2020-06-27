using System;
using System.Collections;
using System.Linq;
using WebApplication1.DATA;

namespace WebApplication1.Statistics
{
    //TODO: Add expenses
    public class DashboardStatistics
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        public IEnumerable MonthlyProfitAndTurnover(int IdSite)
        {
            var currentYear = DateTime.Now.Year;
            return db.BonLivraisonItems.Where(x => x.BonLivraison.Date.Year == currentYear)
                .GroupBy(x => new { Month = x.BonLivraison.Date.Month, Year = x.BonLivraison.Date.Year })
                .Select(x => new
                {
                    year = x.Key.Year,
                    month = x.Key.Month,
                    expense = db.DepenseItems.Where(y=>y.Depense.Date.Year == currentYear && y.Depense.Date.Month == x.Key.Month).Sum(y=>(float?)y.Montant) ?? 0,
                    turnover = x.Sum(y => (float?)(y.Qte * y.Pu)) ?? 0 - db.BonAvoirCItems.Where(y => y.BonAvoirC.Date.Year == currentYear && y.BonAvoirC.Date.Month == x.Key.Month).Sum(y => (float?)(y.Qte * y.Pu)) ?? 0,
                    profit = x.Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0 - db.BonAvoirCItems.Where(y => y.BonAvoirC.Date.Year == currentYear && y.BonAvoirC.Date.Month == x.Key.Month).Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0,
                }).Select(x=> new
                {
                    year = x.year,
                    month = x.month,
                    expense = x.expense,
                    turnover = x.turnover,
                    profit = x.profit,
                    netProfit = x.profit - x.expense
                });
        }

        public IEnumerable DailyProfitAndTurnover(int IdSite)
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            return db.BonLivraisonItems.Where(x => x.BonLivraison.Date.Year == currentYear && x.BonLivraison.Date.Month == currentMonth)
                .GroupBy(x => new { Day = x.BonLivraison.Date.Day })
                .Select(x => new
                {
                    day = x.Key.Day,
                    expense = db.DepenseItems.Where(y=>y.Depense.Date.Year == currentYear && y.Depense.Date.Month == currentMonth && y.Depense.Date.Day == x.Key.Day).Sum(y=>(float?)y.Montant) ?? 0,
                    turnover = x.Sum(y => (float?)(y.Qte * y.Pu)) ?? 0 - db.BonAvoirCItems.Where(y=>y.BonAvoirC.Date.Day == x.Key.Day && y.BonAvoirC.Date.Year == currentYear && y.BonAvoirC.Date.Month == currentMonth).Sum(y=> (float?)(y.Qte*y.Pu)) ?? 0,
                    profit = x.Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0 - db.BonAvoirCItems.Where(y => y.BonAvoirC.Date.Day == x.Key.Day && y.BonAvoirC.Date.Year == currentYear && y.BonAvoirC.Date.Month == currentMonth).Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0,
                }).Select(x=> new
                {
                    day = x.day,
                    expense = x.expense,
                    turnover = x.turnover,
                    profit = x.profit,
                    netProfit = x.profit - x.expense
                });
        }
    }
}