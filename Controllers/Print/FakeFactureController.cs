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
    public class FakeFactureController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index(Guid IdFakeFacture)
        {
            var ESPECE_PAYMENT_TYPE = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");
            AmountTextGenerator atg = new AmountTextGenerator();
            string totalMots = "";
            var company = StatistiqueController.getCompany();
            var useVAT = company.UseVAT;
            var FactureById = context.FakeFactures.Where(x => x.Id == IdFakeFacture).FirstOrDefault();
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();

            reportDocument.Load(
                Path.Combine(
                    this.Server.MapPath("~/CrystalReports/" + company.Name + "/Facture" + company.Name +
                                        ".rpt")));

            var facturePrintData = FactureById.FakeFactureItems.Select(x => new
            {
                NumBon = x.FakeFacture.NumBon,
                Date = x.FakeFacture.Date,
                Client = x.FakeFacture.Client.Name,
                Ref = x.ArticleFacture.Ref,
                Designation = x.ArticleFacture.Designation,
                Qte = x.Qte,
                PU = x.Pu,
                TVA = x.ArticleFacture.TVA ?? 20.0f,
                Unite = x.ArticleFacture.Unite,
                Adresse = x.FakeFacture.Client.Adresse,
                ClientName = x.FakeFacture.ClientName,
                TypeReglement = x.FakeFacture.TypePaiement?.Name,
                Comment = x.FakeFacture.Comment,
                NumBL = "",
                NumBC = "",
                ICE = x.FakeFacture.Client.ICE,
                TotalHT = x.TotalHT,
                CodeClient = x.FakeFacture.Client.Code,
                Discount = x.Discount + (x.PercentageDiscount ? "%" : ""),
                Total = (x.Qte * x.Pu),
            }).ToList();
            reportDocument.SetDataSource(facturePrintData);

            float total = facturePrintData.Sum(x => x.Total);

            //TODO: need to review this
            if (FactureById.IdTypePaiement == ESPECE_PAYMENT_TYPE && (company.Name == "AQK" || company.Name == "TSR"))
            {
                total *= (1 + 0.0025f);
            }

            totalMots = atg.DecimalToWords(Math.Round((Decimal)total, 2, MidpointRounding.AwayFromZero));

            // TVA
            var baseTVA0 = facturePrintData.Where(x => x.TVA == 0f).Sum(x => x.Total);
            var baseTVA7 = facturePrintData.Where(x => x.TVA == 7f).Sum(x => x.Total / 0.7);
            var baseTVA10 = facturePrintData.Where(x => x.TVA == 10f).Sum(x => x.Total / 1.1);
            var baseTVA14 = facturePrintData.Where(x => x.TVA == 14f).Sum(x => x.Total / 1.14);
            var baseTVA20 = facturePrintData.Where(x => x.TVA == 20f).Sum(x => x.Total / 1.2);

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

            if (reportDocument.ParameterFields["totalMots"] != null)
                reportDocument.SetParameterValue("totalMots", totalMots);
            if (reportDocument.ParameterFields["ShowDiscount"] != null)
                reportDocument.SetParameterValue("ShowDiscount", FactureById.WithDiscount);
            if (reportDocument.ParameterFields["IsEspece"] != null)
                reportDocument.SetParameterValue("IsEspece", FactureById.IdTypePaiement.HasValue ? FactureById.IdTypePaiement == ESPECE_PAYMENT_TYPE : false);


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