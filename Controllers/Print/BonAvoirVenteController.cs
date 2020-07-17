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
    public class BonAvoirVenteController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index(Guid id, bool showBalance = false, bool showStamp = false)
        {
            var DocumentById = context.BonAvoirCs.Where(x => x.Id == id).FirstOrDefault();
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string company = StatistiqueController.getCompanyName().ToUpper();

            reportDocument.Load(
                Path.Combine(
                    this.Server.MapPath("~/CrystalReports/" + company + "/BonAvoirVente.rpt")));


            reportDocument.SetDataSource(
                DocumentById.BonAvoirCItems.Select(x => new
                {
                    NumBon = x.BonAvoirC.NumBon,
                    Date = x.BonAvoirC.Date,
                    CodeClient = x.BonAvoirC.Client.Code,
                    Client = x.BonAvoirC.Client.Name,
                    Ref = x.Article.Ref,
                    Designation = x.Article.Designation,
                    Qte = x.Qte,
                    PU = x.Pu,
                    TotalHT = x.Qte * x.Pu,
                    Unite = x.Article.Unite,
                    Adresse = x.BonAvoirC.Client.Adresse,
                    NumBL = x.NumBL ?? "",
                    TVA = x.Article.TVA ?? 20
                }).ToList()
             );

            if (reportDocument.ParameterFields["showStamp"] != null)
                reportDocument.SetParameterValue("showStamp", showStamp);

            reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
            reportDocument.PrintOptions.ApplyPageMargins(new PageMargins(0, 0, 0, 0));

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "BAV.pdf",
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