// Decompiled with JetBrains decompiler
// Type: WebApplication1.Controllers.AdministrationController
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Data.Entity;
using WebApplication1.DATA;
using System.Net.Mail;
using System.Net;

namespace WebApplication1.Controllers
{
    [OutputCache(Duration = 15, VaryByParam = "None")]
    [Authorize]
    public class AdministrationController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public ActionResult Index()
        {
           // if (this.User.Identity.Name.Contains("cmp") || User.Identity.Name.Contains("sec"))
              //  return View("BonLivraison");
           // if(User.Identity.Name.Contains("comptabilite"))
               // return View("ArticleFacture");

            return View();
        }

        public ActionResult ClientsUpPlafond()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult JournalConnexion()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult FakeFactureF()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult ListFakeFactureF()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult PopUpPrintFakeFactureF()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }
        public ActionResult CodeSecurite()
        {
            return View();
        }
        public ActionResult MesChequeEffetFournisseur()
        {
            return View();
        }
        public ActionResult ArticleGaz()
        {
            return View();
        }

        public ActionResult ReleveFacture()
        {
            return View();
        }

        public ActionResult ExportFacture(Guid IdFacture,bool Cachet = false)
        {
            StatistiqueController statistiqueController = new StatistiqueController();
            Dictionary<double, double> source = statistiqueController.totalWithTVAFacture(IdFacture);
            double num1 =
                source.Where(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 0.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num2 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 7.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num3 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 10.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num4 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 14.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num5 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 20.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            Decimal number =
                (Decimal)
                this.context.FactureItems.Where<FactureItem>(
                        (Expression<Func<FactureItem, bool>>) (x => x.Facture.Id == IdFacture))
                    .Sum<FactureItem>((Expression<Func<FactureItem, float>>) (x => x.Qte*x.Pu));


            Decimal number2 =
                (Decimal)
                this.context.FactureItems.Where<FactureItem>(
                        (Expression<Func<FactureItem, bool>>)(x => x.Facture.Id == IdFacture))
                    .Sum(x => x.Qte * (x.Pu + (x.Pu * (x.Article.TVA ?? 20)/100)));


            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/Facture" + upper + ".rpt")));

            if(upper == "SUIV" || upper == "SBCIT")
            {
                reportDocument.SetDataSource(
                this.context.FactureItems.Where(
                    (x => x.Facture.Id == IdFacture)).Select(x => new
                    {
                        NumBon = x.Facture.NumBon,
                        Date = x.Facture.Date,
                        Client = x.Facture.Client.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.TotalHT,
                        TVA = x.Article.TVA ?? 20.0f,
                        Unite = x.Article.Unite,
                        Adresse = x.Facture.Client.Adresse,
                        ClientName = x.Facture.ClientName,
                        TypeReglement = x.Facture.TypeReglement,
                        Comment = x.Facture.Comment,
                        NumBL = x.NumBL,
                        NumBC = x.NumBC,
                        ICE = x.Facture.Client.ICE
                    }).ToList());
                reportDocument.SetParameterValue("Cachet", Cachet);

            }
            else
            {
                reportDocument.SetDataSource(
                this.context.FactureItems.Where<FactureItem>(
                    (Expression<Func<FactureItem, bool>>)(x => x.Facture.Id == IdFacture)).Select(x => new
                    {
                        NumBon = x.Facture.NumBon,
                        Date = x.Facture.Date,
                        Client = x.Facture.Client.Name,
                        ClientName = x.Facture.ClientName,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.TotalHT,
                        TVA = x.Article.TVA ?? 20.0f,
                        Unite = x.Article.Unite
                    }).ToList());
            }

         
            if (upper == "TSR")
                (reportDocument.ReportDefinition.ReportObjects["modeReglement"] as TextObject).Text =
                    StatistiqueController.getModeReglementFacture(IdFacture);


            if (upper != "SUIV" && upper != "SBCIT")
            {
                double num6 = num1 / 1.0;
                TextObject reportObject1 = reportDocument.ReportDefinition.ReportObjects["t0"] as TextObject;
                string str1 = num6.ToString("F");
                reportObject1.Text = str1;

                TextObject reportObject2 = reportDocument.ReportDefinition.ReportObjects["t7"] as TextObject;
                num6 = num2 / 1.07;
                string str2 = num6.ToString("F");
                reportObject2.Text = str2;
                TextObject reportObject3 = reportDocument.ReportDefinition.ReportObjects["t10"] as TextObject;
                num6 = num3 / 1.1;
                string str3 = num6.ToString("F");
                reportObject3.Text = str3;
                TextObject reportObject4 = reportDocument.ReportDefinition.ReportObjects["t14"] as TextObject;
                num6 = num4 / 1.14;
                string str4 = num6.ToString("F");
                reportObject4.Text = str4;
                TextObject reportObject5 = reportDocument.ReportDefinition.ReportObjects["t20"] as TextObject;
                num6 = num5 / 1.2;
                string str5 = num6.ToString("F");
                reportObject5.Text = str5;
                TextObject reportObject6 = reportDocument.ReportDefinition.ReportObjects["total0"] as TextObject;
                num6 = num1 - num1 / 1.0;
                string str6 = num6.ToString("F");
                reportObject6.Text = str6;
                TextObject reportObject7 = reportDocument.ReportDefinition.ReportObjects["total7"] as TextObject;
                num6 = num2 - num2 / 1.07;
                string str7 = num6.ToString("F");
                reportObject7.Text = str7;
                TextObject reportObject8 = reportDocument.ReportDefinition.ReportObjects["total10"] as TextObject;
                num6 = num3 - num3 / 1.1;
                string str8 = num6.ToString("F");
                reportObject8.Text = str8;
                (reportDocument.ReportDefinition.ReportObjects["total14"] as TextObject).Text =
               (num4 - num4 / 1.14).ToString("F");
                (reportDocument.ReportDefinition.ReportObjects["total20"] as TextObject).Text =
                    (num5 - num5 / 1.2).ToString("F");
                number = Decimal.Parse(String.Format("{0:.##}", number));

                (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
              statistiqueController.DecimalToWords(number);
            }
            else
            {
                ////////////////////// suiv


                double num6 = num1 / 1.0;


                TextObject reportObject2 = reportDocument.ReportDefinition.ReportObjects["t7"] as TextObject;
                num6 = num2;
                string str2 = num6.ToString("F");
                reportObject2.Text = str2;
                TextObject reportObject3 = reportDocument.ReportDefinition.ReportObjects["t10"] as TextObject;
                num6 = num3;
                string str3 = num6.ToString("F");
                reportObject3.Text = str3;
                TextObject reportObject4 = reportDocument.ReportDefinition.ReportObjects["t14"] as TextObject;
                num6 = num4;
                string str4 = num6.ToString("F");
                reportObject4.Text = str4;
                TextObject reportObject5 = reportDocument.ReportDefinition.ReportObjects["t20"] as TextObject;
                num6 = num5;
                string str5 = num6.ToString("F");
                reportObject5.Text = str5;



                //tax
                TextObject reportObject7 = reportDocument.ReportDefinition.ReportObjects["total7"] as TextObject;
                num6 = num2 * 0.07;
                string str7 = num6.ToString("F");
                reportObject7.Text = str7;
                TextObject reportObject8 = reportDocument.ReportDefinition.ReportObjects["total10"] as TextObject;
                num6 = num3 * 0.1;
                string str8 = num6.ToString("F");
                reportObject8.Text = str8;
                (reportDocument.ReportDefinition.ReportObjects["total14"] as TextObject).Text =
               (num4 * 0.14).ToString("F");
                (reportDocument.ReportDefinition.ReportObjects["total20"] as TextObject).Text =
                    (num5 * 0.2).ToString("F");






                TextObject reportObjectTotalTaxe = reportDocument.ReportDefinition.ReportObjects["totalTaxe"] as TextObject;
                TextObject reportObjectTotalBase = reportDocument.ReportDefinition.ReportObjects["totalBase"] as TextObject;
                reportObjectTotalTaxe.Text = ((num2 * 0.07)+(num3 * 0.1)+ (num4 * 0.14)+ (num5 * 0.2)).ToString("F");
                reportObjectTotalBase.Text = (num2 + num3 + num4 + num5).ToString("F");

                if(context.Factures.Where(x=>x.Id == IdFacture && (x.TypeReglement == "Au-Comptant" || x.TypeReglement == "au-comptant")).FirstOrDefault() != null)
                {
                    Decimal drt = number2 * (Decimal)0.0025;//droit timbre if espece
                    number2 += drt;
                }
                number2 = Decimal.Parse(String.Format("{0:.##}", number2));


                string totalmots = statistiqueController.DecimalToWords(number2); ;
               
                (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
              totalmots;
            }

           
          
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }



        public ActionResult EmailFacture(Guid IdFacture, bool Cachet = false)
        {
            StatistiqueController statistiqueController = new StatistiqueController();
            Dictionary<double, double> source = statistiqueController.totalWithTVAFacture(IdFacture);
            double num1 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 0.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num2 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 7.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num3 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 10.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num4 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 14.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num5 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 20.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            Decimal number =
                (Decimal)
                this.context.FactureItems.Where<FactureItem>(
                        (Expression<Func<FactureItem, bool>>)(x => x.Facture.Id == IdFacture))
                    .Sum<FactureItem>((Expression<Func<FactureItem, float>>)(x => x.Qte * x.Pu));


            Decimal number2 =
                (Decimal)
                this.context.FactureItems.Where<FactureItem>(
                        (Expression<Func<FactureItem, bool>>)(x => x.Facture.Id == IdFacture))
                    .Sum(x => x.Qte * (x.Pu + (x.Pu * (x.Article.TVA ?? 20) / 100)));


            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/Facture" + upper + ".rpt")));

            if (upper == "SUIV" || upper == "SBCIT")
            {
                reportDocument.SetDataSource(
                this.context.FactureItems.Where(
                    (x => x.Facture.Id == IdFacture)).Select(x => new
                    {
                        NumBon = x.Facture.NumBon,
                        Date = x.Facture.Date,
                        Client = x.Facture.Client.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.TotalHT,
                        TVA = x.Article.TVA ?? 20.0f,
                        Unite = x.Article.Unite,
                        Adresse = x.Facture.Client.Adresse,
                        TypeReglement = x.Facture.TypeReglement,
                        Comment = x.Facture.Comment,
                        NumBL = x.NumBL,
                        NumBC = x.NumBC
                    }).ToList());
                reportDocument.SetParameterValue("Cachet", Cachet);

            }
            else
            {
                reportDocument.SetDataSource(
                this.context.FactureItems.Where<FactureItem>(
                    (Expression<Func<FactureItem, bool>>)(x => x.Facture.Id == IdFacture)).Select(x => new
                    {
                        NumBon = x.Facture.NumBon,
                        Date = x.Facture.Date,
                        Client = x.Facture.Client.Name,
                        ClientName = x.Facture.ClientName,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.TotalHT,
                        TVA = x.Article.TVA ?? 20.0f,
                        Unite = x.Article.Unite
                    }).ToList());
            }


            if (upper == "TSR")
                (reportDocument.ReportDefinition.ReportObjects["modeReglement"] as TextObject).Text =
                    StatistiqueController.getModeReglementFacture(IdFacture);







            if (upper != "SUIV" && upper != "SBCIT")
            {

                double num6 = num1 / 1.0;
                TextObject reportObject1 = reportDocument.ReportDefinition.ReportObjects["t0"] as TextObject;
                string str1 = num6.ToString("F");
                reportObject1.Text = str1;

                TextObject reportObject2 = reportDocument.ReportDefinition.ReportObjects["t7"] as TextObject;
                num6 = num2 / 1.07;
                string str2 = num6.ToString("F");
                reportObject2.Text = str2;
                TextObject reportObject3 = reportDocument.ReportDefinition.ReportObjects["t10"] as TextObject;
                num6 = num3 / 1.1;
                string str3 = num6.ToString("F");
                reportObject3.Text = str3;
                TextObject reportObject4 = reportDocument.ReportDefinition.ReportObjects["t14"] as TextObject;
                num6 = num4 / 1.14;
                string str4 = num6.ToString("F");
                reportObject4.Text = str4;
                TextObject reportObject5 = reportDocument.ReportDefinition.ReportObjects["t20"] as TextObject;
                num6 = num5 / 1.2;
                string str5 = num6.ToString("F");
                reportObject5.Text = str5;
                TextObject reportObject6 = reportDocument.ReportDefinition.ReportObjects["total0"] as TextObject;
                num6 = num1 - num1 / 1.0;
                string str6 = num6.ToString("F");
                reportObject6.Text = str6;
                TextObject reportObject7 = reportDocument.ReportDefinition.ReportObjects["total7"] as TextObject;
                num6 = num2 - num2 / 1.07;
                string str7 = num6.ToString("F");
                reportObject7.Text = str7;
                TextObject reportObject8 = reportDocument.ReportDefinition.ReportObjects["total10"] as TextObject;
                num6 = num3 - num3 / 1.1;
                string str8 = num6.ToString("F");
                reportObject8.Text = str8;
                (reportDocument.ReportDefinition.ReportObjects["total14"] as TextObject).Text =
               (num4 - num4 / 1.14).ToString("F");
                (reportDocument.ReportDefinition.ReportObjects["total20"] as TextObject).Text =
                    (num5 - num5 / 1.2).ToString("F");
                number = Decimal.Parse(String.Format("{0:.##}", number));

                (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
              statistiqueController.DecimalToWords(number);
            }
            else
            {
                ////////////////////// suiv


                double num6 = num1 / 1.0;


                TextObject reportObject2 = reportDocument.ReportDefinition.ReportObjects["t7"] as TextObject;
                num6 = num2;
                string str2 = num6.ToString("F");
                reportObject2.Text = str2;
                TextObject reportObject3 = reportDocument.ReportDefinition.ReportObjects["t10"] as TextObject;
                num6 = num3;
                string str3 = num6.ToString("F");
                reportObject3.Text = str3;
                TextObject reportObject4 = reportDocument.ReportDefinition.ReportObjects["t14"] as TextObject;
                num6 = num4;
                string str4 = num6.ToString("F");
                reportObject4.Text = str4;
                TextObject reportObject5 = reportDocument.ReportDefinition.ReportObjects["t20"] as TextObject;
                num6 = num5;
                string str5 = num6.ToString("F");
                reportObject5.Text = str5;



                //taxe
                TextObject reportObject7 = reportDocument.ReportDefinition.ReportObjects["total7"] as TextObject;
                num6 = num2 * 0.07;
                string str7 = num6.ToString("F");
                reportObject7.Text = str7;
                TextObject reportObject8 = reportDocument.ReportDefinition.ReportObjects["total10"] as TextObject;
                num6 = num3 * 0.1;
                string str8 = num6.ToString("F");
                reportObject8.Text = str8;
                (reportDocument.ReportDefinition.ReportObjects["total14"] as TextObject).Text =
               (num4 * 0.14).ToString("F");
                (reportDocument.ReportDefinition.ReportObjects["total20"] as TextObject).Text =
                    (num5 * 0.2).ToString("F");






                TextObject reportObjectTotalTaxe = reportDocument.ReportDefinition.ReportObjects["totalTaxe"] as TextObject;
                TextObject reportObjectTotalBase = reportDocument.ReportDefinition.ReportObjects["totalBase"] as TextObject;
                reportObjectTotalTaxe.Text = ((num2 * 0.07) + (num3 * 0.1) + (num4 * 0.14) + (num5 * 0.2)).ToString("F");
                reportObjectTotalBase.Text = (num2 + num3 + num4 + num5).ToString("F");

                if (context.Factures.Where(x => x.Id == IdFacture && (x.TypeReglement == "Au-Comptant" || x.TypeReglement == "au-comptant")).FirstOrDefault() != null)
                {
                    Decimal drt = number2 * (Decimal)0.0025;//droit timbre if espece
                    number2 += drt;
                }
                number2 = Decimal.Parse(String.Format("{0:.##}", number2));


                string totalmots = statistiqueController.DecimalToWords(number2); ;

                (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
              totalmots;
            }



            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();

            MailMessage mailMessage = null;

            try
            {
                Company company = StatistiqueController.getCompany();

                Facture Facture = context.Factures.Where(x => x.Id == IdFacture).FirstOrDefault();
                SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
                sc.Credentials = new NetworkCredential(company.Adresse, company.CodeSecurite);
                sc.EnableSsl = true;
                mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(company.Adresse);
                if (Facture.Client.Email != null && Facture.Client.Email != "")
                {
                    mailMessage.To.Add(Facture.Client.Email);

                }
                else
                {
                    return (ActionResult)this.Json(new
                    {
                        envoye = -1
                    }, JsonRequestBehavior.AllowGet);
                }
                mailMessage.Subject = "Facture N° " + Facture.NumBon;
                mailMessage.Body = "<h3>Bonjour,</h3><div>Vous trouverez ci-joint la facture N° " + Facture.NumBon + "</div><div style='margin-top:50px;'>Cordialement,</div><div style='margin-top:100px;'>" + company.CompleteName + "<br/>" + company.AdresseSociete1 + "<br/>" + company.AdresseSociete2 + "<br/>" + company.AdresseSociete3 + "<br/>" + company.AdresseSociete4 + "<br/>Tel : " + company.Tel + "<br/>Tel/Fax : " + company.Fax + "<br/>E-mail : " + company.Adresse + "</div>";
                mailMessage.IsBodyHtml = true;

                Attachment attachment = new Attachment(stream, "Facture_" + Facture.NumBon.Replace("/", "-") + ".pdf");

                mailMessage.Attachments.Add(attachment);

                sc.Send(mailMessage);
                return (ActionResult)this.Json(new
                {
                    envoye = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return (ActionResult)this.Json(new
                {
                    envoye = 0,
                    ex = ex.Message,
                    innerEx = ex.InnerException.Message
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }

        }

        public ActionResult ExportFacture2(Guid IdFacture)
        {
            StatistiqueController statistiqueController = new StatistiqueController();
            Dictionary<double, double> source = statistiqueController.totalWithTVAFacture(IdFacture);
            double num1 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 0.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num2 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 7.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num3 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 10.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num4 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 14.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num5 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 20.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            Decimal number =
                (Decimal)
                this.context.FactureItems.Where<FactureItem>(
                        (Expression<Func<FactureItem, bool>>) (x => x.Facture.Id == IdFacture))
                    .Sum<FactureItem>((Expression<Func<FactureItem, float>>) (x => x.TotalHT));
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/FactureELEC.rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.FactureItems.Where<FactureItem>(
                    (Expression<Func<FactureItem, bool>>) (x => x.Facture.Id == IdFacture)).Select(x => new
                {
                    NumBon = x.Facture.NumBon,
                    Date = x.Facture.Date,
                    Client = x.Facture.Client.Name,
                    Ref = x.Article.Ref,
                    Designation = x.Article.Designation,
                    Qte = x.Qte,
                    PU = x.Pu,
                    Comment = x.Facture.Comment,
                    TotalHT = x.TotalHT,
                    TVA = x.Article.TVA ?? 20.0f,
                    Unite = x.Article.Unite
                }).ToList());

            string modeReglement = StatistiqueController.getModeReglementFacture(IdFacture);
            if (upper == "TSR" || upper == "AQK" || upper == "SHMZ")
                (reportDocument.ReportDefinition.ReportObjects["modeReglement"] as TextObject).Text = modeReglement;
                 


            TextObject reportObject1 = reportDocument.ReportDefinition.ReportObjects["t0"] as TextObject;
            double num6 = num1 / 1.0;
            string str1 = num6.ToString("F");
            reportObject1.Text = str1;
            TextObject reportObject2 = reportDocument.ReportDefinition.ReportObjects["t7"] as TextObject;
            num6 = num2 / 1.07;
            string str2 = num6.ToString("F");
            reportObject2.Text = str2;
            TextObject reportObject3 = reportDocument.ReportDefinition.ReportObjects["t10"] as TextObject;
            num6 = num3 / 1.1;
            string str3 = num6.ToString("F");
            reportObject3.Text = str3;
            TextObject reportObject4 = reportDocument.ReportDefinition.ReportObjects["t14"] as TextObject;
            num6 = num4 / 1.14;
            string str4 = num6.ToString("F");
            reportObject4.Text = str4;
            TextObject reportObject5 = reportDocument.ReportDefinition.ReportObjects["t20"] as TextObject;
            num6 = num5 / 1.2;
            string str5 = num6.ToString("F");
            reportObject5.Text = str5;
            TextObject reportObject6 = reportDocument.ReportDefinition.ReportObjects["total0"] as TextObject;
            num6 = num1 - num1 / 1.0;
            string str6 = num6.ToString("F");
            reportObject6.Text = str6;
            TextObject reportObject7 = reportDocument.ReportDefinition.ReportObjects["total7"] as TextObject;
            num6 = num2 - num2 / 1.07;
            string str7 = num6.ToString("F");
            reportObject7.Text = str7;
            TextObject reportObject8 = reportDocument.ReportDefinition.ReportObjects["total10"] as TextObject;
            num6 = num3 - num3 / 1.1;
            string str8 = num6.ToString("F");
            reportObject8.Text = str8;
            (reportDocument.ReportDefinition.ReportObjects["total14"] as TextObject).Text =
                (num4 - num4 / 1.14).ToString("F");
            (reportDocument.ReportDefinition.ReportObjects["total20"] as TextObject).Text =
                (num5 - num5 / 1.2).ToString("F");
            number = Decimal.Parse(String.Format("{0:.##}", number));

            (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
                statistiqueController.DecimalToWords(number);
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportFakeFacture(Guid IdFakeFacture)
        {
            StatistiqueController statistiqueController = new StatistiqueController();
            Dictionary<double, double> source = statistiqueController.totalWithTVAFakeFacture(IdFakeFacture);
            double num1 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 0.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num2 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 7.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num3 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 10.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num4 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 14.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num5 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 20.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            Decimal number =
                (Decimal)
                this.context.FakeFactureItems.Where<FakeFactureItem>(
                        (Expression<Func<FakeFactureItem, bool>>) (x => x.FakeFacture.Id == IdFakeFacture))
                    .Sum<FakeFactureItem>((Expression<Func<FakeFactureItem, float>>) (x => x.TotalHT));
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/Facture" + upper + ".rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.FakeFactureItems.Where<FakeFactureItem>(
                    (Expression<Func<FakeFactureItem, bool>>) (x => x.FakeFacture.Id == IdFakeFacture)).Select(x => new
                {
                    NumBon = x.FakeFacture.NumBon,
                    Date = x.FakeFacture.Date,
                    Client = x.FakeFacture.Client.Name,
                    Ref = x.ArticleFacture.Ref,
                        ClientName = x.FakeFacture.ClientName,

                        Designation = x.ArticleFacture.Designation,
                    Qte = x.Qte,
                    PU = x.Pu,
                        Comment = x.FakeFacture.Comment,
                
                        TotalHT = x.TotalHT,
                    TVA = x.ArticleFacture.TVA ?? 20.0f,
                    Unite = x.ArticleFacture.Unite
                }).ToList());
            if (upper == "TSR" || upper == "AQK" || upper == "SHMZ" || upper == "EAS")
                (reportDocument.ReportDefinition.ReportObjects["modeReglement"] as TextObject).Text =
                    StatistiqueController.getModeReglementFakeFacture(IdFakeFacture);
            TextObject reportObject1 = reportDocument.ReportDefinition.ReportObjects["t0"] as TextObject;
            double num6 = num1 / 1.0;
            string str1 = num6.ToString("F");
            reportObject1.Text = str1;
            TextObject reportObject2 = reportDocument.ReportDefinition.ReportObjects["t7"] as TextObject;
            num6 = num2 / 1.07;
            string str2 = num6.ToString("F");
            reportObject2.Text = str2;
            TextObject reportObject3 = reportDocument.ReportDefinition.ReportObjects["t10"] as TextObject;
            num6 = num3 / 1.1;
            string str3 = num6.ToString("F");
            reportObject3.Text = str3;
            TextObject reportObject4 = reportDocument.ReportDefinition.ReportObjects["t14"] as TextObject;
            num6 = num4 / 1.14;
            string str4 = num6.ToString("F");
            reportObject4.Text = str4;
            TextObject reportObject5 = reportDocument.ReportDefinition.ReportObjects["t20"] as TextObject;
            num6 = num5 / 1.2;
            string str5 = num6.ToString("F");
            reportObject5.Text = str5;
            TextObject reportObject6 = reportDocument.ReportDefinition.ReportObjects["total0"] as TextObject;
            num6 = num1 - num1 / 1.0;
            string str6 = num6.ToString("F");
            reportObject6.Text = str6;
            TextObject reportObject7 = reportDocument.ReportDefinition.ReportObjects["total7"] as TextObject;
            num6 = num2 - num2 / 1.07;
            string str7 = num6.ToString("F");
            reportObject7.Text = str7;
            TextObject reportObject8 = reportDocument.ReportDefinition.ReportObjects["total10"] as TextObject;
            num6 = num3 - num3 / 1.1;
            string str8 = num6.ToString("F");
            reportObject8.Text = str8;
            (reportDocument.ReportDefinition.ReportObjects["total14"] as TextObject).Text =
                (num4 - num4 / 1.14).ToString("F");
            (reportDocument.ReportDefinition.ReportObjects["total20"] as TextObject).Text =
                (num5 - num5 / 1.2).ToString("F");
            number = Decimal.Parse(String.Format("{0:.##}", number));

            if(upper != "EAS")
                if (context.FakeFactures.Where(x => x.Id == IdFakeFacture && x.Comment.ToLower().Contains("esp")).FirstOrDefault() != null)
                {
                    Decimal drt = number * (Decimal)0.0025;//droit timbre if espece
                    number += drt;
                }
            number = Decimal.Parse(String.Format("{0:.##}", number));


            //string totalmots = statistiqueController.DecimalToWords(number); ;

            (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
                statistiqueController.DecimalToWords(number);
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult EmailFakeFacture(Guid IdFakeFacture)
        {
            StatistiqueController statistiqueController = new StatistiqueController();
            Dictionary<double, double> source = statistiqueController.totalWithTVAFakeFacture(IdFakeFacture);
            double num1 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 0.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num2 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 7.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num3 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 10.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num4 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 14.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num5 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 20.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            Decimal number =
                (Decimal)
                this.context.FakeFactureItems.Where<FakeFactureItem>(
                        (Expression<Func<FakeFactureItem, bool>>)(x => x.FakeFacture.Id == IdFakeFacture))
                    .Sum<FakeFactureItem>((Expression<Func<FakeFactureItem, float>>)(x => x.TotalHT));
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/Facture" + upper + ".rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.FakeFactureItems.Where<FakeFactureItem>(
                    (Expression<Func<FakeFactureItem, bool>>)(x => x.FakeFacture.Id == IdFakeFacture)).Select(x => new
                    {
                        NumBon = x.FakeFacture.NumBon,
                        Date = x.FakeFacture.Date,
                        Client = x.FakeFacture.Client.Name,
                        Ref = x.ArticleFacture.Ref,
                        ClientName = x.FakeFacture.ClientName,

                        Designation = x.ArticleFacture.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.TotalHT,
                        TVA = x.ArticleFacture.TVA ?? 20.0f,
                        Unite = x.ArticleFacture.Unite
                    }).ToList());
            if (upper == "TSR" || upper == "AQK" || upper == "SHMZ")
                (reportDocument.ReportDefinition.ReportObjects["modeReglement"] as TextObject).Text =
                    StatistiqueController.getModeReglementFakeFacture(IdFakeFacture);
            TextObject reportObject1 = reportDocument.ReportDefinition.ReportObjects["t0"] as TextObject;
            double num6 = num1 / 1.0;
            string str1 = num6.ToString("F");
            reportObject1.Text = str1;
            TextObject reportObject2 = reportDocument.ReportDefinition.ReportObjects["t7"] as TextObject;
            num6 = num2 / 1.07;
            string str2 = num6.ToString("F");
            reportObject2.Text = str2;
            TextObject reportObject3 = reportDocument.ReportDefinition.ReportObjects["t10"] as TextObject;
            num6 = num3 / 1.1;
            string str3 = num6.ToString("F");
            reportObject3.Text = str3;
            TextObject reportObject4 = reportDocument.ReportDefinition.ReportObjects["t14"] as TextObject;
            num6 = num4 / 1.14;
            string str4 = num6.ToString("F");
            reportObject4.Text = str4;
            TextObject reportObject5 = reportDocument.ReportDefinition.ReportObjects["t20"] as TextObject;
            num6 = num5 / 1.2;
            string str5 = num6.ToString("F");
            reportObject5.Text = str5;
            TextObject reportObject6 = reportDocument.ReportDefinition.ReportObjects["total0"] as TextObject;
            num6 = num1 - num1 / 1.0;
            string str6 = num6.ToString("F");
            reportObject6.Text = str6;
            TextObject reportObject7 = reportDocument.ReportDefinition.ReportObjects["total7"] as TextObject;
            num6 = num2 - num2 / 1.07;
            string str7 = num6.ToString("F");
            reportObject7.Text = str7;
            TextObject reportObject8 = reportDocument.ReportDefinition.ReportObjects["total10"] as TextObject;
            num6 = num3 - num3 / 1.1;
            string str8 = num6.ToString("F");
            reportObject8.Text = str8;
            (reportDocument.ReportDefinition.ReportObjects["total14"] as TextObject).Text =
                (num4 - num4 / 1.14).ToString("F");
            (reportDocument.ReportDefinition.ReportObjects["total20"] as TextObject).Text =
                (num5 - num5 / 1.2).ToString("F");
            number = Decimal.Parse(String.Format("{0:.##}", number));

            (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
                statistiqueController.DecimalToWords(number);
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();

            MailMessage mailMessage = null;

            try
            {
                Company company = StatistiqueController.getCompany();

                FakeFacture FakeFacture = context.FakeFactures.Where(x => x.Id == IdFakeFacture).FirstOrDefault();
                SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
                sc.Credentials = new NetworkCredential(company.Adresse, company.CodeSecurite);
                sc.EnableSsl = true;
                mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(company.Adresse);
                if (FakeFacture.Client.Email != null && FakeFacture.Client.Email != "")
                {
                    mailMessage.To.Add(FakeFacture.Client.Email);

                }
                else
                {
                    return (ActionResult)this.Json(new
                    {
                        envoye = -1
                    }, JsonRequestBehavior.AllowGet);
                }
                mailMessage.Subject = "Facture N° " + FakeFacture.NumBon;
                mailMessage.Body = "<h3>Bonjour,</h3><div>Vous trouverez ci-joint la facture N° " + FakeFacture.NumBon + "</div><div style='margin-top:50px;'>Cordialement,</div><div style='margin-top:100px;'>" + company.CompleteName + "<br/>" + company.AdresseSociete1 + "<br/>" + company.AdresseSociete2 + "<br/>" + company.AdresseSociete3 + "<br/>" + company.AdresseSociete4 + "<br/>Tel : " + company.Tel + "<br/>Tel/Fax : " + company.Fax + "<br/>E-mail : " + company.Adresse + "</div>";
                mailMessage.IsBodyHtml = true;

                Attachment attachment = new Attachment(stream, "Facture_" + FakeFacture.NumBon.Replace("/", "-") + ".pdf");

                mailMessage.Attachments.Add(attachment);

                sc.Send(mailMessage);
                return (ActionResult)this.Json(new
                {
                    envoye = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return (ActionResult)this.Json(new
                {
                    envoye = 0
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }
        }


        public ActionResult ExportFakeFacture2(Guid IdFakeFacture)
        {
            StatistiqueController statistiqueController = new StatistiqueController();
            Dictionary<double, double> source = statistiqueController.totalWithTVAFakeFacture(IdFakeFacture);
            double num1 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 0.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num2 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 7.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num3 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 10.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num4 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 14.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num5 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 20.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            Decimal number =
                (Decimal)
                this.context.FakeFactureItems.Where<FakeFactureItem>(
                        (Expression<Func<FakeFactureItem, bool>>) (x => x.FakeFacture.Id == IdFakeFacture))
                    .Sum<FakeFactureItem>((Expression<Func<FakeFactureItem, float>>) (x => x.TotalHT));
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/FactureELEC.rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.FakeFactureItems.Where<FakeFactureItem>(
                    (Expression<Func<FakeFactureItem, bool>>) (x => x.FakeFacture.Id == IdFakeFacture)).Select(x => new
                {
                    NumBon = x.FakeFacture.NumBon,
                    Date = x.FakeFacture.Date,
                    Client = x.FakeFacture.Client.Name,
                    Ref = x.ArticleFacture.Ref,
                    Designation = x.ArticleFacture.Designation,
                        ClientName = x.FakeFacture.ClientName,
                    Qte = x.Qte,
                    PU = x.Pu,
                    TotalHT = x.TotalHT,
                    TVA = x.ArticleFacture.TVA ?? 20.0f,
                    Unite = x.ArticleFacture.Unite
                }).ToList());
            if (upper == "TSR" || upper == "AQK" || upper == "SHMZ")
                (reportDocument.ReportDefinition.ReportObjects["modeReglement"] as TextObject).Text =
                    StatistiqueController.getModeReglementFakeFacture(IdFakeFacture);
            TextObject reportObject1 = reportDocument.ReportDefinition.ReportObjects["t0"] as TextObject;
            double num6 = num1 / 1.0;
            string str1 = num6.ToString("F");
            reportObject1.Text = str1;
            TextObject reportObject2 = reportDocument.ReportDefinition.ReportObjects["t7"] as TextObject;
            num6 = num2 / 1.07;
            string str2 = num6.ToString("F");
            reportObject2.Text = str2;
            TextObject reportObject3 = reportDocument.ReportDefinition.ReportObjects["t10"] as TextObject;
            num6 = num3 / 1.1;
            string str3 = num6.ToString("F");
            reportObject3.Text = str3;
            TextObject reportObject4 = reportDocument.ReportDefinition.ReportObjects["t14"] as TextObject;
            num6 = num4 / 1.14;
            string str4 = num6.ToString("F");
            reportObject4.Text = str4;
            TextObject reportObject5 = reportDocument.ReportDefinition.ReportObjects["t20"] as TextObject;
            num6 = num5 / 1.2;
            string str5 = num6.ToString("F");
            reportObject5.Text = str5;
            TextObject reportObject6 = reportDocument.ReportDefinition.ReportObjects["total0"] as TextObject;
            num6 = num1 - num1 / 1.0;
            string str6 = num6.ToString("F");
            reportObject6.Text = str6;
            TextObject reportObject7 = reportDocument.ReportDefinition.ReportObjects["total7"] as TextObject;
            num6 = num2 - num2 / 1.07;
            string str7 = num6.ToString("F");
            reportObject7.Text = str7;
            TextObject reportObject8 = reportDocument.ReportDefinition.ReportObjects["total10"] as TextObject;
            num6 = num3 - num3 / 1.1;
            string str8 = num6.ToString("F");
            reportObject8.Text = str8;
            (reportDocument.ReportDefinition.ReportObjects["total14"] as TextObject).Text =
                (num4 - num4 / 1.14).ToString("F");
            (reportDocument.ReportDefinition.ReportObjects["total20"] as TextObject).Text =
                (num5 - num5 / 1.2).ToString("F");
            number = Decimal.Parse(String.Format("{0:.##}", number));

            (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
                statistiqueController.DecimalToWords(number);
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }



        /* public ActionResult ExportBarCodeByCategory(Guid IdCategory)
         {
           ReportDocument reportDocument = new ReportDocument();
           reportDocument.Load(Path.Combine(this.Server.MapPath("~/CrystalReports/CrystalReportTest.rpt")));
           reportDocument.SetDataSource((IEnumerable) this.context.Articles.Where<Article>((Expression<Func<Article, bool>>) (x => x.IdFamille == (Guid?) IdCategory && x.BarCode != default (string) && x.BarCode != "")).Select(x => new
           {
             Ref = x.Ref,
             Designation = x.Designation,
             Qte = x.QteStock,
             Unite = x.Unite,
             BarCode = x.BarCode
           }).OrderBy(x => x.Designation).ToList());
           this.Response.Buffer = false;
           this.Response.ClearContent();
           this.Response.ClearHeaders();
           Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
           stream.Seek(0L, SeekOrigin.Begin);
           reportDocument.Close();
           return File(stream, "application/pdf");
         }*/

        public ActionResult ExportSituationClient(Guid IdClient, DateTime? dateDebut, DateTime? dateFin)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            DateTime dateTime = new DateTime(1970, 1, 1);
            if (dateDebut.HasValue && dateFin.HasValue)
            {
                reportDocument.Load(
                    Path.Combine(this.Server.MapPath("~/CrystalReports/CrystalReportCompteClientParPeriode.rpt")));
                if(StatistiqueController.getCompanyName() == "TSR")
                    reportDocument.Load(
                        Path.Combine(this.Server.MapPath("~/CrystalReports/CrystalReportCompteClientParPeriodeTSR.rpt")));
                reportDocument.SetDataSource(
                    (IEnumerable)
                    this.context.Paiements.Where<Paiement>(
                            (Expression<Func<Paiement, bool>>)
                            (x => x.IdClient == IdClient && (DateTime?)DbFunctions.TruncateTime(x.Date) >= dateDebut && DbFunctions.TruncateTime(x.Date) <= dateFin))
                        .Where(x=>x.Hide != true).Select(x => new
                        {
                            Client = x.Client.Name,
                            NumBon = x.BonLivraison.NumBon,
                            Date = x.Date,
                            Debit = x.Debit,
                            Credit = x.Credit,
                            Type = x.TypePaiement.Name,
                            DateEcheance = x.DateEcheance.ToString(),
                            Commentaire = x.Comment
                        }).OrderBy(x => x.Date).ToList());
                NumberFormatInfo numberFormatInfo = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
                numberFormatInfo.NumberGroupSeparator = " ";
                string str = statistiqueController.SoldeByClient(IdClient)
                    .ToString("#,#.00", (IFormatProvider) numberFormatInfo);
                (reportDocument.ReportDefinition.ReportObjects["solde"] as TextObject).Text = str + "DH";
               

                reportDocument.SetParameterValue("totalAchat",statistiqueController.getTotalAchatByClient(IdClient,dateDebut.Value,dateFin.Value));
                reportDocument.SetParameterValue("dateDebut", dateDebut);
                reportDocument.SetParameterValue("dateFin", dateFin);
                reportDocument.SetParameterValue("soldeBeforeDate", statistiqueController.getSoldeByClientBeforeDate(IdClient,dateDebut.Value));
            }
            else
            {
                reportDocument.Load(Path.Combine(this.Server.MapPath("~/CrystalReports/CrystalReportCompteClient.rpt")));
                reportDocument.SetDataSource(
                    (IEnumerable)
                    this.context.Paiements.Where<Paiement>(
                        (x => x.IdClient == IdClient)).Where(x => x.Hide != true).Select(x => new
                    {
                        Client = x.Client.Name,
                        NumBon = x.BonLivraison.NumBon,
                        Date = x.Date,
                        Debit = x.Debit,
                        Credit = x.Credit,
                        Type = x.TypePaiement.Name,
                        DateEcheance = x.DateEcheance.ToString(),
                        Commentaire = x.Comment
                    }).OrderBy(x => x.Date).ToList());

                reportDocument.SetParameterValue("Solde", statistiqueController.SoldeByClient(IdClient));

            }

            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        //
        public ActionResult ExportSituationRevendeur(Guid IdRevendeur, DateTime dateDebut, DateTime dateFin)
        {
            ReportDocument reportDocument = new ReportDocument();
            NumberFormatInfo numberFormatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            numberFormatInfo.NumberGroupSeparator = " ";
            StatistiqueController statistiqueController = new StatistiqueController();
                reportDocument.Load(
                    Path.Combine(this.Server.MapPath("~/CrystalReports/SituationRevendeur.rpt")));
                reportDocument.SetDataSource(
                    this.context.Paiements.Where(
                            (x => x.Client.IdRevendeur == IdRevendeur && DbFunctions.TruncateTime(x.Date) >= dateDebut.Date && DbFunctions.TruncateTime(x.Date) <= dateFin.Date))
                        .Select(x => new
                        {
                            Revendeur = x.Client.Revendeur.Name,
                            Client = x.Client.Name,
                            NumBon = x.BonLivraison.NumBon,
                            Date = x.Date,
                            Debit = x.Debit,
                            Credit = x.Credit,
                            Type = x.TypePaiement.Name,
                            DateEcheance = x.DateEcheance.ToString(),
                            Commentaire = x.Comment,
                            SoldeByClient = context.Paiements.Where(q => q.IdClient == x.IdClient).Sum(q=>q.Debit - q.Credit)
                        }).OrderBy(x=> x.Client).ThenBy(x => x.Date).ToList());
             


                string str = statistiqueController.SoldeByRevendeur(IdRevendeur)
                    .ToString("#,#.00", (IFormatProvider)numberFormatInfo);
                (reportDocument.ReportDefinition.ReportObjects["solde"] as TextObject).Text = str + "DH";
                (reportDocument.ReportDefinition.ReportObjects["dateDebut"] as TextObject).Text =
                    dateDebut.ToShortDateString();
                (reportDocument.ReportDefinition.ReportObjects["dateFin"] as TextObject).Text =
                    dateFin.ToShortDateString();
            reportDocument.SetParameterValue("marge", statistiqueController.getMargeByRevendeur(IdRevendeur,dateDebut,dateFin));
            reportDocument.SetParameterValue("ca", statistiqueController.getChiffreAffaireByRevendeur(IdRevendeur,dateDebut,dateFin));


            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }


        //ExportSituationBLByDates
        public ActionResult ExportSituationBLByDates(DateTime dateDebut, DateTime dateFin)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/SituationVenteBL.rpt")));
            reportDocument.SetDataSource(
               context.Paiements.Select(x => new
               {
                   Date = x.Date,
                   Debit = x.Debit,
                   Credit = x.Credit,
                   Type = x.TypePaiement.Name,
                   Client = x.Client.Name,
                   NumBon = x.BonLivraison.NumBon ?? "",
                   Comment = x.Comment,
               }).Where(x => DbFunctions.TruncateTime(x.Date) >= dateDebut.Date && DbFunctions.TruncateTime(x.Date) <= dateFin.Date && x.NumBon != "").OrderBy(x => x.Date).ToList());
            reportDocument.SetParameterValue("dateDebut", dateDebut.Date);
            reportDocument.SetParameterValue("dateFin", dateFin.Date);

            if (context.Paiements.Select(x => new
            {
                Date = x.Date,
                Debit = x.Debit,
                Credit = x.Credit,
                Type = x.TypePaiement.Name,
                Client = x.Client.Name,
                NumBon = x.BonLivraison.NumBon ?? "",
                Comment = x.Comment,
            }).Where(x => DbFunctions.TruncateTime(x.Date) >= dateDebut.Date && DbFunctions.TruncateTime(x.Date) <= dateFin.Date && x.NumBon != "").Count() > 0)
            {
                this.Response.Buffer = false;
                this.Response.ClearContent();
                this.Response.ClearHeaders();
                Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0L, SeekOrigin.Begin);
                reportDocument.Close();
                return File(stream, "application/pdf");
            }
            else
            {
                return View("SituationParDate");
            }



        }

        public ActionResult ExportSituationFAByDates(DateTime dateDebut, DateTime dateFin)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/SituationVenteFA.rpt")));
            reportDocument.SetDataSource(
               context.Paiements.Select(x => new
               {
                   Date = x.Date,
                   Debit = x.Debit,
                   Credit = x.Credit,
                   Type = x.TypePaiement.Name,
                   Client = x.Client.Name,
                   NumBon = x.Facture.NumBon ?? "",
                   Comment = x.Comment,
               }).Where(x => DbFunctions.TruncateTime(x.Date) >= dateDebut.Date && DbFunctions.TruncateTime(x.Date) <= dateFin.Date && x.NumBon != "").OrderBy(x => x.Date).ToList());
            reportDocument.SetParameterValue("dateDebut", dateDebut.Date);
            reportDocument.SetParameterValue("dateFin", dateFin.Date);

            if (context.Paiements.Select(x => new
            {
                Date = x.Date,
                Debit = x.Debit,
                Credit = x.Credit,
                Type = x.TypePaiement.Name,
                Client = x.Client.Name,
                NumBon = x.Facture.NumBon ?? "",
                Comment = x.Comment,
            }).Where(x => DbFunctions.TruncateTime(x.Date) >= dateDebut.Date && DbFunctions.TruncateTime(x.Date) <= dateFin.Date && x.NumBon != "").Count() > 0)
            {
                this.Response.Buffer = false;
                this.Response.ClearContent();
                this.Response.ClearHeaders();
                Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0L, SeekOrigin.Begin);
                reportDocument.Close();
                return File(stream, "application/pdf");
            }
            else
            {
                return View("SituationParDate");
            }



        }

        public ActionResult ExportSituationFakeFAByDates(DateTime dateDebut, DateTime dateFin)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/SituationVenteFakeFA.rpt")));
            reportDocument.SetDataSource(
               context.FakeFactures.Select(x => new
               {
                   Date = x.Date,
                   Debit = x.FakeFactureItems.Sum(a=>a.Pu*a.Qte),
                   Credit = 0,
                   Type = "",
                   Client = x.ClientName,
                   NumBon = x.NumBon ?? "",
                   Commentaire = x.Comment,
               }).Where(x => DbFunctions.TruncateTime(x.Date) >= dateDebut.Date && DbFunctions.TruncateTime(x.Date) <= dateFin.Date && x.NumBon != "").OrderBy(x => x.Date).ToList());
            reportDocument.SetParameterValue("dateDebut", dateDebut.Date);
            reportDocument.SetParameterValue("dateFin", dateFin.Date);

           
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
         
        }

        public ActionResult ExportSituationByDate(DateTime dt)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
                reportDocument.Load(
                    Path.Combine(this.Server.MapPath("~/CrystalReports/SituationJournaliere.rpt")));
              

            if (context.Paiements.Select(x => new
            {
                Date = x.Date,
                Debit = x.Debit,
                Credit = x.Credit,
                Type = x.TypePaiement.Name,
                Client = x.Client.Name,
                NumBon = x.BonLivraison.NumBon ?? "",
                Commentaire = x.Comment,
            }).Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date.Day == dt.Day).Count() > 0)
            {
                  reportDocument.SetDataSource(
                   context.Paiements.Select(x => new
                   {
                       Date = x.Date,
                       Debit = x.Debit,
                       Credit = x.Credit,
                       Type = x.TypePaiement.Name,
                       Client = x.Client.Name,
                       NumBon = x.BonLivraison.NumBon ?? "",
                       Commentaire = x.Comment,
                   }).Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date.Day == dt.Day).OrderBy(x => x.Type).ThenBy(x => x.Date).ToList());
            reportDocument.SetParameterValue("date", dt);
            reportDocument.SetParameterValue("Marge", statistiqueController.getMargeByDate(dt));
            reportDocument.SetParameterValue("Depense", statistiqueController.getTotalDepenseByDate(dt));
            reportDocument.SetParameterValue("Caisse", statistiqueController.MontantALaCaisse(dt));
                this.Response.Buffer = false;
                this.Response.ClearContent();
                this.Response.ClearHeaders();
                Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0L, SeekOrigin.Begin);
                reportDocument.Close();
                return File(stream, "application/pdf");
            }
            else
            {
                return View("SituationParDate");
            }



        }

        public ActionResult ExportSituationFournisseur(Guid IdFournisseur, DateTime? dateDebut, DateTime? dateFin)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            DateTime dateTime = new DateTime(1970, 1, 1);
            if (dateDebut.HasValue && dateFin.HasValue)
            {
                reportDocument.Load(
                    Path.Combine(this.Server.MapPath("~/CrystalReports/CrystalReportCompteFournisseurParPeriode.rpt")));
                reportDocument.SetDataSource(
                    (IEnumerable)
                    this.context.PaiementFs.Where<PaiementF>(
                        (Expression<Func<PaiementF, bool>>)
                        (x =>
                            x.IdFournisseur == IdFournisseur && (DateTime?) x.Date >= dateDebut &&
                            (DateTime?) x.Date <= dateFin)).Select(x => new
                    {
                        Client = x.Fournisseur.Name,
                        NumBon = x.BonReception.NumBon,
                        Date = x.Date,
                        Debit = x.Debit,
                        Credit = x.Credit,
                        Type = x.TypePaiement.Name,
                        DateEcheance = x.DateEcheance.ToString(),
                        Commentaire = x.Comment
                    }).OrderByDescending(x => x.Date).ToList());
                NumberFormatInfo numberFormatInfo = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
                numberFormatInfo.NumberGroupSeparator = " ";
                string str = statistiqueController.SoldeByFournisseur(IdFournisseur)
                    .ToString("#,#.00", (IFormatProvider) numberFormatInfo);
                (reportDocument.ReportDefinition.ReportObjects["solde"] as TextObject).Text = str + "DH";
                (reportDocument.ReportDefinition.ReportObjects["dateDebut"] as TextObject).Text =
                    dateDebut.Value.ToShortDateString();
                (reportDocument.ReportDefinition.ReportObjects["dateFin"] as TextObject).Text =
                    dateFin.Value.ToShortDateString();
            }
            else
            {
                reportDocument.Load(
                    Path.Combine(this.Server.MapPath("~/CrystalReports/CrystalReportCompteFournisseur.rpt")));
                reportDocument.SetDataSource(
                    (IEnumerable)
                    this.context.PaiementFs.Where<PaiementF>(
                        (Expression<Func<PaiementF, bool>>) (x => x.IdFournisseur == IdFournisseur)).Select(x => new
                    {
                        Client = x.Fournisseur.Name,
                        NumBon = x.BonReception.NumBon,
                        Date = x.Date,
                        Debit = x.Debit,
                        Credit = x.Credit,
                        Type = x.TypePaiement.Name,
                        DateEcheance = x.DateEcheance.ToString(),
                        Commentaire = x.Comment
                    }).OrderByDescending(x => x.Date).ToList());
            }
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportBonLivraison(Guid IdBonLivraison,bool ShowPrices,bool IsEspece,bool Cachet = false)
        {

            CrystalDecisions.Shared.SharedUtils.RequestLcid = 212;
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();

            reportDocument.Load(
                Path.Combine(Server.MapPath("~/CrystalReports/" + upper + "/BonLivraison" + upper + ".rpt")));


            if (upper == "SUIV" || upper == "SBCIT")
            {
                
                reportDocument.SetDataSource(
                context.BonLivraisonItems.Where(
                       (x => x.BonLivraison.Id == IdBonLivraison))
                   .Select(x => new
                   {
                       NumBon = x.BonLivraison.NumBon,
                       Date = x.BonLivraison.Date,
                       Client = x.BonLivraison.Client.Name,
                       Ref = x.Article.Ref,
                       Designation = x.Article.Designation,
                       Qte = x.Qte,
                       PU = x.Pu,
                       TotalHT = x.TotalHT,
                       Adresse = x.BonLivraison.Client.Adresse,
                       Unite = x.Article.Unite,
                       TVA = x.Article.TVA ?? 20,
                       NumBC = x.NumBC ?? "",
                       ICE = x.BonLivraison.Client.ICE
                   }).ToList());
                reportDocument.SetParameterValue("Cachet", Cachet);

            }
            else
            {
                reportDocument.SetDataSource(
                context.BonLivraisonItems.Where(
                       (x => x.BonLivraison.Id == IdBonLivraison))
                   .Select(x => new
                   {
                       NumBon = x.BonLivraison.NumBon,
                       TypeReglement = x.BonLivraison.TypeReglement,
                       Date = x.BonLivraison.Date,
                       Client = x.BonLivraison.Client.Name,
                       Ref = x.Article.Ref,
                       Designation = x.Article.Designation,
                       Qte = x.Qte,
                       PU = x.Pu,
                       TotalHT = x.TotalHT,
                       Unite = x.Article.Unite
                   }).ToList());
            }

            reportDocument.SetParameterValue("ShowPrices", ShowPrices);
            reportDocument.SetParameterValue("IsEspece", IsEspece);

            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult EmailBonLivraison(Guid IdBonLivraison, bool ShowPrices, bool IsEspece, bool Cachet = false)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();

            reportDocument.Load(
                Path.Combine(Server.MapPath("~/CrystalReports/" + upper + "/BonLivraison" + upper + ".rpt")));


            if (upper == "SUIV" || upper == "SBCIT")
            {

                reportDocument.SetDataSource(
                context.BonLivraisonItems.Where(
                       (x => x.BonLivraison.Id == IdBonLivraison))
                   .Select(x => new
                   {
                       NumBon = x.BonLivraison.NumBon,
                       Date = x.BonLivraison.Date,
                       Client = x.BonLivraison.Client.Name,
                       Ref = x.Article.Ref,
                       Designation = x.Article.Designation,
                       Qte = x.Qte,
                       PU = x.Pu,
                       TotalHT = x.TotalHT,
                       Adresse = x.BonLivraison.Client.Adresse,
                       Unite = x.Article.Unite,
                       TVA = x.Article.TVA ?? 20,
                       NumBC = x.NumBC ?? ""
                   }).ToList());
                reportDocument.SetParameterValue("Cachet", Cachet);

            }
            else
            {
                reportDocument.SetDataSource(
                context.BonLivraisonItems.Where(
                       (x => x.BonLivraison.Id == IdBonLivraison))
                   .Select(x => new
                   {
                       NumBon = x.BonLivraison.NumBon,
                       Date = x.BonLivraison.Date,
                       Client = x.BonLivraison.Client.Name,
                       Ref = x.Article.Ref,
                       Designation = x.Article.Designation,
                       Qte = x.Qte,
                       PU = x.Pu,
                       TotalHT = x.TotalHT,
                       Unite = x.Article.Unite
                   }).ToList());
            }

            reportDocument.SetParameterValue("ShowPrices", ShowPrices);
            reportDocument.SetParameterValue("IsEspece", IsEspece);

            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();

            MailMessage mailMessage = null;

            try
            {
                Company company = StatistiqueController.getCompany();

                BonLivraison BonLivraison = context.BonLivraisons.Where(x => x.Id == IdBonLivraison).FirstOrDefault();
                SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
                sc.Credentials = new NetworkCredential(company.Adresse, company.CodeSecurite);
                sc.EnableSsl = true;
                mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(company.Adresse);
                if (BonLivraison.Client.Email != null && BonLivraison.Client.Email != "")
                {
                    mailMessage.To.Add(BonLivraison.Client.Email);

                }
                else
                {
                    return (ActionResult)this.Json(new
                    {
                        envoye = -1
                    }, JsonRequestBehavior.AllowGet);
                }
                mailMessage.Subject = "Bon De Livraison N° " + BonLivraison.NumBon;
                mailMessage.Body = "<h5>Bonjour,</h5><div>Vous trouverez ci-joint le bon de livraison N° " + BonLivraison.NumBon + "</div><div style='margin-top:50px;'>Cordialement,Cordialement,</div><div style='margin-top:100px;'>" + company.CompleteName + "<br/>" + company.AdresseSociete1 + "<br/>" + company.AdresseSociete2 + "<br/>" + company.AdresseSociete3 + "<br/>" + company.AdresseSociete4 + "<br/>Tel : " + company.Tel + "<br/>Tel/Fax : " + company.Fax + "<br/>E-mail : " + company.Adresse + "</div>";
                mailMessage.IsBodyHtml = true;

                Attachment attachment = new Attachment(stream, "BL_" + BonLivraison.NumBon.Replace("/", "-") + ".pdf");

                mailMessage.Attachments.Add(attachment);

                sc.Send(mailMessage);
                return (ActionResult)this.Json(new
                {
                    envoye = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return (ActionResult)this.Json(new
                {
                    envoye = 0,
                    error = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }

        }


        public ActionResult ExportReleveFacture(Guid IdClient,DateTime DateDebut,DateTime DateFin, bool Cachet = false)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();

            reportDocument.Load(
                Path.Combine(Server.MapPath("~/CrystalReports/" + upper + "/ReleveFacture" + upper + ".rpt")));


                reportDocument.SetDataSource(
                context.Paiements.Where(x => x.Client.Id == IdClient && x.Hide != true && x.Facture != null && 
                (DbFunctions.TruncateTime(x.Date) >= DateDebut.Date &&
                (DbFunctions.TruncateTime(x.Date) <= DateFin.Date)))
                   .Select(x => new
                   {
                       NumBon = x.Facture.NumBon,
                       Date = x.Date,
                       Client = x.Client.Name,
                       Debit = x.Debit,
                       Credit = x.Credit,
                       Adresse = x.Client.Adresse
                   }).OrderBy(x=>x.Date).ToList());
            reportDocument.SetParameterValue("Solde", new StatistiqueController().SoldeByClient(IdClient));
            reportDocument.SetParameterValue("Cachet", Cachet);


            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportEtatPaiement(Guid IdClient, DateTime DateDebut, DateTime DateFin, bool Cachet = false)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();

            reportDocument.Load(
                Path.Combine(Server.MapPath("~/CrystalReports/" + upper + "/EtatPaiement"+upper+".rpt")));


            reportDocument.SetDataSource(
            context.Paiements.Where(x => x.Client.Id == IdClient && x.Hide != true &&
            (DbFunctions.TruncateTime(x.Date) >= DateDebut.Date &&
            (DbFunctions.TruncateTime(x.Date) <= DateFin.Date)))
               .Select(x => new
               {
                   NumBon = x.Facture.NumBon,
                   Date = x.Date,
                   Client = x.Client.Name,
                   Debit = x.Debit,
                   Credit = x.Credit,
                   Commentaire = x.Comment,
                   Type = x.TypePaiement.Name,
                   Adresse = x.Client.Adresse
               }).OrderBy(x => x.Date).ToList());
            reportDocument.SetParameterValue("Solde", new StatistiqueController().SoldeByClient(IdClient));
            reportDocument.SetParameterValue("Cachet", Cachet);


            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult EmailReleveFacture(Guid IdClient, DateTime DateDebut, DateTime DateFin, bool Cachet = false)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();

            reportDocument.Load(
                Path.Combine(Server.MapPath("~/CrystalReports/" + upper + "/ReleveFacture" + upper + ".rpt")));


            reportDocument.SetDataSource(
            context.Paiements.Where(x => x.Client.Id == IdClient && x.Facture != null &&
            (DbFunctions.TruncateTime(x.Date) >= DateDebut.Date &&
            (DbFunctions.TruncateTime(x.Date) <= DateFin.Date)))
               .Select(x => new
               {
                   NumBon = x.Facture.NumBon,
                   Date = x.Date,
                   Client = x.Client.Name,
                   Debit = x.Debit,
                   Credit = x.Credit,
                   Adresse = x.Client.Adresse
               }).OrderBy(x => x.Date).ToList());
            reportDocument.SetParameterValue("Solde", new StatistiqueController().SoldeByClient(IdClient));
            reportDocument.SetParameterValue("Cachet", Cachet);


            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();

            MailMessage mailMessage = null;

            try
            {
                Company company = StatistiqueController.getCompany();

                Client Client= context.Paiements.Where(x => x.Client.Id == IdClient).FirstOrDefault().Client;
                SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
                sc.Credentials = new NetworkCredential(company.Adresse, company.CodeSecurite);
                sc.EnableSsl = true;
                mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(company.Adresse);
                if (Client.Email != null && Client.Email != "")
                {
                    mailMessage.To.Add(Client.Email);

                }
                else
                {
                    return (ActionResult)this.Json(new
                    {
                        envoye = -1
                    }, JsonRequestBehavior.AllowGet);
                }

                string datedebut_ = DateDebut.Date.Day+"/"+ DateDebut.Date.Month+1+"/"+DateDebut.Date.Year;
                string datefin_ = DateFin.Date.Day+"/"+ DateFin.Date.Month+1+"/"+ DateFin.Date.Year;
                mailMessage.Subject = "Relevé des factures ["+DateDebut.Date.Day+"]";
                mailMessage.Body = "<h3>Bonjour,</h3><div>Vous trouverez ci-joint votre relevé des factures</div><div style='margin-top:50px;'>Cordialement,</div><div style='margin-top:100px;'>" + company.CompleteName + "<br/>" + company.AdresseSociete1 + "<br/>" + company.AdresseSociete2 + "<br/>" + company.AdresseSociete3 + "<br/>" + company.AdresseSociete4 + "<br/>Tel : " + company.Tel + "<br/>Tel/Fax : " + company.Fax + "<br/>E-mail : " + company.Adresse + "</div>";
                mailMessage.IsBodyHtml = true;

                Attachment attachment = new Attachment(stream, "Relevés_factures_" + datedebut_.Replace("/", "-")+"_"+datefin_ + ".pdf");

                mailMessage.Attachments.Add(attachment);

                sc.Send(mailMessage);
                return (ActionResult)this.Json(new
                {
                    envoye = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return (ActionResult)this.Json(new
                {
                    envoye = 0
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }
        }

        public ActionResult ExportRdb(Guid IdRdb)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/RDB.rpt")));
            reportDocument.SetDataSource(
                this.context.RdbItems.Where(
                       (x => x.Rdb.Id == IdRdb))
                    .Select(x => new
                    {
                        NumBon = x.Rdb.NumBon,
                        Date = x.Rdb.Date,
                        Client = x.Rdb.Client.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        Adresse = x.Rdb.Client.Adresse,
                        Type = x.Rdb.Type
                    }).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportRdbF(Guid IdRdbF)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/RDBF.rpt")));
            reportDocument.SetDataSource(
                this.context.RdbFItems.Where(
                       (x => x.RdbF.Id == IdRdbF))
                    .Select(x => new
                    {
                        NumBon = x.RdbF.NumBon,
                        Date = x.RdbF.Date,
                        Client = x.RdbF.Fournisseur.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        Adresse = x.RdbF.Fournisseur.Adresse,
                        Type = x.RdbF.Type
                    }).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }


        public ActionResult ExportDgb(Guid IdDgb)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/DGB.rpt")));
            reportDocument.SetDataSource(
                this.context.DgbItems.Where(
                       (x => x.Dgb.Id == IdDgb))
                    .Select(x => new
                    {
                        NumBon = x.Dgb.NumBon,
                        Date = x.Dgb.Date,
                        Client = x.Dgb.Client.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        Adresse = x.Dgb.Client.Adresse,
                        PU = x.Pu,
                        ModeConsignation_DGB = x.Dgb.ModeConsignation,
                        CinRcn_DGB = x.Dgb.CinRcn,
                        TypeReglement_DGB = x.Dgb.TypeReglement,
                        NumCheque_DGB = x.Dgb.NumCheque,
                        DatePaiement_DGB = x.Dgb.DatePaiement,
                        Banque_DGB = x.Dgb.Banque
                    }).ToList());


            Decimal total  =
                (Decimal)
                this.context.DgbItems.Where(x => x.Dgb.Id == IdDgb)
                    .Sum(x => x.Qte*x.Pu);

            total = Decimal.Parse(String.Format("{0:.##}", total));

            (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
                   statistiqueController.DecimalToWords(total);

            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportMiniBonLivraisonArabic(Guid IdBonLivraison)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/MiniBonLivraisonArabic" + upper + ".rpt")));
            StatistiqueController statistiqueController = new StatistiqueController();
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.BonLivraisonItems.Where<BonLivraisonItem>(
                        (Expression<Func<BonLivraisonItem, bool>>) (x => x.BonLivraison.Id == IdBonLivraison))
                    .Select(x => new
                    {
                        NumBon = x.BonLivraison.NumBon,
                        Date = x.BonLivraison.Date,
                        Client = x.BonLivraison.Client.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.TotalHT,
                        Unite = x.Article.Unite
                    }).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportMiniBonLivraisonFrancais(Guid IdBonLivraison, bool Solde, bool IsArabic, bool? ShowPrices)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            if (Solde)
            {

                if (upper == "EAS")
                    reportDocument.Load(
                        Path.Combine(
                            this.Server.MapPath("~/CrystalReports/" + upper + "/MiniBonLivraisonSoldeFrancais" + upper +
                                                ".rpt")));
                else
                    reportDocument.Load(
                    Path.Combine(
                        this.Server.MapPath("~/CrystalReports/" + upper + "/MiniBonLivraisonFrancais" + upper +
                                            ".rpt")));
           
            }
            else
            {
               
               
                    reportDocument.Load(
                    Path.Combine(
                        this.Server.MapPath("~/CrystalReports/" + upper + "/MiniBonLivraisonFrancais" + upper + ".rpt")));
            }

            if (upper == "H9S" && IsArabic)
                reportDocument.Load(
                    Path.Combine(
                        this.Server.MapPath("~/CrystalReports/" + upper + "/MiniBonLivraisonArabic" + upper + ".rpt")));

            ////////////////////

           

            NumberFormatInfo numberFormatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            numberFormatInfo.NumberGroupSeparator = " ";
            if (Solde) { 
            (reportDocument.ReportDefinition.ReportObjects["solde"] as TextObject).Text =
                statistiqueController.SoldeByClient(
                    this.context.BonLivraisons.Where<BonLivraison>(
                            (Expression<Func<BonLivraison, bool>>)(x => x.Id == IdBonLivraison))
                        .Select<BonLivraison, Guid>((Expression<Func<BonLivraison, Guid>>)(x => x.IdClient))
                        .FirstOrDefault<Guid>()).ToString("#,#.00", (IFormatProvider)numberFormatInfo) + "DH";


            TextObject reportObject = reportDocument.ReportDefinition.ReportObjects["oldSolde"] as TextObject;
            if (
                this.context.BonLivraisons.Where<BonLivraison>(
                        (Expression<Func<BonLivraison, bool>>)(x => x.Id == IdBonLivraison))
                    .FirstOrDefault<BonLivraison>() != null)
            {
                float? oldSolde =
                    this.context.BonLivraisons.Where<BonLivraison>(
                            (Expression<Func<BonLivraison, bool>>)(x => x.Id == IdBonLivraison))
                        .FirstOrDefault<BonLivraison>()
                        .OldSolde;
                float num1 = 0.0f;
                if (((double)oldSolde.GetValueOrDefault() > (double)num1 ? (oldSolde.HasValue ? 1 : 0) : 0) != 0)
                {
                    oldSolde =
                        this.context.BonLivraisons.Where<BonLivraison>(
                                (Expression<Func<BonLivraison, bool>>)(x => x.Id == IdBonLivraison))
                            .FirstOrDefault<BonLivraison>()
                            .OldSolde;
                    float num2 = oldSolde.HasValue ? oldSolde.GetValueOrDefault() : 0.0f;
                    reportObject.Text = num2.ToString() ?? "NEANT";
                }
            }
            }

            ////////////////////////

            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.BonLivraisonItems.Where<BonLivraisonItem>(
                        (Expression<Func<BonLivraisonItem, bool>>) (x => x.BonLivraison.Id == IdBonLivraison))
                    .Select(x => new
                    {
                        NumBon = x.BonLivraison.NumBon,
                        Date = x.BonLivraison.Date,
                        Client = x.BonLivraison.Client.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.TotalHT,
                        CodeClient=x.BonLivraison.Client.Code,
                        Unite = x.Article.Unite,
                        Index = x.Index ?? 0
                    }).OrderByDescending(x => x.Index).ToList());

            if(upper == "H9S" || upper == "SANVIE" || upper == "TSR" || upper == "AQK" || upper == "SHMZ") 
                reportDocument.SetParameterValue("Solde", Solde);

            if (upper == "EAS")
            {
                reportDocument.SetParameterValue("ShowPrices", ShowPrices);
            }


            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportMiniBonReception(Guid IdBonReception)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string upper = StatistiqueController.getCompanyName().ToUpper();

            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/MiniBonReception" + upper + ".rpt")));
            NumberFormatInfo numberFormatInfo = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
            (reportDocument.ReportDefinition.ReportObjects["solde"] as TextObject).Text =
                statistiqueController.SoldeByFournisseur_(
                    this.context.BonReceptions.Where((x => x.Id == IdBonReception))
                        .Select((x => x.IdFournisseur))
                        .FirstOrDefault()).ToString("#,#.00", (IFormatProvider) numberFormatInfo) + "DH";

            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.BonReceptionItems.Where<BonReceptionItem>(
                        (Expression<Func<BonReceptionItem, bool>>) (x => x.BonReception.Id == IdBonReception))
                    .Select(x => new
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
                    }).OrderByDescending(x => x.Index).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportBonAvoirF(Guid IdBonAvoirF)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/BonAvoirF.rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.BonAvoirItems.Where<BonAvoirItem>(
                    (Expression<Func<BonAvoirItem, bool>>) (x => x.BonAvoir.Id == IdBonAvoirF)).Select(x => new
                {
                    NumBon = x.BonAvoir.NumBon,
                    Date = x.BonAvoir.Date,
                    Client = x.BonAvoir.Fournisseur.Name,
                    Ref = x.Article.Ref,
                    Designation = x.Article.Designation,
                    Qte = x.Qte,
                    PU = x.Pu,
                    TotalHT = x.TotalHT,
                    Unite = x.Article.Unite
                }).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }



        public ActionResult ExportBonAvoirC(Guid IdBonAvoirC,bool Cachet = false)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(Path.Combine(this.Server.MapPath("~/CrystalReports/"+upper+"/BonAvoirC" + upper + ".rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.BonAvoirCItems.Where(
                    (x => x.BonAvoirC.Id == IdBonAvoirC)).Select(x => new
                    {
                        NumBon = x.BonAvoirC.NumBon,
                        Date = x.BonAvoirC.Date,
                        CodeClient = x.BonAvoirC.Client.Code,
                        Client = x.BonAvoirC.Client.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.Qte*x.Pu,
                        Unite = x.Article.Unite,
                        Adresse = x.BonAvoirC.Client.Adresse,
                        NumBL = x.NumBL ?? "",
                        TVA = x.Article.TVA ?? 20
                    }).ToList());

            if (upper == "SBCIT" || upper == "SUIV")
                reportDocument.SetParameterValue("Cachet", Cachet);
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult EmailBonAvoirC(Guid IdBonAvoirC, bool Cachet = false)
        {
            ReportDocument reportDocument = new ReportDocument();
            StatistiqueController statistiqueController = new StatistiqueController();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/BonAvoirC" + upper + ".rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.BonAvoirCItems.Where(
                    (x => x.BonAvoirC.Id == IdBonAvoirC)).Select(x => new
                    {
                        NumBon = x.BonAvoirC.NumBon,
                        Date = x.BonAvoirC.Date,
                        CodeClient = x.BonAvoirC.Client.Code,
                        Client = x.BonAvoirC.Client.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.Qte * x.Pu,
                        Unite = x.Article.Unite,
                        Adresse = x.BonAvoirC.Client.Adresse,
                        NumBL = x.NumBL ?? "",
                        TVA = x.Article.TVA ?? 20
                    }).ToList());

            if (upper == "SBCIT")
                reportDocument.SetParameterValue("Cachet", Cachet);
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();




            MailMessage mailMessage = null;

            try
            {
                Company company = StatistiqueController.getCompany();

                BonAvoirC BonAvoirC = context.BonAvoirCs.Where(x => x.Id == IdBonAvoirC).FirstOrDefault();
                SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
                sc.Credentials = new NetworkCredential(company.Adresse, company.CodeSecurite);
                sc.EnableSsl = true;
                mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(company.Adresse);
                if (BonAvoirC.Client.Email != null && BonAvoirC.Client.Email != "")
                {
                    mailMessage.To.Add(BonAvoirC.Client.Email);

                }
                else
                {
                    return (ActionResult)this.Json(new
                    {
                        envoye = -1
                    }, JsonRequestBehavior.AllowGet);
                }
                mailMessage.Subject = "BonAvoirC N° " + BonAvoirC.NumBon;
                mailMessage.Body = "<h3>Bonjour,</h3><div>Vous trouverez ci-joint le bon d'avoir N° " + BonAvoirC.NumBon + "</div><div style='margin-top:50px;'>Cordialement,</div><div style='margin-top:100px;'>" + company.CompleteName + "<br/>" + company.AdresseSociete1 + "<br/>" + company.AdresseSociete2 + "<br/>" + company.AdresseSociete3 + "<br/>" + company.AdresseSociete4 + "<br/>Tel : " + company.Tel + "<br/>Tel/Fax : " + company.Fax + "<br/>E-mail : " + company.Adresse + "</div>";
                mailMessage.IsBodyHtml = true;

                Attachment attachment = new Attachment(stream, "bon_avoir_" + BonAvoirC.NumBon.Replace("/", "-") + ".pdf");

                mailMessage.Attachments.Add(attachment);

                sc.Send(mailMessage);
                return (ActionResult)this.Json(new
                {
                    envoye = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return (ActionResult)this.Json(new
                {
                    envoye = 0
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }

        }



        public ActionResult ExportArticles()
        {
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(Path.Combine(this.Server.MapPath("~/CrystalReports/CrystalReportStock.rpt")));
            reportDocument.SetDataSource((IEnumerable) this.context.Articles.Where(x=>x.QteStock != 0).Select(x => new
            {
                Ref = x.Ref,
                Designation = x.Designation.ToUpper(),
                Qte = x.QteStock,
                Unite = x.Unite,
                BarCode = x.BarCode
            }).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportArticlesGaz(DateTime DateDebut,DateTime DateFin)
        {
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(Path.Combine(Server.MapPath("~/CrystalReports/SUIV/EtatStockArticlesGaz.rpt")));


            reportDocument.SetDataSource(context.Articles.Where(x=>x.Ref.StartsWith("GZ")).Select(x => new
            {
                Designation = x.Designation,
                QtePleineFinale = x.QteEmballagePleine ?? 0,
                QteVideFinale = x.QteEmballageVide ?? 0,
                QtePleineLivreeF = x.DgbFItems.Where(y => DbFunctions.TruncateTime(y.DgbF.Date) >= DbFunctions.TruncateTime(DateDebut) && DbFunctions.TruncateTime(y.DgbF.Date) <= DbFunctions.TruncateTime(DateFin)).Sum(y=> (int?)y.Qte) ?? 0,
                QteVideRecueF = x.RdbFItems.Where(y => DbFunctions.TruncateTime(y.RdbF.Date) >= DbFunctions.TruncateTime(DateDebut) && DbFunctions.TruncateTime(y.RdbF.Date) <= DbFunctions.TruncateTime(DateFin)).Sum(y=> (int?)y.Qte) ?? 0,
                QtePleineLivreeC = x.DgbItems.Where(y => DbFunctions.TruncateTime(y.Dgb.Date) >= DbFunctions.TruncateTime(DateDebut) && DbFunctions.TruncateTime(y.Dgb.Date) <= DbFunctions.TruncateTime(DateFin)).Sum(y=> (int?)y.Qte) ?? 0,
                QteVideRecueC = x.RdbItems.Where(y => DbFunctions.TruncateTime(y.Rdb.Date) >= DbFunctions.TruncateTime(DateDebut) && DbFunctions.TruncateTime(y.Rdb.Date) <= DbFunctions.TruncateTime(DateFin)).Sum(y=>(int?)y.Qte) ?? 0
                

            }).OrderByDescending(x => x.Designation).ToList());


            reportDocument.SetParameterValue("DateDebut", DateDebut);
            reportDocument.SetParameterValue("DateFin", DateFin);


            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportBonCommande(Guid IdBonCommande, bool? Chiffre = false, bool Cachet = false)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();

            if (Chiffre == true)
            {
                reportDocument.Load(
            Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/BonCommandeChiffre" + upper + ".rpt")));
            }
            else
            {
                    reportDocument.Load(
                    Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/BonCommande" + upper + ".rpt")));
               
            }
            
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.BonCommandeItems.Where(x => x.BonCommande.Id == IdBonCommande).Select(x => new
                {
                        NumBon = x.BonCommande.NumBon,
                        Date = x.BonCommande.Date,
                        Client = x.BonCommande.Fournisseur.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.TotalHT,
                        TVA = x.Article.TVA ?? 20,
                        Unite = x.Article.Unite,
                        Adresse = x.BonCommande.Fournisseur.Adresse,
                    }).ToList());

            if (upper == "SBCIT" || upper == "SUIV")
            {
                reportDocument.SetParameterValue("Cachet", Cachet);
            }
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportTarif(Guid IdTarif)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/Tarif.rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.TarifItems.Where<TarifItem>(
                    (x => x.Tarif.Id == IdTarif)).Select(x => new
                    {
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Date = x.Tarif.Date,
                        Pu = x.Pu,
                        Pa = x.Article.PA,
                        Unite = x.Article.Unite,
                        Titre = x.Tarif.Ref,
                        Qte = x.Article.QteStock
                    }).OrderBy(x=>x.Designation).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportAllTarif()
        {
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/Tarif.rpt")));
            reportDocument.SetDataSource(
                this.context.Articles.Select(x => new
                    {
                        Ref = x.Ref,
                        Designation = x.Designation,
                        Date = DateTime.Now,
                        Pu = x.PVD ?? 0,
                        Pa = x.PA,
                        Unite = x.Unite,
                        Qte = x.QteStock,
                        Titre = DateTime.Now.Year.ToString()
                    }).OrderBy(x => x.Designation).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }


        public ActionResult ExportSituationGlobalClients()
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/SituationClients.rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
               context.Paiements.Where<Paiement>((x => x.IdClient != new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"))).GroupBy(q => new
               {
                   IdClient = q.IdClient,
                   Name = q.Client.Name
               }).Select(x => new
               {
                   Client = x.Key.Name,
                   Solde = x.Sum<Paiement>((y => y.Debit)) - x.Sum<Paiement>((y => y.Credit))
               }).OrderByDescending(x => x.Solde).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportEtatBonLivraisons(DateTime Date)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/"+ upper + "/EtatBonLivraisons"+upper+".rpt")));

            reportDocument.SetDataSource(
              this.context.BonLivraisons.Where(x => x.Date.Month == Date.Month && x.Date.Year == Date.Year)
                .Select(x => new
                {
                    Date = x.Date,
                    NumBon = x.NumBon,
                    Debit = context.BonLivraisonItems.Where(y => y.IdBonLivraison == x.Id).Count() > 0 ? context.BonLivraisonItems.Where(y=>y.IdBonLivraison == x.Id).Sum(y => y.Pu * y.Qte) : 0,
                    Client = (this.context.FactureItems.Where(f => f.NumBL.Contains(x.NumBon)).FirstOrDefault() != null) ? "FA " + this.context.FactureItems.Where(f => f.NumBL.Contains(x.NumBon)).FirstOrDefault().Facture.NumBon : "NF",
                    Adresse = x.Client.Name.ToUpper()
                }).OrderBy(x => x.Client).ThenBy(x=>x.Date).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }

        public ActionResult ExportSituationGlobalFournisseurs()
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/SituationFournisseurs.rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
               context.PaiementFs.Where<PaiementF>((x => x.IdFournisseur != new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"))).GroupBy(q => new
               {
                   IdFournisseur = q.IdFournisseur,
                   Name = q.Fournisseur.Name
               }).Select(x => new
               {
                   Client = x.Key.Name,
                   Solde = x.Sum<PaiementF>((y => y.Debit)) - x.Sum<PaiementF>((y => y.Credit))
               }).OrderByDescending(x => x.Solde).ToList());
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }


        public ActionResult ExportDevis(Guid IdDevis,bool? Chiffre,bool Cachet = false)
        {
            StatistiqueController statistiqueController = new StatistiqueController();
            Dictionary<double, double> source = statistiqueController.totalWithTVADevis(IdDevis);
            double num1 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 0.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num2 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 7.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num3 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 10.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num4 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 14.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            double num5 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>) (x => x.Key == 20.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>) (x => x.Value))
                    .FirstOrDefault<double>();
            Decimal number =
                (Decimal)
                this.context.DevisItems.Where<DevisItem>(
                        (Expression<Func<DevisItem, bool>>) (x => x.Devis.Id == IdDevis))
                    .Sum<DevisItem>((Expression<Func<DevisItem, float>>) (x => x.TotalHT));
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();

            if (Chiffre == true)
            {
                reportDocument.Load(
            Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/DevisChiffre" + upper + ".rpt")));
            }else
            {
                if(upper == "SUIV" || upper == "SBCIT" || upper == "EAS" || upper == "TSR" || upper == "AQK" || upper == "SHMZ")
                reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/Devis" + upper + ".rpt")));
                else
                    reportDocument.Load(
            Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/DevisChiffre" + upper + ".rpt")));
            }

           

            
            if(upper == "SUIV" || upper == "SBCIT")
            {
                reportDocument.SetDataSource(
               this.context.DevisItems.Where(
                   (x => x.Devis.Id == IdDevis)).Select(x => new
                   {
                       NumBon = x.Devis.NumBon,
                       Date = x.Devis.Date,
                       Client = x.Devis.Client.Name,
                       Ref = x.Article.Ref,
                       Designation = x.Article.Designation,
                       Qte = x.Qte,
                       PU = x.Pu,
                       TotalHT = x.TotalHT,
                       TVA = x.Article.TVA ?? 20,
                       Unite = x.Article.Unite,
                       Adresse = x.Devis.Client.Adresse,
                       DelaiLivraison = x.Devis.DelaiLivrasion,
                       TransportExpedition = x.Devis.TransportExpedition,
                       TypeReglement = x.Devis.TypeReglement,
                       ValiditeOffre = x.Devis.ValiditeOffre
                   }).ToList());

                reportDocument.SetParameterValue("Cachet", Cachet);


            }
            else
            {
                reportDocument.SetDataSource(
               this.context.DevisItems.Where(
                   (x => x.Devis.Id == IdDevis)).Select(x => new
                   {
                       NumBon = x.Devis.NumBon,
                       Date = x.Devis.Date,
                       Client = x.Devis.Client.Name,
                       Ref = x.Article.Ref,
                       Designation = x.Article.Designation,
                       Qte = x.Qte,
                       PU = x.Pu,
                       TotalHT = x.TotalHT,
                       TVA = x.Article.TVA ?? 20,
                       Unite = x.Article.Unite,
                   }).ToList());
            }
           
            if (upper == "SOLY")
            {
                TextObject reportObject1 = reportDocument.ReportDefinition.ReportObjects["t0"] as TextObject;
                double num6 = num1 / 1.0;
                string str1 = num6.ToString("F");
                reportObject1.Text = str1;
                TextObject reportObject2 = reportDocument.ReportDefinition.ReportObjects["t7"] as TextObject;
                num6 = num2 / 1.07;
                string str2 = num6.ToString("F");
                reportObject2.Text = str2;
                TextObject reportObject3 = reportDocument.ReportDefinition.ReportObjects["t10"] as TextObject;
                num6 = num3 / 1.1;
                string str3 = num6.ToString("F");
                reportObject3.Text = str3;
                TextObject reportObject4 = reportDocument.ReportDefinition.ReportObjects["t14"] as TextObject;
                num6 = num4 / 1.14;
                string str4 = num6.ToString("F");
                reportObject4.Text = str4;
                TextObject reportObject5 = reportDocument.ReportDefinition.ReportObjects["t20"] as TextObject;
                num6 = num5 / 1.2;
                string str5 = num6.ToString("F");
                reportObject5.Text = str5;
                TextObject reportObject6 = reportDocument.ReportDefinition.ReportObjects["total0"] as TextObject;
                num6 = num1 - num1 / 1.0;
                string str6 = num6.ToString("F");
                reportObject6.Text = str6;
                TextObject reportObject7 = reportDocument.ReportDefinition.ReportObjects["total7"] as TextObject;
                num6 = num2 - num2 / 1.07;
                string str7 = num6.ToString("F");
                reportObject7.Text = str7;
                TextObject reportObject8 = reportDocument.ReportDefinition.ReportObjects["total10"] as TextObject;
                num6 = num3 - num3 / 1.1;
                string str8 = num6.ToString("F");
                reportObject8.Text = str8;
                (reportDocument.ReportDefinition.ReportObjects["total14"] as TextObject).Text =
                    (num4 - num4 / 1.14).ToString("F");
                (reportDocument.ReportDefinition.ReportObjects["total20"] as TextObject).Text =
                    (num5 - num5 / 1.2).ToString("F");

                number = Decimal.Parse(String.Format("{0:.##}", number));

                (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
                    statistiqueController.DecimalToWords(number);
            }

            if(upper == "AQK" || upper == "SHMZ")
            {
                Decimal number2 =
              (Decimal)
              this.context.DevisItems.Where(
                      (x => x.Devis.Id == IdDevis))
                  .Sum(x => x.Qte * (x.Pu + (x.Pu * (x.Article.TVA ?? 20) / 100)));
                number2 = Decimal.Parse(String.Format("{0:.##}", number2));


                string totalmots = statistiqueController.DecimalToWords(number2); ;

                (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
              totalmots;
            }
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();
            return File(stream, "application/pdf");
        }



        public ActionResult EmailDevis(Guid IdDevis,bool Chiffre= false, bool Cachet = false)
        {
            StatistiqueController statistiqueController = new StatistiqueController();
            Dictionary<double, double> source = statistiqueController.totalWithTVADevis(IdDevis);
            double num1 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 0.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num2 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 7.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num3 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 10.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num4 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 14.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            double num5 =
                source.Where<KeyValuePair<double, double>>(
                        (Func<KeyValuePair<double, double>, bool>)(x => x.Key == 20.0))
                    .Select<KeyValuePair<double, double>, double>(
                        (Func<KeyValuePair<double, double>, double>)(x => x.Value))
                    .FirstOrDefault<double>();
            Decimal number =
                (Decimal)
                this.context.DevisItems.Where<DevisItem>(
                        (Expression<Func<DevisItem, bool>>)(x => x.Devis.Id == IdDevis))
                    .Sum<DevisItem>((Expression<Func<DevisItem, float>>)(x => x.TotalHT));
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();

            if (Chiffre == true)
            {
                reportDocument.Load(
            Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/DevisChiffre" + upper + ".rpt")));
            }
            else
            {
                if (upper == "SUIV" || upper == "SBCIT" || upper == "EAS" || upper == "TSR" || upper == "AQK" || upper == "SHMZ")
                    reportDocument.Load(
                    Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/Devis" + upper + ".rpt")));
                else
                    reportDocument.Load(
            Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/DevisChiffre" + upper + ".rpt")));
            }




            if (upper == "SUIV" || upper == "SBCIT")
            {
                reportDocument.SetDataSource(
               this.context.DevisItems.Where(
                   (x => x.Devis.Id == IdDevis)).Select(x => new
                   {
                       NumBon = x.Devis.NumBon,
                       Date = x.Devis.Date,
                       Client = x.Devis.Client.Name,
                       Ref = x.Article.Ref,
                       Designation = x.Article.Designation,
                       Qte = x.Qte,
                       PU = x.Pu,
                       TotalHT = x.TotalHT,
                       TVA = x.Article.TVA ?? 20,
                       Unite = x.Article.Unite,
                       Adresse = x.Devis.Client.Adresse,
                       DelaiLivraison = x.Devis.DelaiLivrasion,
                       TransportExpedition = x.Devis.TransportExpedition,
                       TypeReglement = x.Devis.TypeReglement,
                       ValiditeOffre = x.Devis.ValiditeOffre
                   }).ToList());

                reportDocument.SetParameterValue("Cachet", Cachet);


            }
            else
            {
                reportDocument.SetDataSource(
               this.context.DevisItems.Where(
                   (x => x.Devis.Id == IdDevis)).Select(x => new
                   {
                       NumBon = x.Devis.NumBon,
                       Date = x.Devis.Date,
                       Client = x.Devis.Client.Name,
                       Ref = x.Article.Ref,
                       Designation = x.Article.Designation,
                       Qte = x.Qte,
                       PU = x.Pu,
                       TotalHT = x.TotalHT,
                       TVA = x.Article.TVA ?? 20,
                       Unite = x.Article.Unite,
                   }).ToList());
            }

            if (upper == "SOLY")
            {
                TextObject reportObject1 = reportDocument.ReportDefinition.ReportObjects["t0"] as TextObject;
                double num6 = num1 / 1.0;
                string str1 = num6.ToString("F");
                reportObject1.Text = str1;
                TextObject reportObject2 = reportDocument.ReportDefinition.ReportObjects["t7"] as TextObject;
                num6 = num2 / 1.07;
                string str2 = num6.ToString("F");
                reportObject2.Text = str2;
                TextObject reportObject3 = reportDocument.ReportDefinition.ReportObjects["t10"] as TextObject;
                num6 = num3 / 1.1;
                string str3 = num6.ToString("F");
                reportObject3.Text = str3;
                TextObject reportObject4 = reportDocument.ReportDefinition.ReportObjects["t14"] as TextObject;
                num6 = num4 / 1.14;
                string str4 = num6.ToString("F");
                reportObject4.Text = str4;
                TextObject reportObject5 = reportDocument.ReportDefinition.ReportObjects["t20"] as TextObject;
                num6 = num5 / 1.2;
                string str5 = num6.ToString("F");
                reportObject5.Text = str5;
                TextObject reportObject6 = reportDocument.ReportDefinition.ReportObjects["total0"] as TextObject;
                num6 = num1 - num1 / 1.0;
                string str6 = num6.ToString("F");
                reportObject6.Text = str6;
                TextObject reportObject7 = reportDocument.ReportDefinition.ReportObjects["total7"] as TextObject;
                num6 = num2 - num2 / 1.07;
                string str7 = num6.ToString("F");
                reportObject7.Text = str7;
                TextObject reportObject8 = reportDocument.ReportDefinition.ReportObjects["total10"] as TextObject;
                num6 = num3 - num3 / 1.1;
                string str8 = num6.ToString("F");
                reportObject8.Text = str8;
                (reportDocument.ReportDefinition.ReportObjects["total14"] as TextObject).Text =
                    (num4 - num4 / 1.14).ToString("F");
                (reportDocument.ReportDefinition.ReportObjects["total20"] as TextObject).Text =
                    (num5 - num5 / 1.2).ToString("F");

                number = Decimal.Parse(String.Format("{0:.##}", number));

                (reportDocument.ReportDefinition.ReportObjects["totalMots"] as TextObject).Text =
                    statistiqueController.DecimalToWords(number);
            }
            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();





            MailMessage mailMessage = null;

            try
            {
                Company company = StatistiqueController.getCompany();

                Devis Devis = context.Devises.Where(x => x.Id == IdDevis).FirstOrDefault();
                SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
                sc.Credentials = new NetworkCredential(company.Adresse, company.CodeSecurite);
                sc.EnableSsl = true;
                mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(company.Adresse);
                if(Devis.Client.Email != null && Devis.Client.Email != "")
                {
                    mailMessage.To.Add(Devis.Client.Email);

                }else
                {
                    return (ActionResult)this.Json(new
                    {
                        envoye = -1
                    }, JsonRequestBehavior.AllowGet);
                }
                mailMessage.Subject = "Devis N° "+Devis.NumBon;
                mailMessage.Body = "<h3>Bonjour,</h3><div>Vous trouverez ci-joint le devis N° "+Devis.NumBon+"</div><div style='margin-top:50px;'>Cordialement,</div><div style='margin-top:100px;'>"+company.CompleteName+"<br/>"+company.AdresseSociete1+"<br/>"+company.AdresseSociete2+"<br/>"+company.AdresseSociete3+"<br/>"+company.AdresseSociete4+"<br/>Tel : "+company.Tel+"<br/>Tel/Fax : "+company.Fax+"<br/>E-mail : "+company.Adresse+"</div>";
                mailMessage.IsBodyHtml = true;

                Attachment attachment = new Attachment(stream, "devis_"+Devis.NumBon.Replace("/","-")+".pdf");

                mailMessage.Attachments.Add(attachment);

                sc.Send(mailMessage);
                return (ActionResult)this.Json(new
                {
                    envoye = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return (ActionResult)this.Json(new
                {
                    envoye = 0,
                    error = ex.Message

                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }


        }


        public ActionResult EmailBonCommande(Guid IdBonCommande, bool Cachet = false)
        {
            ReportDocument reportDocument = new ReportDocument();
            string upper = StatistiqueController.getCompanyName().ToUpper();
            reportDocument.Load(
                Path.Combine(this.Server.MapPath("~/CrystalReports/" + upper + "/BonCommande" + upper + ".rpt")));
            reportDocument.SetDataSource(
                (IEnumerable)
                this.context.BonCommandeItems.Where<BonCommandeItem>(
                    (Expression<Func<BonCommandeItem, bool>>)(x => x.BonCommande.Id == IdBonCommande)).Select(x => new
                    {
                        NumBon = x.BonCommande.NumBon,
                        Date = x.BonCommande.Date,
                        Client = x.BonCommande.Fournisseur.Name,
                        Ref = x.Article.Ref,
                        Designation = x.Article.Designation,
                        Qte = x.Qte,
                        PU = x.Pu,
                        TotalHT = x.TotalHT,
                        TVA = x.Article.TVA ?? 20,
                        Unite = x.Article.Unite,
                        Adresse = x.BonCommande.Fournisseur.Adresse,
                    }).ToList());

            if(upper == "SBCIT")
            {
                reportDocument.SetParameterValue("Cachet", Cachet);
            }

            this.Response.Buffer = false;
            this.Response.ClearContent();
            this.Response.ClearHeaders();
            Stream stream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0L, SeekOrigin.Begin);
            reportDocument.Close();




            MailMessage mailMessage = null;

            try
            {
                Company company = StatistiqueController.getCompany();

                BonCommande BonCommande = context.BonCommandes.Where(x => x.Id == IdBonCommande).FirstOrDefault();
                SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
                sc.Credentials = new NetworkCredential(company.Adresse, company.CodeSecurite);
                sc.EnableSsl = true;
                mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(company.Adresse);
                if (BonCommande.Fournisseur.Email != null && BonCommande.Fournisseur.Email != "")
                {
                    mailMessage.To.Add(BonCommande.Fournisseur.Email);

                }
                else
                {
                    return (ActionResult)this.Json(new
                    {
                        envoye = -1
                    }, JsonRequestBehavior.AllowGet);
                }
                mailMessage.Subject = "Bon de commande N° " + BonCommande.NumBon;
                mailMessage.Body = "<h3>Bonjour,</h3><div>Vous trouverez ci-joint le Bon de Commande N° " + BonCommande.NumBon + "</div><div style='margin-top:50px;'>Cordialement,</div><div style='margin-top:100px;'>" + company.CompleteName + "<br/>" + company.AdresseSociete1 + "<br/>" + company.AdresseSociete2 + "<br/>" + company.AdresseSociete3 + "<br/>" + company.AdresseSociete4 + "<br/>Tel : " + company.Tel + "<br/>Tel/Fax : " + company.Fax + "<br/>E-mail : " + company.Adresse + "</div>";
                mailMessage.IsBodyHtml = true;

                Attachment attachment = new Attachment(stream, "BonCommande_" + BonCommande.NumBon.Replace("/", "-") + ".pdf");

                mailMessage.Attachments.Add(attachment);

                sc.Send(mailMessage);
                return (ActionResult)this.Json(new
                {
                    envoye = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return (ActionResult)this.Json(new
                {
                    envoye = 0
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }


        }
        public ActionResult SituationGlobalClient()
        {
            return View();
        }

        public ActionResult SituationGlobalFournisseur()
        {
            return View();
        }

        public ActionResult PopUpPrintTarif()
        {
            return View();
        }

        public ActionResult PopUpPrintRDB()
        {
            return View();
        }

        public ActionResult Tarif()
        {
            return View();
        }
        public ActionResult SituationParDate()
        {
            return View();
        }
        public ActionResult DGB()
        {
            return View();
        }

        public ActionResult ListDGB()
        {
            return View();
        }

        public ActionResult PopUpPrintDGB()
        {
            return View();
        }

        public ActionResult DGBF()
        {
            return View();
        }

        public ActionResult ListDGBF()
        {
            return View();
        }

        public ActionResult PopUpPrintDGBF()
        {
            return View();
        }

        public ActionResult RDBF()
        {
            return View();
        }

        public ActionResult ListRDBF()
        {
            return View();
        }

        public ActionResult PopUpPrintRDBF()
        {
            return View();
        }

        public ActionResult Depense()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult TestNewUiGrid()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult ProductInAlertStatus()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult ProduitsMoinsVendus()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult Reglage()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult Settings()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult MargeParBL()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult TypeDepense()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult ChequeNonEnCaisse()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult ChequeNonEnCaisseF()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult SuiviProduitClients()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult SuiviProduitFournisseurs()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult Fournisseur()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult Client()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult MargeBeneficiaire()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult BonReception()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult Facture()
        {
            return View();
        }

        public ActionResult BonAvoir()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult BonAvoirC()
        {
            return View();
        }

        public ActionResult RDB()
        {
            return View();
        }

        public ActionResult PopUpPeriod()
        {
            return View();
        }

        public ActionResult BouteillesFournisseurs()
        {
            return View();
        }

        public ActionResult BouteillesClients()
        {
            return View();
        }


        public ActionResult PopUpBonLivraison()
        {
            return View();
        }

        public ActionResult PopUpBonReception()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult PopUpDevis()
        {
            return View();
        }

        public ActionResult PopUpBonCommande()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult BonCommande()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult CompteClient()
        {
            return View();
        }

        public ActionResult CompteFournisseur()
        {
            return View();
        }

        public ActionResult BonLivraison()
        {
            return View();
        }

        public ActionResult Devis()
        {
            return View();
        }

        public ActionResult ArticleFacture()
        {
            return View();
        }

        public ActionResult FakeFacture()
        {
            return View();
        }

        public ActionResult FactureFournisseur()
        {
            return View();
        }

        public ActionResult ListFakeFacture()
        {
            return View();
        }

        public ActionResult ListFactureAchat()
        {
            return View();
        }

        public ActionResult PopUpPrintFakeFacture()
        {
            return View();
        }

        public ActionResult PopUpPrintFactureAchat()
        {
            return View();
        }

        public ActionResult Famille()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult PopUpArticle()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult PopUpHistoryArticleVente()
        {
            return View();
        }

        public ActionResult PopUpHistoryArticleAchat()
        {
            return View();
        }

        public ActionResult PopUpPrintBonLivraison()
        {
            return View();
        }

        public ActionResult PopUpPrintBonAvoir()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult PopUpPrintFacture()
        {
            return View();
        }

        public ActionResult PopUpPrintBonAvoirC()
        {
            return View();
        }

        public ActionResult PopUpPrintBonReception()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult PopUpPrintDevis()
        {
            return View();
        }

        public ActionResult Revendeur()
        {
            return View();
        }

        public ActionResult PopUpPrintBonCommande()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult PopUpFournisseur()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult PopUpClient()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult ListBonReception()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult ListBonLivraison()
        {
            return View();
        }

        public ActionResult ListFacture()
        {
            return View();
        }

        public ActionResult ListTarif()
        {
            return View();
        }

        public ActionResult ListBonAvoirC()
        {
            return View();
        }

        public ActionResult ListDevis()
        {
            return View();
        }

        public ActionResult ListRDB()
        {
            return View();
        }

        public ActionResult ListBonAvoir()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult ListBonCommande()
        {
            if (this.User.Identity.Name.Contains("cmp"))
                return View("BonLivraison");
            return View();
        }

        public ActionResult Article()
        {
            return View();
        }
    }
}
