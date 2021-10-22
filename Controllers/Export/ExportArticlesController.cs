using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using WebApplication1.DATA;


/*
 * 
 * String.Join(", ",context.Fournisseurs.Where(y=>y.BonReceptions
                .Where(z=>z.BonReceptionItems
                .Where(c=>c.IdArticle == x.Article.Id).Count() > 0).Count() > 0).Select(y=>y.Name).ToList())
 */
namespace WebApplication1.Controllers.Export
{
    public class ExportArticlesController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public void Index(int idSite)
        {
            var articles = context.ArticleSites.Where(x => x.IdSite == idSite && !x.Disabled).OrderByDescending(x=>x.Article.Categorie.Name).ToList().Select(x => new
            {
                Ref = x.Article.Ref,
                Designation = x.Article.Designation,
                Qte = x.QteStock,
                PA = x.Article.PA,
                PU = x.Article.PVD,
                Famille = x.Article.Categorie != null ? x.Article.Categorie.Name : "",
                Fournisseurs = String.Join(", ",context.Fournisseurs.Where(y=>y.BonReceptions
                .Where(z=>z.BonReceptionItems
                .Where(c=>c.IdArticle == x.Article.Id).Count() > 0).Count() > 0).Select(y=>y.Name).ToList())
            });
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
            Sheet.Row(2).Height = 50;
            Sheet.Row(3).Height = 50;
            Sheet.Row(4).Height = 20;

            Sheet.Cells["A4:G4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheet.Cells["A4:G4"].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            Sheet.Row(4).Style.Font.Bold = true;

            //Setting meta data

            Sheet.Cells["A2"].Value = "Stock";
            Sheet.Cells["A2"].Style.WrapText = true;
            Sheet.Cells["A2"].Style.Font.Bold = true;
            Sheet.Cells["A2"].Style.Font.Size = 17;

            Sheet.Cells["A3"].Value = "Marrakech le : " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            Sheet.Cells["A3"].Style.Font.Bold = true;

            Sheet.Cells["A4"].Value = "Référence";
            Sheet.Cells["B4"].Value = "Désignation";
            Sheet.Cells["C4"].Value = "Qte";
            Sheet.Cells["D4"].Value = "Prix d'achat";
            Sheet.Cells["E4"].Value = "Prix de vente";
            Sheet.Cells["F4"].Value = "Famille";
            Sheet.Cells["G4"].Value = "Fournisseurs";
            int row = 5;
            foreach (var item in articles)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.Ref;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Designation;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Qte;
                Sheet.Cells[string.Format("C{0}", row)].Style.Numberformat.Format = "0.00";
                Sheet.Cells[string.Format("D{0}", row)].Value = item.PA;
                Sheet.Cells[string.Format("D{0}", row)].Style.Numberformat.Format = "0.00";
                Sheet.Cells[string.Format("E{0}", row)].Value = item.PU;
                Sheet.Cells[string.Format("E{0}", row)].Style.Numberformat.Format = "0.00";
                Sheet.Cells[string.Format("F{0}", row)].Value = item.Famille;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.Fournisseurs;
                row++;
            }



            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" +
                "Stock " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) +".xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }

    }
}