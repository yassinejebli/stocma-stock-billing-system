using Microsoft.AspNet.OData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.DATA;

namespace WebApplication1
{
    public class ArticlesStatistics
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        public int LowStockArticlesCount(int IdSite)
        {
            var counter = db.ArticleSites
                .Where(x => x.QteStock < x.Article.MinStock && x.IdSite == IdSite && !x.Disabled && !x.Site.Disabled)
                .Count();

            return counter;
        }

        public float TotalStock(int IdSite)
        {
            var counter = db.ArticleSites
                .Where(x => x.QteStock > 0 && x.IdSite == IdSite && !x.Disabled && !x.Site.Disabled)
                .Sum(x => (float?)(x.QteStock * x.Article.PA)) ?? 0;

            return counter;
        }

        public float TotalStockFacture()
        {
            /* var counter = db.ArticleFactures
                 .Where(x => x.QteStock > 0 && !x.Disabled)
                 .Sum(x => (float?)(x.QteStock * (x.PA - (x.PA * x.TVA/100)))) ?? 0;*/

            var counter = db.ArticleFactures
               .Where(x => x.QteStock > 0 && !x.Disabled)
               .Sum(x => (float?)(x.QteStock * x.PA / (1 + (x.TVA / 100)))) ?? 0;

            return counter;
        }

        public dynamic ArticlesWithMargin(int IdSite, int Skip, string SearchText, DateTime? From, DateTime? To)
        {
            if (!From.HasValue) From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (!To.HasValue) To = DateTime.Now;
            var articlesWithMargin = db.ArticleSites.Where(x => !x.Disabled && !x.Site.Disabled).Select(x => new
            {
                Article = x.Article.Designation,
                QteStock = x.QteStock,
                QteSold = x.Article.BonLivraisonItems.Where(y => y.BonLivraison.Date >= From && y.BonLivraison.Date <= To).Sum(y => (int?)y.Qte) ?? 0 -
                x.Article.BonAvoirCItems.Where(y => y.BonAvoirC.Date >= From && y.BonAvoirC.Date <= To).Sum(y => (int?)y.Qte) ?? 0,
                PA = x.Article.PA,
                Turnover = x.Article.BonLivraisonItems.Where(y => y.BonLivraison.Date >= From && y.BonLivraison.Date <= To).
                Sum(y => (float?)(y.Qte * y.Pu)) ?? 0f
                - x.Article.BonAvoirCItems.Where(y => y.BonAvoirC.Date >= From && y.BonAvoirC.Date <= To).
                Sum(y => (float?)(y.Qte * y.Pu)) ?? 0f,
                Margin = x.Article.BonLivraisonItems.Where(y => y.BonLivraison.Date >= From && y.BonLivraison.Date <= To)
                .Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0f
                - x.Article.BonAvoirCItems.Where(y => y.BonAvoirC.Date >= From && y.BonAvoirC.Date <= To)
                .Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0f,
            }).Where(x => x.Turnover > 0 && (SearchText == "" || x.Article.ToLower().Contains(SearchText.ToLower())));

            var totalItems = articlesWithMargin.Count();


            return new { data = articlesWithMargin.OrderByDescending(x => x.Margin).Skip(Skip).Take(10), totalItems };
        }

        public dynamic ArticlesNotSellingIn(int IdSite, int Skip, string SearchText, int Months)
        {
            var today = DateTime.Now;
            var dateBeforeNmonths = DateTime.Now.AddMonths(-Months);
            var articles = db.ArticleSites.Where(x => x.IdSite == IdSite && x.QteStock > 0.0f && !x.Article.BonLivraisonItems.Where(y => y.BonLivraison.Date > dateBeforeNmonths).Any() && (SearchText == "" || x.Article.Designation.ToLower().Contains(SearchText.ToLower())))
                .Select(x => new
                {
                    Article = x.Article.Designation,
                    QteStock = x.QteStock,
                    Unite = x.Article.Unite,
                    PA = x.Article.PA,
                    PVD = x.Article.PVD,
                    Total = x.Article.PA * x.QteStock,
                });

            var totalItems = articles.Count();


            return new { data = articles.OrderByDescending(x => x.Total).Skip(Skip).Take(10), totalItems };
        }
    }
}