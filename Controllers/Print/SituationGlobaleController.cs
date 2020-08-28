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
    public class SituationGlobaleController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Clients()
        {
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/SituationClients.rpt")));
            reportDocument.SetDataSource(
               context.Clients.Where(x=>!x.IsClientDivers && !x.Disabled).Select(x=> new
               {
                   Client = x.Name.ToUpper(),
                   Solde = x.Paiements.Sum(y => (float?) (y.Debit - y.Credit)) ?? 0
               }).OrderByDescending(x => x.Solde).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult Fournisseurs()
        {
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/SituationFournisseurs.rpt")));
            reportDocument.SetDataSource(
               context.Fournisseurs.Where(x => !x.Disabled).Select(x => new
               {
                   Client = x.Name.ToUpper(),
                   Solde = x.PaiementFs.Sum(y => (float?)(y.Debit - y.Credit)) ?? 0
               }).OrderByDescending(x=>x.Solde).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }
    }
}