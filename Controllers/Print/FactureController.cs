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
            var ESPECE_PAYMENT_TYPE = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");
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
                var facturePrintData = FactureById.FactureItems.Select(x => new
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
                    TypeReglement = x.Facture.TypePaiement?.Name,
                    Comment = x.Facture.Comment,
                    NumBL = "",
                    NumBC = "",
                    ICE = x.Facture.Client.ICE,
                    TotalHT = x.TotalHT,
                    CodeClient = x.Facture.Client.Code,
                    Discount = x.Discount + (x.PercentageDiscount ? "%" : ""),
                    Total = (x.Qte * x.Pu) - (x.PercentageDiscount ? (x.Qte * x.Pu * (x.Discount ?? 0.0f) / 100) : x.Discount ?? 0.0f),
                }).OrderBy(x=>x.Designation).ToList();
                reportDocument.SetDataSource(facturePrintData);

                float total = facturePrintData.Sum(x => x.Total * (1 + x.TVA / 100));

                totalMots = atg.DecimalToWords(Math.Round((Decimal)total, 2, MidpointRounding.AwayFromZero));
            }
            else
            {
                var facturePrintData = FactureById.BonLivraisons.SelectMany(x => x.BonLivraisonItems).Select(x => new
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
                    TypeReglement = x.BonLivraison.Facture.TypePaiement?.Name,
                    Comment = x.BonLivraison.Facture.Comment,
                    NumBL = x.BonLivraison.NumBon,
                    NumBC = x.NumBC,
                    ICE = x.BonLivraison.Facture.Client.ICE,
                    TotalHT = x.TotalHT,
                    CodeClient = x.BonLivraison.Client.Code,
                    Discount = x.Discount + (x.PercentageDiscount ? "%" : ""),
                    Total = (x.Qte * x.Pu) - (x.PercentageDiscount ? (x.Qte * x.Pu * (x.Discount ?? 0.0f) / 100) : x.Discount ?? 0.0f),
                    Index = x.Index ?? 0
                }).ToList();
                reportDocument.SetDataSource(facturePrintData);

                float total = facturePrintData.Sum(x => x.Total * (1 + x.TVA / 100));

                if (FactureById.IdTypePaiement == ESPECE_PAYMENT_TYPE && company.Name != "SMTB")
                {
                    total *= (1 + 0.0025f);
                }
                totalMots = atg.DecimalToWords(Math.Round((Decimal)total, 2, MidpointRounding.AwayFromZero));

                // TVA
                var baseTVA0 = facturePrintData.Where(x => x.TVA == 0f).Sum(x => x.Total);
                var baseTVA7 = facturePrintData.Where(x => x.TVA == 7f).Sum(x => x.Total);
                var baseTVA10 = facturePrintData.Where(x => x.TVA == 10f).Sum(x => x.Total);
                var baseTVA14 = facturePrintData.Where(x => x.TVA == 14f).Sum(x => x.Total);
                var baseTVA20 = facturePrintData.Where(x => x.TVA == 20f).Sum(x => x.Total);

                if (reportDocument.ParameterFields["BaseTVA0"] != null)
                    reportDocument.SetParameterValue("BaseTVA0", baseTVA0);
                if (reportDocument.ParameterFields["BaseTVA7"] != null)
                    reportDocument.SetParameterValue("BaseTVA7", baseTVA7);
                if (reportDocument.ParameterFields["BaseTVA10"] != null)
                    reportDocument.SetParameterValue("BaseTVA10", baseTVA10);
                if (reportDocument.ParameterFields["BaseTVA14"] != null)
                    reportDocument.SetParameterValue("BaseTVA14", baseTVA14);
                if (reportDocument.ParameterFields["BaseTVA20"] != null)
                    reportDocument.SetParameterValue("BaseTVA20", baseTVA20);

            }


            if (reportDocument.ParameterFields["totalMots"] != null)
                reportDocument.SetParameterValue("totalMots", totalMots);
            if (reportDocument.ParameterFields["Cachet"] != null)
                reportDocument.SetParameterValue("Cachet", cachet);
            if (reportDocument.ParameterFields["ShowDiscount"] != null)
                reportDocument.SetParameterValue("ShowDiscount", FactureById.WithDiscount);
            if (reportDocument.ParameterFields["IsEspece"] != null)
                reportDocument.SetParameterValue("IsEspece", FactureById.IdTypePaiement.HasValue ? FactureById.IdTypePaiement == ESPECE_PAYMENT_TYPE : false);

            reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
            reportDocument.PrintOptions.ApplyPageMargins(new PageMargins(0, 0, 0, 0));

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