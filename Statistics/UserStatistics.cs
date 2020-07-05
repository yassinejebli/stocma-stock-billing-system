using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using WebApplication1.DATA;

namespace WebApplication1.Statistics
{
    //TODO: Add expenses
    public class UserStatistics
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        public IEnumerable SalesByUser(DateTime dateFrom, DateTime dateTo)
        {
            return db.BonLivraisons.Where(x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo && !string.IsNullOrEmpty(x.IdUser))
                .GroupBy(x => new { IdUser = x.IdUser, User = x.User })
                .Select(x => new
                {
                    user = x.Key.User,
                    totalSales = x.SelectMany(y=>y.BonLivraisonItems).Sum(y=>(float?)(y.Qte * y.Pu)) ?? 0,
                    totalProfit = x.SelectMany(y => y.BonLivraisonItems).Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0,
                });
        }
    }
}