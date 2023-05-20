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
    public class SitutationJournaliereController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index(DateTime dt)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/SituationJournaliere.rpt")));


            if (context.Paiements.Select(x => new
            {
                Date = x.Date,
                Debit = x.Debit,
                Credit = x.Credit,
                Type = x.TypePaiement.Name,
                Client = x.Client.Name,
                NumBon = x.BonLivraison.NumBon ?? "",
                Commentaire = x.Comment,
            }).Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date.Day == dt.Day).Count() > 0)
            {
                reportDocument.SetDataSource(
                 context.Paiements.Select(x => new
                 {
                     Date = x.Date,
                     Debit = x.Debit,
                     Credit = x.Credit,
                     Type = x.TypePaiement.Name,
                     Client = x.Client.Name,
                     NumBon = x.BonLivraison.NumBon ?? "",
                     Commentaire = x.Comment,
                 }).Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date.Day == dt.Day).OrderBy(x => x.Type).ThenBy(x => x.Date).ToList());
                reportDocument.SetParameterValue("date", dt);
                reportDocument.SetParameterValue("Marge", statistiqueController.getMargeByDate(dt));
                reportDocument.SetParameterValue("Depense", statistiqueController.getTotalDepenseByDate(dt));
                reportDocument.SetParameterValue("Caisse", statistiqueController.MontantALaCaisse(dt));
                this.Response.Buffer = false;
                this.Response.ClearContent();
                this.Response.ClearHeaders();
                Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0L, SeekOrigin.Begin);
                reportDocument.Close();
                return File(stream, "application/pdf");
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}