using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DATA;

namespace WebApplication1.Controllers.Print
{
    public class PaiementsController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index(Guid id, DateTime dateFrom, DateTime dateTo)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string company = StatistiqueController.getCompanyName().ToUpper();
            var client = context.Clients.Find(id);


            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/" + company + "/Paiements.rpt")));
            var dataSource = context.Paiements.Where(
                        (x => x.IdClient == id && DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo && x.Hide != true))
                .Select(x => new
                {
                    Client = x.Client.Name,
                    NumBon = x.BonLivraison.NumBon,
                    Date = x.Date,
                    Debit = x.Debit,
                    Credit = x.Credit,
                    Type = x.TypePaiement.Name,
                    DateEcheance = x.DateEcheance.ToString(),
                    Commentaire = x.Comment,
                    ICE = x.Client.ICE,
                    Adresse = x.Client.Adresse
                }).OrderBy(x => x.Date).ToList();
            var soldeBeforeDate = context.Paiements.Where(x => DbFunctions.TruncateTime(x.Date) < dateFrom.Date && x.IdClient == id).Sum(x => (float?)(x.Debit - x.Credit)) ?? 0f;
            var solde = context.Paiements.Where(x => x.IdClient == id).Sum(x => (float?)(x.Debit - x.Credit)) ?? 0f;
            dataSource.Insert(0, new
            {
                Client = client.Name,
                NumBon = "",
                Date = dateFrom,
                Debit = soldeBeforeDate,
                Credit = 0f,
                Type = "-",
                DateEcheance = "",
                Commentaire = "Solde avant : " + dateFrom.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                ICE = client.ICE,
                Adresse = client.Adresse
            });

            reportDocument.SetDataSource(dataSource);

            if (reportDocument.ParameterFields["solde"] != null)
                reportDocument.SetParameterValue("solde", solde);
            if (reportDocument.ParameterFields["header"] != null)
                reportDocument.SetParameterValue("header", solde);
            //if (reportDocument.ParameterFields["soldeBeforeDate"] != null)
            //reportDocument.SetParameterValue("soldeBeforeDate", soldeBeforeDate);

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Paiements.pdf",
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