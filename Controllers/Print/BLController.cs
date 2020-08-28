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
    public class BLController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();
        
        public ActionResult Index(Guid IdBonLivraison, bool? showBalance = false, bool? showPrices = true, bool? bigFormat = false, bool? showStamp = false)
        {
            string company = StatistiqueController.getCompanyName().ToUpper();
            if (company == "SUIV" || company == "SBCIT")
                bigFormat = true;

            var ESPECE_PAYMENT_TYPE = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");
            var BonLivraisonById = context.BonLivraisons.Where(x => x.Id == IdBonLivraison).FirstOrDefault();
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();

            if (bigFormat == true)
                reportDocument.Load(
                    Path.Combine(
                        this.Server.MapPath("~/CrystalReports/" + company + "/BonLivraison.rpt")));
            else
                reportDocument.Load(
                    Path.Combine(
                        this.Server.MapPath("~/CrystalReports/" + company + "/MiniBonLivraison.rpt")));

            reportDocument.SetDataSource(
                BonLivraisonById.BonLivraisonItems.Select(x => new
                {
                    NumBon = x.BonLivraison.NumBon,
                    Date = x.BonLivraison.Date,
                    Client = x.BonLivraison.Client.Name,
                    Ref = x.Article.Ref,
                    Designation = x.Article.Designation,
                    Qte = x.Qte,
                    PU = x.Pu,
                    TotalHT = x.TotalHT,
                    TypeReglement = x.BonLivraison.TypePaiement != null ? x.BonLivraison.TypePaiement.Name : "",
                    TVA = x.Article.TVA ?? 20,
                    CodeClient = x.BonLivraison.Client.Code,
                    NumBL = x.BonLivraison.NumBon,
                    Adresse = x.BonLivraison.Client.Adresse,
                    ICE = x.BonLivraison.Client.ICE,
                    User = x.BonLivraison.User,
                    NumBC = x.NumBC,
                    Unite = x.Article.Unite,
                    Discount = x.Discount + (x.PercentageDiscount ? "%" : ""),
                    Total = (x.Qte * x.Pu) - (x.PercentageDiscount ? (x.Qte * x.Pu * (x.Discount ?? 0.0f) / 100) : x.Discount ?? 0.0f),
                    Index = x.Index ?? 0
                }).OrderBy(x => x.Index).ToList()
             );
            if (reportDocument.ParameterFields["ShowSolde"] != null)
                reportDocument.SetParameterValue("ShowSolde", showBalance);
            if (reportDocument.ParameterFields["Solde"] != null)
                reportDocument.SetParameterValue("Solde", BonLivraisonById.Client.Paiements.Sum(x => x.Debit - x.Credit));
            if (reportDocument.ParameterFields["ShowPrices"] != null)
                reportDocument.SetParameterValue("ShowPrices", showPrices);
            if (reportDocument.ParameterFields["ShowDiscount"] != null)
                reportDocument.SetParameterValue("ShowDiscount", BonLivraisonById.WithDiscount);
            if (reportDocument.ParameterFields["Cachet"] != null)
                reportDocument.SetParameterValue("Cachet", showStamp);
            if (reportDocument.ParameterFields["IsEspece"] != null)
                reportDocument.SetParameterValue("IsEspece", BonLivraisonById.IdTypePaiement.HasValue ? BonLivraisonById.IdTypePaiement == ESPECE_PAYMENT_TYPE : false);

            reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
            reportDocument.PrintOptions.ApplyPageMargins(new PageMargins(0, 0, 0, 0));

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "BL.pdf",
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