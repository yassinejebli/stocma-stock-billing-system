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
    public class BRController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index(Guid IdBonReception)
        {
            var BonReceptionById = context.BonReceptions.Where(x => x.Id == IdBonReception).FirstOrDefault();
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string company = StatistiqueController.getCompanyName().ToUpper();

           
            reportDocument.Load(
                Path.Combine(
                    this.Server.MapPath("~/CrystalReports/" + company + "/MiniBonReception" + company +
                                        ".rpt")));

            reportDocument.SetDataSource(
                BonReceptionById.BonReceptionItems.Select(x => new
                {
                    NumBon = x.BonReception.NumBon,
                    Date = x.BonReception.Date,
                    Client = x.BonReception.Fournisseur.Name,
                    Ref = x.Article.Ref,
                    Designation = x.Article.Designation,
                    Qte = x.Qte,
                    PU = x.Pu,
                    TotalHT = x.TotalHT,
                    Unite = x.Article.Unite,
                    Index = x.Index ?? 0
                }).OrderByDescending(x => x.Index).ToList()
             );
            if (reportDocument.ParameterFields["Solde"] != null)
                reportDocument.SetParameterValue("Solde", BonReceptionById.Fournisseur.PaiementFs.Sum(x=>x.Debit - x.Credit));
          

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "BR.pdf",
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