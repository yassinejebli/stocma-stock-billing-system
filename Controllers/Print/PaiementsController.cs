using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data.Entity;
using System.Drawing;
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

            var solde = context.Paiements.Where(x => x.IdClient == id).Sum(x => (float?)(x.Debit - x.Credit)) ?? 0f;


            reportDocument.SetDataSource(GetPaiements(id, dateFrom, dateTo));

            if (reportDocument.ParameterFields["solde"] != null)
                reportDocument.SetParameterValue("solde", solde);
            if (reportDocument.ParameterFields["header"] != null)
                reportDocument.SetParameterValue("header", solde);

            reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
            reportDocument.PrintOptions.ApplyPageMargins(new PageMargins(0, 0, 0, 0));

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


        private dynamic GetPaiements(Guid id, DateTime dateFrom, DateTime dateTo)
        {
            var client = context.Clients.Find(id);

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

            return dataSource;
        }


        public void Export(Guid id, DateTime dateFrom, DateTime dateTo)
        {
            var client = context.Clients.Find(id);
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Situation");

            //Settings
            Sheet.PrinterSettings.PaperSize = ePaperSize.A4;
            Sheet.PrinterSettings.Orientation = eOrientation.Portrait;
            Sheet.PrinterSettings.HorizontalCentered = true;
            Sheet.PrinterSettings.FitToPage = true;
            Sheet.PrinterSettings.FitToWidth = 1;
            Sheet.PrinterSettings.FitToHeight = 0;

            //Styling
            Sheet.Cells["A2:F2"].Merge = true;
            Sheet.Cells["A3:F3"].Merge = true;
            Sheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            Sheet.Row(2).Height = 50;
            Sheet.Row(3).Height = 50;
            Sheet.Row(4).Height = 20;

            Sheet.Cells["A4:F4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheet.Cells["A4:F4"].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            Sheet.Row(4).Style.Font.Bold = true;

            //Setting meta data

            Sheet.Cells["A2"].Value = "Situation de compte client : " + client.Name;
            Sheet.Cells["A2"].Style.WrapText = true;
            Sheet.Cells["A2"].Style.Font.Bold = true;
            Sheet.Cells["A2"].Style.Font.Size = 17;

            Sheet.Cells["A3"].Value = "Marrakech le : " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            Sheet.Cells["A3"].Style.Font.Bold = true;

            Sheet.Cells["A4"].Value = "Date";
            Sheet.Cells["B4"].Value = "BL#";
            Sheet.Cells["C4"].Value = "Débit";
            Sheet.Cells["D4"].Value = "Crédit";
            Sheet.Cells["E4"].Value = "Type";
            Sheet.Cells["F4"].Value = "Note";
            //Sheet.Cells["G4"].Value = "Échéance";
            int row = 5;
            foreach (var item in GetPaiements(id, dateFrom, dateTo))
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                Sheet.Cells[string.Format("B{0}", row)].Value = item.NumBon;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Debit != 0 ? item.Debit : "";
                Sheet.Cells[string.Format("C{0}", row)].Style.Numberformat.Format = "0.00";
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Credit != 0 ? item.Credit : "";
                Sheet.Cells[string.Format("D{0}", row)].Style.Numberformat.Format = "0.00";
                Sheet.Cells[string.Format("E{0}", row)].Value = item.Type;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.Commentaire;
                //Sheet.Cells[string.Format("G{0}", row)].Value = item.DateEcheance;
                row++;
            }

            //Formulas
            Sheet.Cells["A" + row].Value = "TOTAL";
            Sheet.Cells["A" + row].Style.Font.Bold = true;

            var totalDebitCell = "C"+row;
            var totalCreditCell = "D" + row;
            Sheet.Cells[totalDebitCell].Formula = "SUM(C5:C"+(row - 1)+")";
            Sheet.Cells[totalCreditCell].Formula = "SUM(D5:D" + (row - 1)+")";

            Sheet.Cells[totalDebitCell].Style.Font.Bold = true;
            Sheet.Cells[totalCreditCell].Style.Font.Bold = true;
            Sheet.Cells[totalDebitCell].Style.Numberformat.Format = "0.00";
            Sheet.Cells[totalCreditCell].Style.Numberformat.Format = "0.00";

            row++;

            Sheet.Cells["A" + row].Value = "SOLDE";
            Sheet.Cells["A" + row].Style.Font.Bold = true;

            Sheet.Cells["D" + row].Formula = totalDebitCell+"-"+totalCreditCell;
            Sheet.Cells["D" + row].Style.Font.Bold = true;
            Sheet.Cells["D" + row].Style.Numberformat.Format = "0.00";



            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" +
                client.Name + " " + dateFrom.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) +
                " - " + dateTo.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + ".xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }
    }
}