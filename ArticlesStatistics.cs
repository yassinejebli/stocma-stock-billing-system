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
                .Where(x => x.QteStock < 0 && x.IdSite == IdSite && !x.Disabled && !x.Site.Disabled)
                .Count();

            return counter;
        }

        public float TotalStock(int IdSite)
        {
            var counter = db.ArticleSites
                .Where(x =>x.QteStock > 0 && x.IdSite == IdSite && !x.Disabled && !x.Site.Disabled)
                .Sum(x => (float?)(x.QteStock * x.Article.PA)) ?? 0;

            return counter;
        }

        public IEnumerable ArticlesWithMargin(int IdSite, DateTime? From, DateTime? To)
        {
            if (!From.HasValue) From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (!To.HasValue) To = DateTime.Now;
            var articlesWithMargin = db.ArticleSites.Where(x => x.IdSite == IdSite && !x.Disabled && !x.Site.Disabled).Select(x => new
            {
                Article = x.Article.Designation,
                QteStock = x.QteStock,
                QteSold = x.Article.BonLivraisonItems.Sum(y=>(int?)y.Qte) ?? 0,
                PA = x.Article.PA,
                //PVD = x.Article.PVD,
                Marge = x.Article.BonLivraisonItems.Where(y=>y.BonLivraison.Date >= From && y.BonLivraison.Date <= To)
                .Sum(y => (float?)(y.Qte * (y.Pu - y.PA))) ?? 0f
            }).Where(x=>x.Marge > 0).OrderByDescending(x=>x.Marge).ToList();

            return articlesWithMargin;
        }
    }
}