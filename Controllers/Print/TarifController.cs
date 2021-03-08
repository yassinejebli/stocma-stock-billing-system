using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using WebApplication1.DATA;
using WebGrease.Css.Extensions;

namespace WebApplication1.Controllers.Print
{
    public class TarifController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index(Guid id)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/Tarif.rpt")));
            reportDocument.SetDataSource(
                context.TarifItems.Where(
                    (x => x.Tarif.Id == id)).Select(x => new
                    {
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Date = x.Tarif.Date,
                        Pu = x.Pu,
                        Pu2 = x.Pu2,
                        Pa = x.Article.PA,
                        Unite = x.Article.Unite,
                        Titre = x.Tarif.Ref,
                        Qte = x.Article.QteStock
                    }).OrderBy(x => x.Designation).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        
    }
}