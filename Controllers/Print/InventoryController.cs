using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using WebApplication1.DATA;
using WebGrease.Css.Extensions;

namespace WebApplication1.Controllers.Print
{
    public class PrintInventoryController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index([FromUri] int idSite, [FromUri] Guid[] ids, string titre, bool showBarCode)
        {
            var currentDate = DateTime.Now;
            ReportDocument reportDocument = new ReportDocument();
            var company = context.Companies.FirstOrDefault();
            if(showBarCode)
            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/InventoryBarCode.rpt")));
            else
                reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/Inventory.rpt")));

            reportDocument.SetDataSource(context.ArticleSites.Where(x => ids.Contains(x.Article.Id) && x.IdSite == idSite).ToList().Select(x => new
            {
                Date = currentDate,
                x.Article.Ref,
                x.Article.Designation,
                x.Article.Unite,
                Qte = x.QteStock,
                x.Article.BarCode,
                Titre = titre,
            }));

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Inventory.pdf",
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