using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using WebApplication1.DATA;

namespace WebApplication1.Statistics
{
    //TODO: Add expenses
    public class DashboardStatistics
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        public IEnumerable MonthlyProfitAndCash(int year)
        {

            var months = Enumerable.Range(1, 12).Select(i => new { I = i, M = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) });
            var monthlyProfitAndCash = months.Select(x =>
            {
                var totalCashPaymentsClients = db.Paiements.Where(y => y.Date.Year == year && y.Date.Month == x.I && (y.TypePaiement.IsEspece || y.TypePaiement.IsRemboursement || (y.TypePaiement.IsBankRelated && y.EnCaisse == true))).Sum(y => (float?)(y.Credit - y.Debit)) ?? 0;
                var totalCashPaymentsFournisseurs = db.PaiementFs.Where(y => y.Date.Year == year && y.Date.Month == x.I && (y.TypePaiement.IsEspece || y.TypePaiement.IsRemboursement || (y.TypePaiement.IsBankRelated && y.EnCaisse == true))).Sum(y => (float?)(y.Credit - y.Debit)) ?? 0;
                var totalExpenses = db.Depenses.Where(y => y.Date.Year == year && y.Date.Month == x.I).SelectMany(y => y.DepenseItems).Sum(y => (float?)y.Montant) ?? 0;
                return new
                {
                    month = x.I,
                    cashProfit = totalCashPaymentsClients,
                    expenses = totalExpenses + totalCashPaymentsFournisseurs,
                    netProfit = totalCashPaymentsClients - totalExpenses - totalCashPaymentsFournisseurs,
                };
            }).OrderBy(x => x.month);

            return monthlyProfitAndCash;
        }

        public IEnumerable MonthlyProfitAndTurnover(int IdSite, int year)
        {
            var useVat = db.Companies.FirstOrDefault().UseVAT;
            return db.BonLivraisonItems.Where(x => x.BonLivraison.Date.Year == year)
                .GroupBy(x => new { Month = x.BonLivraison.Date.Month, Year = x.BonLivraison.Date.Year })
                .Select(x => new
                {
                    year = x.Key.Year,
                    month = x.Key.Month,
                    expense = db.DepenseItems.Where(y => y.Depense.Date.Year == year && y.Depense.Date.Month == x.Key.Month).Sum(y => (float?)y.Montant) ?? 0,
                    turnover = x.Sum(y => (float?)(y.Qte * y.Pu)) ?? 0 - db.BonAvoirCItems.Where(y => y.BonAvoirC.Date.Year == year && y.BonAvoirC.Date.Month == x.Key.Month).Sum(y => (float?)(y.Qte * y.Pu)) ?? 0,
                    remise = db.Paiements.Where(y => y.Date.Year == year && y.Date.Month == x.Key.Month && y.TypePaiement.IsRemise).Sum(y => (float?)y.Credit) ?? 0,
                    profit = x.Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0 - db.BonAvoirCItems.Where(y => y.BonAvoirC.Date.Year == year && y.BonAvoirC.Date.Month == x.Key.Month).Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0,
                }).Select(x => new
                {
                    year = x.year,
                    month = x.month,
                    expense = x.expense,
                    turnover = x.turnover,
                    profit = x.profit,
                    netProfit = x.profit - x.expense - x.remise
                }).OrderBy(x => x.month);
        }

        public IEnumerable DailyProfitAndTurnover(int IdSite, int year)
        {
            var currentMonth = DateTime.Now.Month;

            return db.BonLivraisonItems.Where(x => x.BonLivraison.Date.Year == year && x.BonLivraison.Date.Month == currentMonth)
                .GroupBy(x => new { Day = x.BonLivraison.Date.Day })
                .Select(x => new
                {
                    day = x.Key.Day,
                    expense = db.DepenseItems.Where(y => y.Depense.Date.Year == year && y.Depense.Date.Month == currentMonth && y.Depense.Date.Day == x.Key.Day).Sum(y => (float?)y.Montant) ?? 0,
                    turnover = x.Sum(y => (float?)(y.Qte * y.Pu)) ?? 0 - db.BonAvoirCItems.Where(y => y.BonAvoirC.Date.Day == x.Key.Day && y.BonAvoirC.Date.Year == year && y.BonAvoirC.Date.Month == currentMonth).Sum(y => (float?)(y.Qte * y.Pu)) ?? 0,
                    remise = db.Paiements.Where(y => y.Date.Year == year && y.Date.Month == currentMonth && y.Date.Day == x.Key.Day && y.TypePaiement.IsRemise).Sum(y => (float?)y.Credit) ?? 0,
                    profit = x.Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0 - db.BonAvoirCItems.Where(y => y.BonAvoirC.Date.Day == x.Key.Day && y.BonAvoirC.Date.Year == year && y.BonAvoirC.Date.Month == currentMonth).Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0,
                }).Select(x => new
                {
                    day = x.day,
                    expense = x.expense,
                    turnover = x.turnover,
                    profit = x.profit,
                    netProfit = x.profit - x.expense - x.remise
                }).OrderBy(x => x.day);
        }


    }
}