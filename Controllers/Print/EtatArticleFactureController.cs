using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DATA;

namespace WebApplication1.Controllers.Print
{
    [Authorize]
    public class EtatArticleFactureController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index()
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string company = StatistiqueController.getCompanyName().ToUpper();


            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/" + company + "/RapportArticleFacture.rpt")));


            //(x.QteStock * x.PA / (1 + (x.TVA / 100)))) ?? 0;

            // TODO: Fix HT calculation
            reportDocument.SetDataSource(context.ArticleFactures.Where(x=>x.QteStock>0)
                .Select(x => new
                {
                    Client = "",
                    NumBon = x.Designation,
                    Date = DateTime.Now,
                    Debit = x.QteStock,
                    //{Facture.PU}/(1+{Facture.TVA}/100)
                    Credit = (x.PA / (1 + (x.TVA ?? 20 / 100))),
                    //Credit = x.PA / 1.2,
                    //16 / (1 +(20/100))
                    Total = x.QteStock*(x.PA / 1.2),
                    Type = "",
                    DateEcheance = "",
                    Commentaire = "",
                    ICE ="",
                    Adresse = "",
                }).ToList());

            reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
            reportDocument.PrintOptions.ApplyPageMargins(new PageMargins(0, 0, 0, 0));

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "EtatArticleFacture.pdf",
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