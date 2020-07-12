using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.DATA;

namespace WebApplication1.Statistics
{
    public class ClientStatistics
    {
        MySaniSoftContext db = new MySaniSoftContext();

        public dynamic ClientsProfit(int Skip, string SearchText, DateTime? From, DateTime? To)
        {
            if (!From.HasValue) From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (!To.HasValue) To = DateTime.Now;
            var clientsProfit = db.Clients.Where(x => !x.Disabled).Select(x => new
            {
                client = x.Name,
                turnover = x.BonLivraisons.SelectMany(y=>y.BonLivraisonItems).Where(y => y.BonLivraison.Date >= From && y.BonLivraison.Date <= To).
                Sum(y => (float?)(y.Qte * y.Pu)) ?? 0f,
                margin = x.BonLivraisons.SelectMany(y => y.BonLivraisonItems).Where(y => y.BonLivraison.Date >= From && y.BonLivraison.Date <= To)
                .Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0f,
                solde = x.IsClientDivers ? 0 : x.Paiements.Sum(y=> (float?)(y.Debit - y.Credit)) ?? 0f,
            }).Where(x => x.turnover > 0 && (SearchText == "" || x.client.ToLower().Contains(SearchText.ToLower())));

            var totalItems = clientsProfit.Count();

            return new { data = clientsProfit.OrderByDescending(x => x.margin).Skip(Skip).Take(10), totalItems };
        }
    }
}