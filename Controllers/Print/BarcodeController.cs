using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using WebApplication1.DATA;

namespace WebApplication1.Controllers.Print
{
    public class BarcodeController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult SingleEtiquette(string BarCode, string Designation)
        {
            ReportDocument reportDocument = new ReportDocument();
            var company = context.Companies.FirstOrDefault();

            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/BarcodeLabels.rpt")));
            var dataSource = new List<dynamic>();

            dataSource.Add(new
            {
                BarCode,
                Designation
            });
            reportDocument.SetDataSource(dataSource);

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Barcode.pdf",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult MultipleEtiquettes([FromUri] Guid[] ids)
        {
            ReportDocument reportDocument = new ReportDocument();
            var company = context.Companies.FirstOrDefault();

            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/BarcodeLabels.rpt")));

            reportDocument.SetDataSource(context.Articles.Where(x => ids.Contains(x.Id)).Select(x => new
            {
                x.BarCode,
                x.Designation,
            }).ToList());

            Response.Buffer = false;
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "Barcode.pdf",
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