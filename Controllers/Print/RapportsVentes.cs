﻿using CrystalDecisions.CrystalReports.Engine;
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
    public class RapportsVentesController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult BL(DateTime dateFrom, DateTime dateTo)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string company = StatistiqueController.getCompanyName().ToUpper();


            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/RapportVentes.rpt")));
            var dataSource = context.BonLivraisons.Where(
                        (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo))
                .Select(x => new
                {
                    Client = x.Client.Name,
                    ICE = x.Client.ICE,
                    NumBon = x.NumBon,
                    Ref = x.Ref,
                    Date = x.Date,
                    Debit = x.BonLivraisonItems.Sum(y => (float?)y.Pu * y.Qte) ?? 0,
                    Type = x.TypePaiement != null ? x.TypePaiement.Name : "",
                    Credit = 0,
                }).OrderBy(x => new { x.Ref, x.Date.Year }).ToList();

            reportDocument.SetDataSource(dataSource);
            if (reportDocument.ParameterFields["document"] != null)
                reportDocument.SetParameterValue("document", "BL");
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

        public ActionResult FA(DateTime dateFrom, DateTime dateTo)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            var company = context.Companies.FirstOrDefault();

            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/RapportVentesWithoutGroupBy.rpt")));

            var dataSource = company.UseVAT ? (context.Factures.Where(
                        (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo)).OrderBy(x => x.Date)
                .AsEnumerable().Select(x => new
                {
                    Client = x.Client.Name,
                    ICE = x.ClientICE,
                    NumBon = x.NumBon,
                    Ref = x.Ref,
                    Date = x.Date,
                    Debit = x.BonLivraisons.SelectMany(y => y.BonLivraisonItems).Sum(y => (float?)y.Pu * y.Qte) ?? 0,
                    Credit = 0,
                    Type = x.TypePaiement != null ? x.TypePaiement.Name : "",
                    DateEcheance = x.DateEcheance.HasValue ? x.DateEcheance.Value.ToString("dd/MM/yyyy") : ""
                }).ToList()) : context.FakeFactures.Where(
                        (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo)).OrderBy(x => new { x.Ref, x.Date.Year })
                .AsEnumerable().Select(x => new
                {
                    Client = x.ClientName,
                    ICE = x.ClientICE,
                    NumBon = x.NumBon,
                    Ref = x.Ref,
                    Date = x.Date,
                    Debit = x.FakeFactureItems.Sum(y => (float?)y.Pu * y.Qte) ?? 0,
                    Credit = 0,
                    Type = x.TypePaiement != null ? x.TypePaiement.Name : "",
                    DateEcheance = x.DateEcheance.HasValue ? x.DateEcheance.Value.ToString("dd/MM/yyyy") : ""
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

        public ActionResult Transactions(DateTime dateFrom, DateTime dateTo)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            var company = context.Companies.FirstOrDefault();
            var ESPECE_PAYMENT_TYPE = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");

            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/RapportTransactions.rpt")));
            var dataSource = new List<dynamic>();
            var BonLivraisons = context.BonLivraisons.Where(
                        (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo))
                .Select(x => new
                {
                    Client = x.Client.Name,
                    NumBon = "BL " + x.NumBon,
                    Date = x.Date,
                    Debit = x.BonLivraisonItems.Sum(y => (float?)y.Pu * y.Qte) ?? 0,
                    Type = x.TypePaiement != null ? x.TypePaiement.Name : "",
                    IsRemboursement = false,
                    IsBankRelated = x.TypePaiement != null ? x.TypePaiement.IsBankRelated : false,
                    IsEspece = false,
                    Credit = 0,
                    Commentaire = "",
                }).ToList();
            var Factures = company.UseVAT ? (context.Factures.Where(
                        (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo))
                .Select(x => new
                {
                    Client = x.Client.Name,
                    NumBon = "FA " + x.NumBon,
                    Date = x.Date,
                    Debit = x.BonLivraisons.SelectMany(y => y.BonLivraisonItems).Sum(y => (float?)y.Pu * y.Qte) ?? 0,
                    Credit = 0,
                    Type = x.TypePaiement != null ? x.TypePaiement.Name : "",
                    IsRemboursement = false,
                    IsBankRelated = x.TypePaiement != null ? x.TypePaiement.IsBankRelated : false,
                    IsEspece = false,
                    Commentaire = "",
                }).OrderBy(x => x.Date).ToList()) : context.FakeFactures.Where(
                        (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo))
                .Select(x => new
                {
                    Client = x.Client.Name,
                    NumBon = "FA " + x.NumBon,
                    Date = x.Date,
                    Debit = x.FakeFactureItems.Sum(y => (float?)y.Pu * y.Qte) ?? 0,
                    Credit = 0,
                    Type = x.TypePaiement != null ? x.TypePaiement.Name : "",
                    IsRemboursement = false,
                    IsBankRelated = x.TypePaiement != null ? x.TypePaiement.IsBankRelated : false,
                    IsEspece = false,
                    Commentaire = "",
                }).OrderBy(x => x.Date).ToList();

            var BonAvoirCs = context.BonAvoirCs.Where(
                        (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo))
                .Select(x => new
                {
                    Client = x.Client.Name,
                    NumBon = "BA " + x.NumBon,
                    Date = x.Date,
                    Debit = x.BonAvoirCItems.Sum(y => (float?)y.Pu * y.Qte) ?? 0,
                    Type = "",
                    IsBankRelated = false,
                    IsEspece = false,
                    IsRemboursement = false,
                    Credit = 0,
                    Commentaire = "",
                }).ToList();

            var PaiementClients = !company.UseVAT ? context.Paiements.Where(
                        (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo && (x.IdBonAvoirC == null && x.IdBonLivraison == null)))
                .Select(x => new
                {
                    Client = x.Client.Name,
                    NumBon = "-",
                    Date = x.Date,
                    Debit = x.Credit > 0 ? x.Credit : x.Debit,
                    Type = x.TypePaiement.Name,
                    IsBankRelated = x.TypePaiement != null ? x.TypePaiement.IsBankRelated : false,
                    IsEspece = x.TypePaiement.Id == ESPECE_PAYMENT_TYPE,
                    IsRemboursement = x.TypePaiement != null ? x.TypePaiement.IsRemboursement : false,
                    Credit = 0,
                    Commentaire = x.Comment,
                }).ToList() : context.PaiementFactures.Where(
                       (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo && (x.IdBonAvoirC == null && x.IdFacture == null)))
               .Select(x => new
               {
                   Client = x.Client.Name,
                   NumBon = "-",
                   Date = x.Date,
                   Debit = x.Credit > 0 ? x.Credit : x.Debit,
                   Type = x.TypePaiement.Name,
                   IsBankRelated = x.TypePaiement != null ? x.TypePaiement.IsBankRelated : false,
                   IsEspece = x.TypePaiement.Id == ESPECE_PAYMENT_TYPE,
                   IsRemboursement = x.TypePaiement != null ? x.TypePaiement.IsRemboursement : false,
                   Credit = 0,
                   Commentaire = x.Comment,
               }).ToList();

            var Depenses = context.Depenses.Where(
                       (x => DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo))
                .AsEnumerable()
               .Select(x => new
               {
                   Client = "-",
                   NumBon = "",
                   Date = x.Date,
                   Debit = x.DepenseItems.Sum(y => (float?)y.Montant) ?? 0,
                   Type = "Dépense",
                   IsBankRelated = false,
                   IsRemboursement = false,
                   IsEspece = false,
                   Credit = 0,
                   Commentaire = String.Join(", ", x.DepenseItems.Select(y => y.Name)),
               }).ToList();

            dataSource.AddRange(BonLivraisons);
            dataSource.AddRange(BonAvoirCs);
            dataSource.AddRange(PaiementClients);
            dataSource.AddRange(Factures);
            dataSource.AddRange(Depenses);

            reportDocument.SetDataSource(dataSource.OrderBy(x => x.Date));

            if (reportDocument.ParameterFields["totalDepenses"] != null)
                reportDocument.SetParameterValue("totalDepenses", Depenses.Sum(x => x.Debit));
            if (reportDocument.ParameterFields["totalVentes"] != null)
                reportDocument.SetParameterValue("totalVentes", BonLivraisons.Sum(x => x.Debit));
            if (reportDocument.ParameterFields["totalEspece"] != null)
                reportDocument.SetParameterValue("totalEspece", PaiementClients.Where(x => x.IsEspece).Sum(x => x.Debit));
            if (reportDocument.ParameterFields["totalCaisse"] != null)
                reportDocument.SetParameterValue("totalCaisse", PaiementClients.Where(x => x.IsEspece).Sum(x => x.Debit) - Depenses.Sum(x => x.Debit) - PaiementClients.Where(x => x.IsRemboursement).Sum(x => x.Debit));

            reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
            reportDocument.PrintOptions.ApplyPageMargins(new PageMargins(0, 0, 0, 0));

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Transactions.pdf",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult BLByClient(Guid id, DateTime dateFrom, DateTime dateTo)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();

            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/RapportVentesDetails.rpt")));
            var dataSource = context.BonLivraisons.Where(
                        (x => x.IdClient == id && DbFunctions.TruncateTime(x.Date) >= dateFrom && DbFunctions.TruncateTime(x.Date) <= dateTo))
                .SelectMany(x=>x.BonLivraisonItems)
                .Select(x => new
                {
                    Client = x.BonLivraison.Client.Name,
                    NumBon = x.BonLivraison.NumBon,
                    Date = x.BonLivraison.Date,
                    PU = x.Pu,
                    Qte = x.Qte,
                    Article = x.Article.Designation,
                    Comment = x.BonLivraison.Note
                }).OrderBy(x => x.Date).ToList();

            reportDocument.SetDataSource(dataSource);

            reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
            reportDocument.PrintOptions.ApplyPageMargins(new PageMargins(0, 0, 0, 0));

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "RapportVentesDetails.pdf",
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