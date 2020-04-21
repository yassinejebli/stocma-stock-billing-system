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

        public ActionResult Index(Guid IdBonLivraison, bool? showBalance = false, bool? showPrices = true, bool? bigFormat = false)
        {
            var BonLivraisonById = context.BonLivraisons.Where(x => x.Id == IdBonLivraison).FirstOrDefault();
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string company = StatistiqueController.getCompanyName().ToUpper();

            if(bigFormat == true)
                reportDocument.Load(
                    Path.Combine(
                        this.Server.MapPath("~/CrystalReports/" + company + "/BonLivraison" + company +
                                            ".rpt")));
            else
                reportDocument.Load(
                    Path.Combine(
                        this.Server.MapPath("~/CrystalReports/" + company + "/MiniBonLivraisonSoldeFrancais" + company +
                                            ".rpt")));

          /*  NumberFormatInfo numberFormatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            numberFormatInfo.NumberGroupSeparator = " ";
            */
            //TODO: change this
            /*(reportDocument.ReportDefinition.ReportObjects["solde"] as TextObject).Text =
                    statistiqueController.SoldeByClient(
                        this.context.BonLivraisons.Where(
                               (x => x.Id == IdBonLivraison)).Select((x => x.IdClient))
                            .FirstOrDefault()).ToString("#,#.00", numberFormatInfo) + "DH";
            TextObject reportObject = reportDocument.ReportDefinition.ReportObjects["oldSolde"] as TextObject;

            if (
                    this.context.BonLivraisons.Where(
                         (x => x.Id == IdBonLivraison))
                        .FirstOrDefault() != null)
            {
                float? oldSolde =
                    this.context.BonLivraisons.Where(
                            (x => x.Id == IdBonLivraison))
                        .FirstOrDefault()
                        .OldSolde;
                float num1 = 0.0f;
                if (((double)oldSolde.GetValueOrDefault() > (double)num1 ? (oldSolde.HasValue ? 1 : 0) : 0) != 0)
                {
                    oldSolde =
                        this.context.BonLivraisons.Where(
                                (x => x.Id == IdBonLivraison))
                            .FirstOrDefault()
                            .OldSolde;
                    float num2 = oldSolde.HasValue ? oldSolde.GetValueOrDefault() : 0.0f;
                    reportObject.Text = num2.ToString() ?? "NEANT";
                }
            }
            */
            


            /*else
                reportDocument.Load(
                Path.Combine(
                    this.Server.MapPath("~/CrystalReports/" + company + "/MiniBonLivraisonFrancais" + company +
                                        ".rpt")));*/







            /*if (company == "H9S" && IsArabic == true)
                reportDocument.Load(
                    Path.Combine(
                        this.Server.MapPath("~/CrystalReports/" + company + "/MiniBonLivraisonArabic" + company + ".rpt")));
                        */

            ////////////////////



            ////////////////////////
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
                    CodeClient = x.BonLivraison.Client.Code,
                    Unite = x.Article.Unite,
                    Index = x.Index ?? 0
                }).OrderByDescending(x => x.Index).ToList()
             );
            if(reportDocument.ParameterFields["ShowSolde"] != null)
                reportDocument.SetParameterValue("ShowSolde", showBalance);
            if (reportDocument.ParameterFields["Solde"] != null)
                reportDocument.SetParameterValue("Solde", BonLivraisonById.Client.Paiements.Sum(x=>x.Debit - x.Credit));
            if (reportDocument.ParameterFields["ShowPrices"] != null)
                reportDocument.SetParameterValue("ShowPrices", showPrices);
            /*if (reportDocument.ParameterFields["OldSolde"] != null)
                reportDocument.SetParameterValue("OldSolde", 0);*/

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