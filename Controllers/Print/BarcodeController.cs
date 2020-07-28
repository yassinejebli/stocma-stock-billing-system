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

        public ActionResult MultipleEtiquettes([FromUri] IDictionary<Guid, int> ids)
        {
            ReportDocument reportDocument = new ReportDocument();
            var company = context.Companies.FirstOrDefault();

            reportDocument.Load(
                   Path.Combine(this.Server.MapPath("~/CrystalReports/BarcodeLabels.rpt")));
            var datasource = new List<dynamic>();
            ids.ForEach(x =>
            {
                var article = context.Articles.Find(x.Key);
                for (var i = 0; i < x.Value; i++)
                {
                    datasource.Add(new
                    {
                        BarCode = article.BarCode,
                        Designation = article.Designation,
                    });
                }
            });

            reportDocument.SetDataSource(datasource);

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