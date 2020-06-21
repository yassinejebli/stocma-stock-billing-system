using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.OData;

namespace WebApplication1.DATA.OData
{
    //[Authorize]
    public class SuiviVentesController : Controller
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonLivraisonItem> Index()
        {
            var bonLivraisonItems = db.BonLivraisonItems.OrderByDescending(x=>x.BonLivraison.Date);

            return bonLivraisonItems;
        }
    }
}
