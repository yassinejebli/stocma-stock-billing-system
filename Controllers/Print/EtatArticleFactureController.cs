using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
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


            reportDocument.SetDataSource(context.ArticleFactures.Where(x=>x.QteStock>0)
                .Select(x => new
                {
                    Client = "",
                    NumBon = x.Designation,
                    Date = DateTime.Now,
                    Debit = x.QteStock,
                    Credit = x.PA-(x.PA*(x.TVA??20)/100),
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