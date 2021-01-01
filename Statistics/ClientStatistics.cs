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
            var useVAT = db.Companies.FirstOrDefault().UseVAT;
            var clientsProfit = db.Clients.Where(x => !x.Disabled).Select(x => new
            {
                client = x.Name,
                turnover = x.BonLivraisons.SelectMany(y => y.BonLivraisonItems).Where(y => y.BonLivraison.Date >= From && y.BonLivraison.Date <= To).
                Sum(y => (float?)(y.Qte * y.Pu)) ?? 0f -
                x.BonAvoirCs.SelectMany(y => y.BonAvoirCItems).Where(y => y.BonAvoirC.Date >= From && y.BonAvoirC.Date <= To).
                Sum(y => (float?)(y.Qte * y.Pu)) ?? 0f,
                margin = x.BonLivraisons.SelectMany(y => y.BonLivraisonItems).Where(y => y.BonLivraison.Date >= From && y.BonLivraison.Date <= To)
                .Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0f -
                x.BonAvoirCs.SelectMany(y => y.BonAvoirCItems).Where(y => y.BonAvoirC.Date >= From && y.BonAvoirC.Date <= To)
                .Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0f,
                solde = x.IsClientDivers ? 0 : (useVAT ? x.PaiementFactures.Sum(y => (float?)(y.Debit - y.Credit)) ?? 0f : x.Paiements.Sum(y => (float?)(y.Debit - y.Credit)) ?? 0f),
            }).Where(x => x.turnover > 0 && (SearchText == "" || x.client.ToLower().Contains(SearchText.ToLower())));

            var totalItems = clientsProfit.Count();

            return new { data = clientsProfit.OrderByDescending(x => x.margin).Skip(Skip).Take(10), totalItems };
        }

        public dynamic ClientsNotBuying(int Skip, string SearchText, int Months)
        {
            var dateBeforeNmonths = DateTime.Now.AddMonths(-Months);
            var useVAT = db.Companies.FirstOrDefault().UseVAT;

            var clients = db.Clients.Where(x => !x.Disabled && !x.BonLivraisons.Where(y => y.Date > dateBeforeNmonths).Any() && (SearchText == "" || x.Name.ToLower().Contains(SearchText.ToLower())))
                .Select(x => new
                {
                    client = x.Name,
                    adresse = x.Adresse,
                    email = x.Email,
                    solde = x.IsClientDivers ? 0 : (useVAT ? x.PaiementFactures.Sum(y => (float?)(y.Debit - y.Credit)) ?? 0f :  x.Paiements.Sum(y => (float?)(y.Debit - y.Credit)) ?? 0f),
                    turnover = x.BonLivraisons.SelectMany(y => y.BonLivraisonItems)
                                .Sum(y => (float?)(y.Qte * y.Pu)) ?? 0f -
                                x.BonAvoirCs.SelectMany(y => y.BonAvoirCItems)
                                .Sum(y => (float?)(y.Qte * y.Pu)) ?? 0f,
                    margin = x.BonLivraisons.SelectMany(y => y.BonLivraisonItems)
                            .Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0f -
                            x.BonAvoirCs.SelectMany(y => y.BonAvoirCItems)
                            .Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0f,
                });

            var totalItems = clients.Count();

            return new { data = clients.OrderByDescending(x => x.margin).Skip(Skip).Take(10), totalItems };
        }
    }
}