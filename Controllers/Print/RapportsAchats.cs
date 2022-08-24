using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Auth;
using WebApplication1.DATA;

namespace WebApplication1.Controllers.Print
{
    [Authorize(Roles = "Admin")]
    public class RapportsAchatsController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult FA(DateTime dateFrom, DateTime dateTo)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            var company = context.Companies.FirstOrDefault();

            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/RapportAchatsWithoutGroupBy.rpt")));

            var dataSource = company.UseVAT ? (context.FactureFs.Where(
                        (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo)).OrderBy(x => x.Date)
                .AsEnumerable().Select(x => new
                {
                    Client = x.Fournisseur.Name,
                    ICE = x.Fournisseur.ICE,
                    NumBon = x.NumBon,
                    Ref = x.Ref ?? 0,
                    Date = x.Date,
                    Debit = x.BonReceptions.SelectMany(y => y.BonReceptionItems).Sum(y => (float?)y.Pu * y.Qte) ?? 0,
                    Credit = 0,
                    Type = x.TypePaiement != null ? x.TypePaiement.Name : "",
                    DateEcheance = ""
                }).ToList()) : context.FakeFacturesF.Where(
                        (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo)).OrderBy(x => x.Date)
                .AsEnumerable().Select(x => new
                {
                    Client = x.Fournisseur.Name,
                    ICE = x.Fournisseur.ICE,
                    NumBon = x.NumBon,
                    Ref = x.Ref ?? 0,
                    Date = x.Date,
                    Debit = x.FakeFactureFItems.Sum(y => (float?)y.Pu * y.Qte) ?? 0,
                    Credit = 0,
                    Type = x.TypePaiement != null ? x.TypePaiement.Name : "",
                    DateEcheance =  ""
                }).ToList();

            reportDocument.SetDataSource(dataSource);

            if (reportDocument.ParameterFields["document"] != null)
                reportDocument.SetParameterValue("document", "FA");
            if (reportDocument.ParameterFields["total"] != null)
                reportDocument.SetParameterValue("total", dataSource.Sum(x => x.Debit));

            reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
            reportDocument.PrintOptions.ApplyPageMargins(new PageMargins(0, 0, 0, 0));

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "RapportVentes.pdf",
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