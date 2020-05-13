using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DATA;
using WebApplication1.Generators;

namespace WebApplication1.Controllers.Print
{
    [Authorize]
    public class FactureController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index(Guid IdFacture, bool? cachet = false)
        {
            AmountTextGenerator atg = new AmountTextGenerator();
            string totalMots = "";
            var company = StatistiqueController.getCompany();
            var useVAT = company.UseVAT;
            var FactureById = context.Factures.Where(x => x.Id == IdFacture).FirstOrDefault();
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();

            reportDocument.Load(
                Path.Combine(
                    this.Server.MapPath("~/CrystalReports/" + company.Name + "/Facture" + company.Name +
                                        ".rpt")));

            if (!useVAT)
            {
                reportDocument.SetDataSource(
                FactureById.FactureItems.Select(x => new
                {
                    NumBon = x.Facture.NumBon,
                    Date = x.Facture.Date,
                    Client = x.Facture.Client.Name,
                    Ref = x.Article.Ref,
                    Designation = x.Article.Designation,
                    Qte = x.Qte,
                    PU = x.Pu,
                    TVA = x.Article.TVA ?? 20.0f,
                    Unite = x.Article.Unite,
                    Adresse = x.Facture.Client.Adresse,
                    ClientName = x.Facture.ClientName,
                    TypeReglement = x.Facture.TypeReglement,
                    Comment = x.Facture.Comment,
                    NumBL = "",
                    NumBC = "",
                    ICE = x.Facture.Client.ICE
                }).ToList()
             );

                Decimal total = (Decimal)FactureById.BonLivraisons
                    .SelectMany(x => x.BonLivraisonItems)
                    .Sum(x => x.Qte * x.Pu);
                totalMots = atg.DecimalToWords(total);
            }
            else
            {
                reportDocument.SetDataSource(
                FactureById.BonLivraisons.SelectMany(x => x.BonLivraisonItems).Select(x => new
                {
                    NumBon = x.BonLivraison.Facture.NumBon,
                    Date = x.BonLivraison.Facture.Date,
                    Client = x.BonLivraison.Facture.Client.Name,
                    Ref = x.Article.Ref,
                    Designation = x.Article.Designation,
                    Qte = x.Qte,
                    PU = x.Pu,
                    TVA = x.Article.TVA ?? 20.0f,
                    Unite = x.Article.Unite,
                    Adresse = x.BonLivraison.Facture.Client.Adresse,
                    ClientName = x.BonLivraison.Facture.ClientName,
                    TypeReglement = x.BonLivraison.Facture.TypeReglement,
                    Comment = x.BonLivraison.Facture.Comment,
                    NumBL = x.BonLivraison.NumBon,
                    NumBC = "",
                    ICE = x.BonLivraison.Facture.Client.ICE
                }).ToList()
             );

                Decimal total = (Decimal)FactureById.BonLivraisons
                    .SelectMany(x => x.BonLivraisonItems)
                    .Sum(x => x.Qte * (x.Pu + (x.Pu * x.Article.TVA / 100)));
                totalMots = atg.DecimalToWords(total);
            }


            if (reportDocument.ParameterFields["totalMots"] != null)
                reportDocument.SetParameterValue("totalMots", totalMots);
            if (reportDocument.ParameterFields["Cachet"] != null)
                reportDocument.SetParameterValue("Cachet", cachet);

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Facture.pdf",
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