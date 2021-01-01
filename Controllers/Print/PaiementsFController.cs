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
    public class PaiementsFController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index(Guid id, DateTime dateFrom, DateTime dateTo, bool showStamp = false)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string company = StatistiqueController.getCompanyName().ToUpper();
            var useVAT = context.Companies.FirstOrDefault().UseVAT;

            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/" + company + "/PaiementsF.rpt")));

            var solde = useVAT ? context.PaiementFactureFs.Where(x => x.IdFournisseur == id).Sum(x => (float?)(x.Debit - x.Credit)) ?? 0f : context.PaiementFs.Where(x => x.IdFournisseur == id).Sum(x => (float?)(x.Debit - x.Credit)) ?? 0f;


            reportDocument.SetDataSource(GetPaiements(id, dateFrom, dateTo));

            if (reportDocument.ParameterFields["solde"] != null)
                reportDocument.SetParameterValue("solde", solde);
            if (reportDocument.ParameterFields["header"] != null)
                reportDocument.SetParameterValue("header", solde);
            if (reportDocument.ParameterFields["cachet"] != null)
                reportDocument.SetParameterValue("cachet", showStamp);

            reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
            reportDocument.PrintOptions.ApplyPageMargins(new PageMargins(0, 0, 0, 0));

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "PaiementsF.pdf",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }


        // TODO: Update aliases (Client...)
        private dynamic GetPaiements(Guid id, DateTime dateFrom, DateTime dateTo)
        {
            var useVAT = context.Companies.FirstOrDefault().UseVAT;
            var fournisseur = context.Fournisseurs.Find(id);

            var dataSource = context.Paiements.Where(x => x.IdClient == id && DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo && x.Hide != true)
                .Select(x => new
                {
                    Client = x.Client.Name,
                    NumBon = x.BonLivraison.NumBon,
                    Date = x.Date,
                    Debit = x.Debit,
                    Credit = x.Credit,
                    Type = x.TypePaiement.Name,
                    DateEcheance = "",
                    Commentaire = x.Comment,
                    ICE = x.Client.ICE,
                    Adresse = x.Client.Adresse
                }).OrderBy(x => x.Date).ToList();
            if (useVAT)
            {
                dataSource = context.PaiementFactureFs.Where(x => x.IdFournisseur == id && DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo && x.Hide != true)
                .Select(x => new
                {
                    Client = x.Fournisseur.Name,
                    NumBon = x.FactureF.NumBon,
                    Date = x.Date,
                    Debit = x.Debit,
                    Credit = x.Credit,
                    Type = x.TypePaiement.Name,
                    DateEcheance = "",
                    Commentaire = x.Comment,
                    ICE = x.Fournisseur.ICE,
                    Adresse = x.Fournisseur.Adresse
                }).OrderBy(x => x.Date).ToList();
            }
            var soldeBeforeDate = useVAT ? context.PaiementFactureFs.Where(x => DbFunctions.TruncateTime(x.Date) < dateFrom.Date && x.IdFournisseur == id).Sum(x => (float?)(x.Debit - x.Credit)) ?? 0f : context.PaiementFs.Where(x => DbFunctions.TruncateTime(x.Date) < dateFrom.Date && x.IdFournisseur == id).Sum(x => (float?)(x.Debit - x.Credit)) ?? 0f;
            dataSource.Insert(0, new
            {
                Client = fournisseur.Name,
                NumBon = "",
                Date = dateFrom,
                Debit = soldeBeforeDate,
                Credit = 0f,
                Type = "-",
                DateEcheance = "",
                Commentaire = "Solde avant : " + dateFrom.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                ICE = fournisseur.ICE,
                Adresse = fournisseur.Adresse
            });

            return dataSource;
        }


    }
}