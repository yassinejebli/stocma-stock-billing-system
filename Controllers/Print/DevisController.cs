using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DATA;

namespace WebApplication1.Controllers.Print
{
    public class DevisController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index(Guid IdDevis, bool? showPrices = true, bool? cachet = false)
        {
            var DevisById = context.Devises.Where(x => x.Id == IdDevis).FirstOrDefault();
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string company = StatistiqueController.getCompanyName().ToUpper();

            reportDocument.Load(
                Path.Combine(
                    this.Server.MapPath("~/CrystalReports/" + company + "/Devis" + company +
                                        ".rpt")));


            ////////////////////////
            reportDocument.SetDataSource(
                DevisById.DevisItems.Select(x => new
                {
                    NumBon = x.Devis.NumBon,
                    Date = x.Devis.Date,
                    Client = x.Devis.Client.Name,
                    Ref = x.Article.Ref,
                    Designation = x.Article.Designation,
                    Qte = x.Qte,
                    PU = x.Pu,
                    TotalHT = x.TotalHT,
                    TVA = x.Article.TVA ?? 20,
                    Unite = x.Article.Unite,
                    Adresse = x.Devis.Client.Adresse,
                    DelaiLivraison = x.Devis.DelaiLivrasion,
                    TransportExpedition = x.Devis.TransportExpedition,
                    TypeReglement = x.Devis.TypeReglement,
                    ValiditeOffre = x.Devis.ValiditeOffre
                }).ToList()
             );
         
            if (reportDocument.ParameterFields["ShowPrices"] != null)
                reportDocument.SetParameterValue("ShowPrices", showPrices);
            if (reportDocument.ParameterFields["Cachet"] != null)
                reportDocument.SetParameterValue("Cachet", cachet);

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Devis.pdf",
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