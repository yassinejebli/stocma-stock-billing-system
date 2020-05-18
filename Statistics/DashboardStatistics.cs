﻿using System;
using System.Collections;
using System.Linq;
using WebApplication1.DATA;

namespace WebApplication1.Statistics
{
    //TODO: Add expenses
    public class DashboardStatistics
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        public IEnumerable ProfitAndTurnover(int IdSite)
        {
            var currentYear = DateTime.Now.Year;

            return db.BonLivraisonItems.Where(x => x.BonLivraison.Date.Year == currentYear)
                .GroupBy(x => new { Month = x.BonLivraison.Date.Month, Year = x.BonLivraison.Date.Year })
                .Select(x => new { year = x.Key.Year, month = x.Key.Month, turnover = x.Sum(y => y.Qte * y.Pu), Profit = x.Sum(y => y.Qte * (y.Pu - y.PA)) });
        }
    }
}