using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DATA;

namespace WebApplication1.Controllers.Print
{
    public class BonCommandeController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index(Guid id, bool hidePrices = false, bool showStamp = false)
        {
            var DocumentById = context.BonCommandes.Where(x => x.Id == id).FirstOrDefault();
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string company = StatistiqueController.getCompanyName().ToUpper();

           if(hidePrices)
            reportDocument.Load(
                Path.Combine(
                    this.Server.MapPath("~/CrystalReports/" + company + "/BonCommande.rpt")));
           else
                reportDocument.Load(
                Path.Combine(
                    this.Server.MapPath("~/CrystalReports/" + company + "/BonCommandeChiffre.rpt")));

            reportDocument.SetDataSource(
                DocumentById.BonCommandeItems.Select(x => new
                {
                    NumBon = x.BonCommande.NumBon,
                    Date = x.BonCommande.Date,
                    Client = x.BonCommande.Fournisseur.Name,
                    Ref = x.Article.Ref,
                    Designation = x.Article.Designation,
                    Qte = x.Qte,
                    PU = x.Pu,
                    TotalHT = x.TotalHT,
                    TVA = x.Article.TVA ?? 20,
                    Unite = x.Article.Unite,
                    Adresse = x.BonCommande.Fournisseur.Adresse,
                }).ToList()
             );

            if (reportDocument.ParameterFields["showStamp"] != null)
                reportDocument.SetParameterValue("showStamp", showStamp);

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "BC.pdf",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }
    }
}