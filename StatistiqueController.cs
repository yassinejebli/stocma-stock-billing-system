// Decompiled with JetBrains decompiler
// Type: WebApplication1.StatistiqueController
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using WebApplication1.DATA;

namespace WebApplication1
{
    public class StatistiqueController : Controller
    {
        private MySaniSoftContext context = new MySaniSoftContext();

        public static bool afficherNumBL()
        {
            List<Setting> list = new MySaniSoftContext().Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "1")).ToList<Setting>();
            return list.Count > 0 && list[0].Afficher == 1;
        }

        public void updatePaiement(Guid IdBonLivraison)
        {
            /*IQueryable<BonLivraisonItem> source = this.context.BonLivraisonItems.Where<BonLivraisonItem>((x => x.IdBonLivraison == IdBonLivraison));
            if (source.ToList<BonLivraisonItem>().Count <= 0)
                return;
            float num = source.Sum<BonLivraisonItem>((Expression<Func<BonLivraisonItem, float>>)(x => x.Qte * x.Pu));
            this.context.Paiements.Where<Paiement>((x => x.BonLivraison.Id == IdBonLivraison)).FirstOrDefault<Paiement>().Debit = num;
            this.context.SaveChanges();
            */


            float somme = 0;
            BonLivraison bl = context.BonLivraisons.Where(x => x.Id == IdBonLivraison).FirstOrDefault();

            if (context.BonLivraisonItems.Where(x => x.IdBonLivraison == IdBonLivraison).Count() > 0)
            {
                    somme = context.BonLivraisonItems.Where(x => x.IdBonLivraison == IdBonLivraison).Sum(x => x.Qte * x.Pu);
            }

            if (this.context.Paiements.Where<Paiement>((x => x.BonLivraison.Id == IdBonLivraison)).FirstOrDefault<Paiement>() != null) {
                this.context.Paiements.Where<Paiement>((x => x.BonLivraison.Id == IdBonLivraison)).FirstOrDefault<Paiement>().Debit = somme;
            }
            else
            {
                Paiement paiement = new Paiement(){ Id = Guid.NewGuid(), IdBonLivraison = IdBonLivraison, IdClient = bl.IdClient, Debit = somme, IdTypePaiement = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb6"), Date = bl.Date };
                this.context.Paiements.Add(paiement);

            }

            if(bl.IdClient == new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"))
            {
                Paiement paiementClient = context.Paiements.Where(x => x.Comment == "BL " + bl.NumBon && x.IdClient == new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39") && x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2")).FirstOrDefault();
                if(paiementClient != null)
                {
                    paiementClient.Credit = bl.BonLivraisonItems.Sum(x => x.Pu * x.Qte);
                }
            }


            this.context.SaveChanges();
        }


        public void updatePaiementAvoirC(Guid IdBonAvoirC)
        {
           

            float somme = 0;
            BonAvoirC bl = context.BonAvoirCs.Where(x => x.Id == IdBonAvoirC).FirstOrDefault();

            if (context.BonAvoirCItems.Where(x => x.IdBonAvoirC == IdBonAvoirC).Count() > 0)
            {

                if (getCompanyName() == "SUIV" || getCompanyName() == "SBCIT")
                    somme = context.BonAvoirCItems.Where(x => x.IdBonAvoirC == IdBonAvoirC).Sum(x => x.Qte * (x.Pu + (x.Pu * (x.Article.TVA ?? 20) / 100)));
                else
                    somme = context.BonAvoirCItems.Where(x => x.IdBonAvoirC == IdBonAvoirC).Sum(x => x.Qte * x.Pu);


            }

            if (this.context.Paiements.Where<Paiement>((x => x.Comment == "Avoir N° : "+bl.NumBon)).FirstOrDefault<Paiement>() != null)
            {
                this.context.Paiements.Where<Paiement>((x => x.Comment == "Avoir N° : " + bl.NumBon)).FirstOrDefault<Paiement>().Credit = somme;
            }
            else
            {
                Paiement paiement = new Paiement() { Id = Guid.NewGuid(), Comment = "Avoir N° : " + bl.NumBon, IdClient = bl.IdClient, Debit = 0,Credit = somme, IdTypePaiement = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb8"), Date = bl.Date };
                this.context.Paiements.Add(paiement);

            }

            if (bl.IdClient == new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"))
            {
                Paiement paiementClient = context.Paiements.Where(x => x.Comment == "Avoir N° : " + bl.NumBon && x.IdClient == new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39") && x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4")).FirstOrDefault();
                if (paiementClient != null)
                {
                    if (getCompanyName() == "SUIV" || getCompanyName() == "SBCIT")
                        paiementClient.Debit = bl.BonAvoirCItems.Sum(x => x.Qte * (x.Pu + (x.Pu * (x.Article.TVA ?? 20) / 100)));
                    else
                        paiementClient.Debit = bl.BonAvoirCItems.Sum(x => x.Pu * x.Qte);

                }
            }


            this.context.SaveChanges();
        }

        public void updatePaiementFacture(Guid IdFacture)
        {
            var source = context.FactureItems.Where((x => x.IdFacture == IdFacture));
            Facture fa = context.Factures.Where(x => x.Id == IdFacture).FirstOrDefault();

            if (source.ToList().Count <= 0)
                return;

            float number =
              this.context.FactureItems.Where<FactureItem>(
                        (Expression<Func<FactureItem, bool>>)(x => x.Facture.Id == IdFacture))
                    .Sum<FactureItem>((Expression<Func<FactureItem, float>>)(x => x.Qte * x.Pu));
            float num =
              this.context.FactureItems.Where<FactureItem>(
                        (Expression<Func<FactureItem, bool>>)(x => x.Facture.Id == IdFacture))
                    .Sum(x => x.Qte * (x.Pu + (x.Pu * (x.Article.TVA ?? 20) / 100)));

            if (fa.TypeReglement == "Au-Comptant")
            {
                float drt = number * 0.0025f;//droit timbre if espece
                num += drt;
            }

            if (this.context.Paiements.Where((x => x.Facture.Id == IdFacture)).FirstOrDefault() != null) { 
                context.Paiements.Where((x => x.Facture.Id == IdFacture)).FirstOrDefault().Debit = num;
            }
            else
            {
                Paiement paiement = new Paiement() { Id = Guid.NewGuid(), IdFacture = IdFacture, IdClient = fa.IdClient, Debit = num, IdTypePaiement = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb6"), Date = fa.Date };
                this.context.Paiements.Add(paiement);
            }

            this.context.SaveChanges();
        }

        public void updatePaiementFactureFournisseur(Guid IdFactureF)
        {
            if (getCompanyName() == "SUIV" || getCompanyName() == "SBCIT")
                return;

            float somme = 0;

            var BonReceptionsAllItems = context.BonReceptions.Where(x => x.IdFactureF == IdFactureF).SelectMany(x => x.BonReceptionItems).ToList();
            if (BonReceptionsAllItems.Count() > 0)
            {
                somme = BonReceptionsAllItems.Sum(x => x.Qte * (x.Pu + (x.Pu * (x.Article.TVA ?? 20) / 100)));
            }

            FactureF F = this.context.FactureFs.Where(x => x.Id == IdFactureF).FirstOrDefault();
            PaiementF PaiementFactureFournisseur = this.context.PaiementFs.Where(x => x.IdFactureF == IdFactureF).FirstOrDefault();
            if(PaiementFactureFournisseur == null && F != null)
            {
                PaiementF P = new PaiementF {
                    Id = Guid.NewGuid(),
                    IdFactureF = IdFactureF,
                    Date = F.Date,
                    IdTypePaiement = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb6"),
                    Debit = somme,
                    IdFournisseur = F.IdFournisseur
                };
            }else if(F != null && PaiementFactureFournisseur != null)
            {
                PaiementFactureFournisseur.Debit = somme;
            }

            this.context.SaveChanges();
        }

        public void updatePaiementF(Guid IdBonReception, float totals)
        {
            float somme = 0;
            if (context.BonReceptionItems.Where(x => x.IdBonReception == IdBonReception).Count() > 0)
            {


                if (getCompanyName() == "SUIV" || getCompanyName() == "SBCIT")
                    somme = context.BonReceptionItems.Where(x => x.IdBonReception == IdBonReception).Sum(x => x.Qte * (x.Pu + (x.Pu * (x.Article.TVA ?? 20) / 100)));
                else
                {

                    somme = context.BonReceptionItems.Where(x => x.IdBonReception == IdBonReception).Sum(x => x.Qte * x.Pu);

                }

            }
            this.context.PaiementFs.Where<PaiementF>((x => x.BonReception.Id == IdBonReception)).FirstOrDefault<PaiementF>().Debit = somme;

            this.context.SaveChanges();
        }

        public void updateTypeTableauImpression(int type)
        {
            this.context.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "5")).FirstOrDefault<Setting>().Afficher = type;
            this.context.SaveChanges();
        }

        public static int getPolice()
        {
            List<Setting> list = new MySaniSoftContext().Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "2")).ToList<Setting>();
            if (list.Count > 0)
                return list[0].Afficher;
            return 14;
        }

        public static int afficherHT()
        {
            List<Setting> list = new MySaniSoftContext().Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "11")).ToList<Setting>();
            if (list.Count > 0)
                return list[0].Afficher;
            return 0;
        }

        public static int getScannerPriority()
        {
            List<Setting> list = new MySaniSoftContext().Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "10")).ToList<Setting>();
            if (list.Count > 0)
                return list[0].Afficher;
            return 0;
        }

        public static int getShowRef()
        {
            List<Setting> list = new MySaniSoftContext().Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "9")).ToList<Setting>();
            if (list.Count > 0)
                return list[0].Afficher;
            return 0;
        }

        public static int getQteOrder()
        {
            List<Setting> list = new MySaniSoftContext().Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "8")).ToList<Setting>();
            if (list.Count > 0)
                return list[0].Afficher;
            return 0;
        }

        public static int getFormatBL()
        {
            List<Setting> list = new MySaniSoftContext().Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "7")).ToList<Setting>();
            if (list.Count > 0)
                return list[0].Afficher;
            return 0;
        }

        public static int getTypeTableau()
        {
            List<Setting> list = new MySaniSoftContext().Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "5")).ToList<Setting>();
            if (list.Count > 0)
                return list[0].Afficher;
            return 2;
        }

        public static int getColorImpression()
        {
            List<Setting> list = new MySaniSoftContext().Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "4")).ToList<Setting>();
            if (list.Count > 0)
                return list[0].Afficher;
            return 3;
        }

        public static string getCompanyName()
        {
            Company company = new MySaniSoftContext().Companies.FirstOrDefault();
            if (company != null)
                return company.Name.ToUpper();
            return "";
        }

        public static Company getCompany()
        {
            Company company = new MySaniSoftContext().Companies.FirstOrDefault();
            if (company != null)
                return company;
            return null;
        }

        public static string getAddressEmail()
        {
            Company company = new MySaniSoftContext().Companies.FirstOrDefault();
            if (company != null)
                return company.Adresse.ToLower();
            return "";
        }
        public static string getSecuredCode()
        {
            Company company = new MySaniSoftContext().Companies.FirstOrDefault();
            if (company != null)
                return company.CodeSecurite;
            return "";
        }


        public static string getModeReglementFacture(Guid IdFacture)
        {
            string modeReglement =  new MySaniSoftContext().Factures.Where((x => x.Id == IdFacture)).Select((x => x.Comment)).FirstOrDefault();
            if (new MySaniSoftContext().Factures.Where((x => x.Id == IdFacture)).Select((x => x.Comment)).FirstOrDefault() == null)
                return "";
            return modeReglement;
        }

        public static string getModeReglementFakeFacture(Guid IdFakeFacture)
        {
            string modeReglement = new MySaniSoftContext().FakeFactures.Where<FakeFacture>((x => x.Id == IdFakeFacture)).Select((x => x.Comment)).FirstOrDefault<string>();
            if (new MySaniSoftContext().FakeFactures.Where<FakeFacture>((x => x.Id == IdFakeFacture)).Select((x => x.Comment)).FirstOrDefault() == null)
                return "";

            return modeReglement;
        }

        public static string getQrCode()
        {
            Company company = new MySaniSoftContext().Companies.FirstOrDefault<Company>();
            if (company != null && company.QrCode != "")
                return company.QrCode;
            return "";
        }

        public static string getPartner()
        {
            Company company = new MySaniSoftContext().Companies.FirstOrDefault<Company>();
            if (company != null && company.Partner != "")
                return company.Partner;
            return "";
        }

        public static string getHeader()
        {
            Company company = new MySaniSoftContext().Companies.FirstOrDefault<Company>();
            if (company != null)
                return company.Header;
            return "";
        }

        public static string getFooter()
        {
            Company company = new MySaniSoftContext().Companies.FirstOrDefault<Company>();
            if (company != null)
                return company.Footer;
            return "";
        }

        public static int getPoliceImpression()
        {
            List<Setting> list = new MySaniSoftContext().Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "3")).ToList<Setting>();
            if (list.Count > 0)
                return list[0].Afficher;
            return 10;
        }

        public ActionResult getTotalWithTVA(Guid IdFacture)
        {
            double num1;
            if (this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)0.0f)).FirstOrDefault<FactureItem>() != null)
                num1 = (double)this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)0.0f)).Sum<FactureItem>((x => x.Qte * x.Pu));
            else
                num1 = 0.0;
            float num2 = (float)num1;
            double num3;
            if (this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)7f)).FirstOrDefault<FactureItem>() != null)
                num3 = (double)this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)7f)).Sum<FactureItem>((x => x.Qte * x.Pu));
            else
                num3 = 0.0;
            float num4 = (float)num3;
            double num5;
            if (this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)10f)).FirstOrDefault<FactureItem>() != null)
                num5 = (double)this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)10f)).Sum<FactureItem>((x => x.Qte * x.Pu));
            else
                num5 = 0.0;
            float num6 = (float)num5;
            double num7;
            if (this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)14f)).FirstOrDefault<FactureItem>() != null)
                num7 = (double)this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)14f)).Sum<FactureItem>((x => x.Qte * x.Pu));
            else
                num7 = 0.0;
            float num8 = (float)num7;
            double num9;
            if (this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)20f)).FirstOrDefault<FactureItem>() != null)
                num9 = (double)this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)20f)).Sum<FactureItem>((x => x.Qte * x.Pu));
            else
                num9 = 0.0;
            float num10 = (float)num9;
            return (ActionResult)this.Json(new
            {
                tva0 = num2,
                tva7 = num4,
                tva10 = num6,
                tva14 = num8,
                tva20 = num10
            }, JsonRequestBehavior.AllowGet);
        }

        public Dictionary<double, double> totalWithTVAFacture(Guid IdFacture)
        {
            Dictionary<double, double> dictionary = new Dictionary<double, double>();
            double num1;
            if (this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)0.0f)).FirstOrDefault<FactureItem>() != null)
                num1 = (double)this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)0.0f)).Sum<FactureItem>((x => x.Qte * x.Pu));
            else
                num1 = 0.0;
            float num2 = (float)num1;
            double num3;
            if (this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)7f)).FirstOrDefault<FactureItem>() != null)
                num3 = (double)this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)7f)).Sum<FactureItem>((x => x.Qte * x.Pu));
            else
                num3 = 0.0;
            float num4 = (float)num3;
            double num5;
            if (this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)10f)).FirstOrDefault<FactureItem>() != null)
                num5 = (double)this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)10f)).Sum<FactureItem>((x => x.Qte * x.Pu));
            else
                num5 = 0.0;
            float num6 = (float)num5;
            double num7;
            if (this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)14f)).FirstOrDefault<FactureItem>() != null)
                num7 = (double)this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)14f)).Sum<FactureItem>((x => x.Qte * x.Pu));
            else
                num7 = 0.0;
            float num8 = (float)num7;
            double num9;
            if (this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)20f)).FirstOrDefault<FactureItem>() != null)
                num9 = (double)this.context.FactureItems.Where<FactureItem>((x => x.IdFacture == IdFacture && x.Article.TVA == (float?)20f)).Sum<FactureItem>((x => x.Qte * x.Pu));
            else
                num9 = 0.0;
            float num10 = (float)num9;
            dictionary.Add(0.0, (double)num2);
            dictionary.Add(7.0, (double)num4);
            dictionary.Add(10.0, (double)num6);
            dictionary.Add(14.0, (double)num8);
            dictionary.Add(20.0, (double)num10);
            return dictionary;
        }

        public Dictionary<double, double> totalWithTVAFakeFacture(Guid IdFakeFacture)
        {
            Dictionary<double, double> dictionary = new Dictionary<double, double>();
            double num1;
            if (this.context.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>)(x => x.IdFakeFacture == IdFakeFacture && x.ArticleFacture.TVA == (float?)0.0f)).FirstOrDefault<FakeFactureItem>() != null)
                num1 = (double)this.context.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>)(x => x.IdFakeFacture == IdFakeFacture && x.ArticleFacture.TVA == (float?)0.0f)).Sum<FakeFactureItem>((Expression<Func<FakeFactureItem, float>>)(x => x.Qte * x.Pu));
            else
                num1 = 0.0;
            float num2 = (float)num1;
            double num3;
            if (this.context.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>)(x => x.IdFakeFacture == IdFakeFacture && x.ArticleFacture.TVA == (float?)7f)).FirstOrDefault<FakeFactureItem>() != null)
                num3 = (double)this.context.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>)(x => x.IdFakeFacture == IdFakeFacture && x.ArticleFacture.TVA == (float?)7f)).Sum<FakeFactureItem>((Expression<Func<FakeFactureItem, float>>)(x => x.Qte * x.Pu));
            else
                num3 = 0.0;
            float num4 = (float)num3;
            double num5;
            if (this.context.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>)(x => x.IdFakeFacture == IdFakeFacture && x.ArticleFacture.TVA == (float?)10f)).FirstOrDefault<FakeFactureItem>() != null)
                num5 = (double)this.context.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>)(x => x.IdFakeFacture == IdFakeFacture && x.ArticleFacture.TVA == (float?)10f)).Sum<FakeFactureItem>((Expression<Func<FakeFactureItem, float>>)(x => x.Qte * x.Pu));
            else
                num5 = 0.0;
            float num6 = (float)num5;
            double num7;
            if (this.context.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>)(x => x.IdFakeFacture == IdFakeFacture && x.ArticleFacture.TVA == (float?)14f)).FirstOrDefault<FakeFactureItem>() != null)
                num7 = (double)this.context.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>)(x => x.IdFakeFacture == IdFakeFacture && x.ArticleFacture.TVA == (float?)14f)).Sum<FakeFactureItem>((Expression<Func<FakeFactureItem, float>>)(x => x.Qte * x.Pu));
            else
                num7 = 0.0;
            float num8 = (float)num7;
            double num9;
            if (this.context.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>)(x => x.IdFakeFacture == IdFakeFacture && x.ArticleFacture.TVA == (float?)20f)).FirstOrDefault<FakeFactureItem>() != null)
                num9 = (double)this.context.FakeFactureItems.Where<FakeFactureItem>((Expression<Func<FakeFactureItem, bool>>)(x => x.IdFakeFacture == IdFakeFacture && x.ArticleFacture.TVA == (float?)20f)).Sum<FakeFactureItem>((Expression<Func<FakeFactureItem, float>>)(x => x.Qte * x.Pu));
            else
                num9 = 0.0;
            float num10 = (float)num9;
            dictionary.Add(0.0, (double)num2);
            dictionary.Add(7.0, (double)num4);
            dictionary.Add(10.0, (double)num6);
            dictionary.Add(14.0, (double)num8);
            dictionary.Add(20.0, (double)num10);
            return dictionary;
        }

        public Dictionary<double, double> totalWithTVADevis(Guid IdDevis)
        {
            Dictionary<double, double> dictionary = new Dictionary<double, double>();
            double num1;
            if (this.context.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>)(x => x.IdDevis == IdDevis && x.Article.TVA == (float?)0.0f)).FirstOrDefault<DevisItem>() != null)
                num1 = (double)this.context.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>)(x => x.IdDevis == IdDevis && x.Article.TVA == (float?)0.0f)).Sum<DevisItem>((Expression<Func<DevisItem, float>>)(x => x.Qte * x.Pu));
            else
                num1 = 0.0;
            float num2 = (float)num1;
            double num3;
            if (this.context.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>)(x => x.IdDevis == IdDevis && x.Article.TVA == (float?)7f)).FirstOrDefault<DevisItem>() != null)
                num3 = (double)this.context.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>)(x => x.IdDevis == IdDevis && x.Article.TVA == (float?)7f)).Sum<DevisItem>((Expression<Func<DevisItem, float>>)(x => x.Qte * x.Pu));
            else
                num3 = 0.0;
            float num4 = (float)num3;
            double num5;
            if (this.context.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>)(x => x.IdDevis == IdDevis && x.Article.TVA == (float?)10f)).FirstOrDefault<DevisItem>() != null)
                num5 = (double)this.context.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>)(x => x.IdDevis == IdDevis && x.Article.TVA == (float?)10f)).Sum<DevisItem>((Expression<Func<DevisItem, float>>)(x => x.Qte * x.Pu));
            else
                num5 = 0.0;
            float num6 = (float)num5;
            double num7;
            if (this.context.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>)(x => x.IdDevis == IdDevis && x.Article.TVA == (float?)14f)).FirstOrDefault<DevisItem>() != null)
                num7 = (double)this.context.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>)(x => x.IdDevis == IdDevis && x.Article.TVA == (float?)14f)).Sum<DevisItem>((Expression<Func<DevisItem, float>>)(x => x.Qte * x.Pu));
            else
                num7 = 0.0;
            float num8 = (float)num7;
            double num9;
            if (this.context.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>)(x => x.IdDevis == IdDevis && x.Article.TVA == (float?)20f)).FirstOrDefault<DevisItem>() != null)
                num9 = (double)this.context.DevisItems.Where<DevisItem>((Expression<Func<DevisItem, bool>>)(x => x.IdDevis == IdDevis && x.Article.TVA == (float?)20f)).Sum<DevisItem>((Expression<Func<DevisItem, float>>)(x => x.Qte * x.Pu));
            else
                num9 = 0.0;
            float num10 = (float)num9;
            dictionary.Add(0.0, (double)num2);
            dictionary.Add(7.0, (double)num4);
            dictionary.Add(10.0, (double)num6);
            dictionary.Add(14.0, (double)num8);
            dictionary.Add(20.0, (double)num10);
            return dictionary;
        }

        public ActionResult getPlafondByClient(Guid IdClient)
        {
            float num = 0.0f;
            List<Client> list = this.context.Clients.Where<Client>((Expression<Func<Client, bool>>)(x => x.Id == IdClient)).ToList<Client>();
            if (list.Count > 0)
                num = list[0].Plafond;
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getArticleByBarCode(string barCode)
        {
            Article article = this.context.Articles.Where<Article>((Expression<Func<Article, bool>>)(x => x.BarCode == barCode)).FirstOrDefault<Article>();
            if (article != null)
                return (ActionResult)this.Json(new
                {
                    Id = article.Id,
                    Ref = article.Ref,
                    Designation = article.Designation,
                    PVD = article.PVD,
                    PA = article.PA,
                    QteStock = article.QteStock,
                    BarCode = article.BarCode
                }, JsonRequestBehavior.AllowGet);
            return (ActionResult)null;
        }

        public ActionResult getTotalSoldeClient()
        {
            //return Json(GetCurrentTime().ToShortTimeString(), JsonRequestBehavior.AllowGet);
      
            return (ActionResult)this.Json((float)((double)this.context.Paiements.Where<Paiement>((x => x.IdClient != new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"))).Sum<Paiement>((x => x.Debit)) - (double)this.context.Paiements.Where<Paiement>((x => x.IdClient != new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"))).Sum<Paiement>((x => x.Credit))), JsonRequestBehavior.AllowGet);
        }
        public DateTime GetCurrentTime()
        {
            DateTime serverTime = DateTime.Now;
            DateTime _localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "Morocco Standard Time");
            return _localTime;
        }
        public ActionResult getNbrFactureMois()
        {
            DateTime dt = GetCurrentTime();
            return (ActionResult)this.Json((float)this.context.Factures.Where<Facture>((x => x.Date.Month == dt.Month && x.Date.Year == dt.Year)).Count<Facture>(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNbrBLMois()
        {
            DateTime dt = GetCurrentTime();
            return (ActionResult)this.Json((float)this.context.BonLivraisons.Where((x => x.Date.Month == dt.Month && x.Date.Year == dt.Year)).Count(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTotalSoldeFournisseur()
        {
            return (ActionResult)this.Json((float)((double)this.context.PaiementFs.Sum<PaiementF>((x => x.Debit)) - (double)this.context.PaiementFs.Sum<PaiementF>((x => x.Credit))), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSituationGlobaleClients()
        {
            return this.Json(this.context.Paiements.Where<Paiement>((x => x.IdClient != new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"))).GroupBy(q => new
            {
                IdClient = q.IdClient,
                Name = q.Client.Name
            }).Select(x => new
            {
                Key = x.Key,
                Solde = x.Sum<Paiement>((y => y.Debit)) - x.Sum<Paiement>((y => y.Credit))
            }).OrderByDescending(x => x.Solde).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSituationGlobaleFournisseurs()
        {
            return this.Json(this.context.PaiementFs.GroupBy(q => new
            {
                IdFournisseur = q.IdFournisseur,
                Name = q.Fournisseur.Name
            }).Select(x => new
            {
                Key = x.Key,
                Solde = x.Sum<PaiementF>((y => y.Debit)) - x.Sum<PaiementF>((y => y.Credit))
            }).OrderByDescending(x => x.Solde).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTotalStock()
        {
            float num = 0.0f;
            DbSet<Article> articles = this.context.Articles;
            if (articles.ToList<Article>().Count > 0)
                num = articles.Where<Article>((Expression<Func<Article, bool>>)(x => !x.Ref.Contains("<>") && !x.Ref.Contains("-"))).Sum<Article>((Expression<Func<Article, float>>)(x => x.PA * x.QteStock));
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTotalStockFacture()
        {
            float num = 0.0f;
            DbSet<ArticleFacture> articles = this.context.ArticleFactures;
            if (articles.ToList<ArticleFacture>().Count > 0)
                num = articles.Where<ArticleFacture>((Expression<Func<ArticleFacture, bool>>)(x => !x.Ref.Contains("<>") && !x.Ref.Contains("-"))).Sum<ArticleFacture>((Expression<Func<ArticleFacture, float>>)(x => x.PA * x.QteStock));
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNewClients()
        {
            DateTime dt = GetCurrentTime();
            return (ActionResult)this.Json(this.context.Clients.Where<Client>((Expression<Func<Client, bool>>)(q => DbFunctions.TruncateTime((DateTime?)q.DateCreation) == DbFunctions.TruncateTime((DateTime?)dt))).Select(x => new
            {
                Name = x.Name,
                DateCreation = x.DateCreation,
                Tel = x.Tel
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult chequeAEncaisse()
        {
            DateTime dt = GetCurrentTime();
            return (ActionResult)this.Json(this.context.Paiements.Where<Paiement>((q => (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") || q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4")) && DbFunctions.DiffDays((DateTime?)dt, q.DateEcheance) <= (int?)4 && q.EnCaisse != (bool?)true)).Count<Paiement>(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult chequeAEncaisseF()
        {
            DateTime dt = GetCurrentTime();
            return (ActionResult)this.Json(this.context.PaiementFs.Where<PaiementF>((q => (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") || q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4")) && DbFunctions.DiffDays((DateTime?)dt, q.DateEcheance) <= (int?)4 && q.EnCaisse != (bool?)true && q.MonCheque == (bool?)true)).Count<PaiementF>(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPriceLastSell(Guid IdClient, Guid IdArticle)
        {
            float num = 0.0f;
            List<BonLivraisonItem> list = this.context.BonLivraisonItems.Where<BonLivraisonItem>((x => x.IdArticle == IdArticle && x.BonLivraison.IdClient == IdClient)).OrderByDescending<BonLivraisonItem, DateTime>((Expression<Func<BonLivraisonItem, DateTime>>)(q => q.BonLivraison.Date)).Take<BonLivraisonItem>(1).ToList<BonLivraisonItem>();
            if (list.Count > 0)
                num = list[0].Pu;
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPriceLastSellFromDevis(Guid IdClient, Guid IdArticle)
        {
            float num = 0.0f;
            List<DevisItem> list = this.context.DevisItems.Where((x => x.IdArticle == IdArticle && x.Devis.IdClient == IdClient)).OrderByDescending((q => q.Devis.Date)).Take(1).ToList();
            if (list.Count > 0)
                num = list[0].Pu;
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTel()
        {
            string str = "";
            if (this.context.Companies.ToList<Company>().Count > 0)
                str = this.context.Companies.FirstOrDefault<Company>().Tel;
            return (ActionResult)this.Json(str, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getLastBarCode()
        {
            string str1 = "100000";
            if (this.context.Articles.ToList<Article>().Count > 0)
            {
                Article article = this.context.Articles.OrderByDescending<Article, string>((Expression<Func<Article, string>>)(x => x.BarCode)).Take<Article>(1).FirstOrDefault<Article>();
                int num = new Random().Next(100000, 999999);
                string str2 = num.ToString("D6");
                num = int.Parse(article.BarCode ?? str2) + 1;
                str1 = num.ToString();
            }
            return (ActionResult)this.Json(str1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTotalVentes()
        {
            DateTime dt = GetCurrentTime();
            return (ActionResult)this.Json(this.context.BonLivraisonItems.Where<BonLivraisonItem>((q => DbFunctions.TruncateTime((DateTime?)q.BonLivraison.Date) == DbFunctions.TruncateTime((DateTime?)dt))).GroupBy(q => new
            {
                NumBon = q.BonLivraison.NumBon,
                Name = q.BonLivraison.Client.Name
            }).Select(x => new
            {
                Key = x.Key,
                Sum = x.Sum<BonLivraisonItem>((y => y.Pu * y.Qte)),
                marge = x.Sum<BonLivraisonItem>((a => (a.Pu - a.Article.PA) * a.Qte))
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTotalFacturesMois()
        {
            DateTime dt = GetCurrentTime();
            float num = 0.0f;
            if (this.context.FactureItems.Where<FactureItem>((q => q.Facture.Date.Year == dt.Date.Year && q.Facture.Date.Month == dt.Month)).Count<FactureItem>() > 0)
                num = this.context.FactureItems.Where<FactureItem>((q => q.Facture.Date.Year == dt.Date.Year && q.Facture.Date.Month == dt.Month)).Sum<FactureItem>((a => a.Qte * a.Pu));
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getClientUpPlafond()
        {
            ParameterExpression parameterExpression1;
            ParameterExpression parameterExpression2;
            ParameterExpression parameterExpression3;
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: type reference
            return this.Json(this.context.Clients.Select(x => new
            {
                x.Id,
                x.Name,
                x.Adresse,
                x.Tel,
                x.Plafond,
                solde = x.Paiements.Sum(y => y.Debit - y.Credit)


            }).Where(q => q.Plafond != 0.0f && q.solde > q.Plafond), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMargeToday()
        {
            DateTime dt = GetCurrentTime();
            float num = 0.0f;
            if (
                context.BonLivraisons.Where(q => DbFunctions.TruncateTime(q.Date) == DbFunctions.TruncateTime(dt)).Count() > 0)
            {
                num =
                    context.BonLivraisons.Where(q => DbFunctions.TruncateTime(q.Date) == DbFunctions.TruncateTime(dt))
                        .Sum(a => a.Marge ?? 0);
            }
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public int getMargeByDate(DateTime dt)
        {
            float num = 0.0f;
            if (
                context.BonLivraisons.Where(q => DbFunctions.TruncateTime(q.Date) == dt.Date).Count() > 0)
            {
                num =
                    context.BonLivraisons.Where(q => DbFunctions.TruncateTime(q.Date) == dt.Date)
                        .Sum(a => a.Marge ?? 0);
            }
            return (int)num;
        }

        public ActionResult getMargeNetToday()
        {
            DateTime dt = GetCurrentTime();
            float num = 0.0f;
            if (
                this.context.BonLivraisonItems.Where<BonLivraisonItem>(
                    (q =>
                        DbFunctions.TruncateTime((DateTime?)q.BonLivraison.Date) == DbFunctions.TruncateTime((DateTime?)dt)))
                    .FirstOrDefault<BonLivraisonItem>() != null)
                num +=
                    (float)
                    (double)
                    this.context.BonLivraisonItems.Where<BonLivraisonItem>(
                        (q =>
                            DbFunctions.TruncateTime((DateTime?)q.BonLivraison.Date) ==
                            DbFunctions.TruncateTime((DateTime?)dt)))
                        .Sum<BonLivraisonItem>(
                            (Expression<Func<BonLivraisonItem, float>>)(a => (a.Pu - a.Article.PA) * a.Qte));
            if (
                this.context.Depences.Where<Depence>(

                        (x => DbFunctions.TruncateTime((DateTime?)x.Date) == DbFunctions.TruncateTime((DateTime?)dt)))
                    .FirstOrDefault<Depence>() != null)
                num -=
                    (float)
                    (double)
                    this.context.Depences.Where<Depence>(

                            (x => DbFunctions.TruncateTime((DateTime?)x.Date) == DbFunctions.TruncateTime((DateTime?)dt)))
                        .Sum<Depence>((Expression<Func<Depence, float>>)(x => x.Montant));
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMargebyDates(DateTime dateDebut, DateTime dateFin)
        {
            List<BonLivraisonItem> list =
                this.context.BonLivraisonItems.Where<BonLivraisonItem>(
                (q =>
                    q.BonLivraison.Marge != new float?() &&
                    DbFunctions.TruncateTime((DateTime?)q.BonLivraison.Date) >=
                    DbFunctions.TruncateTime((DateTime?)dateDebut) &&
                    DbFunctions.TruncateTime((DateTime?)q.BonLivraison.Date) <=
                    DbFunctions.TruncateTime((DateTime?)dateFin))).ToList<BonLivraisonItem>();
            float num = 0.0f;
            if (list.Count > 0)
                num = list.Sum<BonLivraisonItem>((a => a.BonLivraison.Marge ?? 0.0f));
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMarges()
        {
            return this.Json(this.context.BonLivraisons.GroupBy(q => new
            {
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                Key = x.Key,
                marge = x.Sum(a => a.Marge)
            }).OrderByDescending(x => x.Key), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTotalFactures()
        {
            DateTime dt = GetCurrentTime();
            return (ActionResult)this.Json(this.context.FactureItems.Where<FactureItem>((q => DbFunctions.TruncateTime((DateTime?)q.Facture.Date) == DbFunctions.TruncateTime((DateTime?)dt))).GroupBy(q => new
            {
                NumBon = q.Facture.NumBon,
                Name = q.Facture.Client.Name
            }).Select(x => new
            {
                Key = x.Key,
                Sum = x.Sum<FactureItem>((y => y.Qte * y.Pu))
            }), JsonRequestBehavior.AllowGet);
        }


        public float getTotalDepenseByDate(DateTime dt)
        {
            IQueryable<Depence> source = this.context.Depences.Where<Depence>((q => DbFunctions.TruncateTime(q.Date) == dt.Date));
            float num = 0.0f;
            if (source.ToList<Depence>().Count > 0)
                num = source.Sum<Depence>((Expression<Func<Depence, float>>)(x => x.Montant));

            return num;
        }

        public ActionResult getTotalDepensesToday()
        {
            DateTime dt = GetCurrentTime();
            IQueryable<Depence> source = this.context.Depences.Where<Depence>((q => DbFunctions.TruncateTime((DateTime?)q.Date) == DbFunctions.TruncateTime((DateTime?)dt)));
            float num = 0.0f;
            if (source.ToList<Depence>().Count > 0)
                num = source.Sum<Depence>((Expression<Func<Depence, float>>)(x => x.Montant));
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTotalDepensesThisMonth()
        {
            DateTime dt = GetCurrentTime();
            IQueryable<Depence> source = this.context.Depences.Where<Depence>((q => q.Date.Year == dt.Year && q.Date.Month == dt.Month));
            float num = 0.0f;
            if (source.ToList<Depence>().Count > 0)
                num = source.Sum<Depence>((Expression<Func<Depence, float>>)(x => x.Montant));
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTopClients()
        {
            DateTime dt = GetCurrentTime();
            return (ActionResult)this.Json(this.context.BonLivraisonItems.Where<BonLivraisonItem>((q => q.BonLivraison.Date.Year == dt.Year && q.BonLivraison.Date.Month == dt.Month && q.BonLivraison.Client.Id != new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"))).GroupBy(q => new
            {
                Name = q.BonLivraison.Client.Name
            }).Select(x => new
            {
                Key = x.Key,
                Sum = x.Sum<BonLivraisonItem>((y => y.Qte * y.Pu))
            }).OrderByDescending(w => w.Sum).Take(3), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTopArticles()
        {
            DateTime dt = GetCurrentTime();
            return (ActionResult)this.Json(this.context.BonLivraisonItems.Where<BonLivraisonItem>((q => q.BonLivraison.Date.Year == dt.Year && q.BonLivraison.Date.Month == dt.Month)).GroupBy(q => new
            {
                Designation = q.Article.Designation
            }).Select(x => new
            {
                Key = x.Key,
                nbr = x.Sum<BonLivraisonItem>((q => q.Qte))
            }).OrderByDescending(w => w.nbr).Take(3), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTopArticlesChart()
        {
            DateTime dt = GetCurrentTime();
            return (ActionResult)this.Json(this.context.BonLivraisonItems.Where<BonLivraisonItem>((q => q.BonLivraison.Date.Year == dt.Year && q.BonLivraison.Date.Month == dt.Month)).GroupBy(q => new
            {
                Designation = q.Article.Designation
            }).Select(x => new
            {
                Key = x.Key,
                nbr = x.Sum<BonLivraisonItem>((q => q.Qte))
            }).OrderByDescending(w => w.nbr).Take(30), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTopArticlesACommande()
        {
            return (ActionResult)this.Json(this.context.Articles.Where<Article>((Expression<Func<Article, bool>>)(q => (float?)q.QteStock <= q.MinStock)).Select<Article, string>((Expression<Func<Article, string>>)(x => x.Designation)).Take<string>(3), JsonRequestBehavior.AllowGet);
        }


        public ActionResult getSoldeByClient(Guid IdClient)
        {
            float solde = 0;
            if (
                context.Paiements.Where(q => q.IdClient == IdClient)
                    .GroupBy(q => q.IdClient)
                    .Select(x => new { Sum = (x.Sum(y => y.Debit) - x.Sum(i => i.Credit)) })
                    .Count() > 0)
            {
                solde = context.Paiements.Where(q => q.IdClient == IdClient).GroupBy(q => q.IdClient).Select(x => new { Sum = (x.Sum(y => y.Debit) - x.Sum(i => i.Credit)) }).ToList()[0].Sum;
            }
            return Json(new { solde = solde }, JsonRequestBehavior.AllowGet);

        }


        public float getSoldeByClientBeforeDate(Guid IdClient,DateTime dt)
        {
            float solde = 0;
            if (
                context.Paiements.Where(q => q.IdClient == IdClient && DbFunctions.TruncateTime(q.Date) <= dt.Date)
                    .GroupBy(q => q.IdClient)
                    .Select(x => new { Sum = (x.Sum(y => y.Debit) - x.Sum(i => i.Credit)) })
                    .Count() > 0)
            {
                solde = context.Paiements.Where(q => q.IdClient == IdClient && DbFunctions.TruncateTime(q.Date) <= dt.Date).GroupBy(q => q.IdClient).Select(x => new { Sum = (x.Sum(y => y.Debit) - x.Sum(i => i.Credit)) }).ToList()[0].Sum;
            }
            return solde;

        }
        //SoldeByRevendeur

        public float SoldeByRevendeur(Guid IdRevendeur)
        {
            if (context.Paiements.Where(q => q.Client.IdRevendeur == IdRevendeur).GroupBy(q => q.Client.IdRevendeur).Select(x => new
            {
                Sum = x.Sum(y => y.Debit) - x.Sum(i => i.Credit)
            }).Count() > 0)
            {
                return this.context.Paiements.Where(q => q.Client.IdRevendeur == IdRevendeur).GroupBy(q => q.Client.IdRevendeur).Select(x => new
                {
                    Sum = x.Sum(y => y.Debit) - x.Sum(i => i.Credit)
                }).ToList()[0].Sum;
            }
            else
            {
                return 0;
            }

        }
        public float SoldeByClient(Guid IdClient)
        {
            if (context.Paiements.Where(q => q.IdClient == IdClient).GroupBy(q => q.IdClient).Select(x => new
            {
                Sum = x.Sum(y => y.Debit) - x.Sum(i => i.Credit)
            }).Count() > 0)
            {
                return this.context.Paiements.Where(q => q.IdClient == IdClient).GroupBy(q => q.IdClient).Select(x => new
                {
                    Sum = x.Sum(y => y.Debit) - x.Sum(i => i.Credit)
                }).ToList()[0].Sum;
            }
            else
            {
                return 0;
            }

        }

        public float SoldeByFournisseur_(Guid IdFournisseur)
        {
            if (context.PaiementFs.Where(q => q.IdFournisseur == IdFournisseur).GroupBy(q => q.IdFournisseur).Select(x => new
            {
                Sum = x.Sum(y => y.Debit) - x.Sum(i => i.Credit)
            }).Count() > 0)
            {
                return this.context.PaiementFs.Where(q => q.IdFournisseur == IdFournisseur).GroupBy(q => q.IdFournisseur).Select(x => new
                {
                    Sum = x.Sum(y => y.Debit) - x.Sum(i => i.Credit)
                }).ToList()[0].Sum;
            }
            else
            {
                return 0;
            }

        }

        public ActionResult SoldeByChiffreAffaireThisYear()
        {
            DateTime dt = GetCurrentTime();
            float num = 0.0f;
            if (context.BonLivraisonItems.Where(q => q.BonLivraison.Date.Year == dt.Year).Count() > 0)
            {
                num = this.context.BonLivraisonItems.Where(q => q.BonLivraison.Date.Year == dt.Year).Sum(x => x.Qte * x.Pu);
                if (context.BonAvoirCItems.Where(q => q.BonAvoirC.Date.Year == dt.Year).Count() > 0)
                    num -= (float)this.context.BonAvoirCItems.Where(q => q.BonAvoirC.Date.Year == dt.Year).Sum(x => x.Qte * x.Pu);
            }
            return this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public float getChiffreAffaireByRevendeur(Guid IdRevendeur,DateTime dateDebut,DateTime dateFin)
        {
            float chiffreAffaire = 0;
            if (context.BonLivraisonItems.Where(x => x.BonLivraison.Client.IdRevendeur == IdRevendeur && DbFunctions.TruncateTime(x.BonLivraison.Date) >= dateDebut.Date && DbFunctions.TruncateTime(x.BonLivraison.Date) <= dateFin.Date).Count() > 0){
             chiffreAffaire = context.BonLivraisonItems.Where(x => x.BonLivraison.Client.IdRevendeur == IdRevendeur && DbFunctions.TruncateTime(x.BonLivraison.Date) >= dateDebut.Date && DbFunctions.TruncateTime(x.BonLivraison.Date) <= dateFin.Date).Sum(x => x.Qte * x.Pu);

            }

            float chiffreAffaireBAC = 0;
            if (context.BonAvoirCItems.Where(x => x.BonAvoirC.Client.IdRevendeur == IdRevendeur && DbFunctions.TruncateTime(x.BonAvoirC.Date) >= dateDebut.Date && DbFunctions.TruncateTime(x.BonAvoirC.Date) <= dateFin.Date).Count() > 0)
            {
                chiffreAffaireBAC = context.BonAvoirCItems.Where(x => x.BonAvoirC.Client.IdRevendeur == IdRevendeur && DbFunctions.TruncateTime(x.BonAvoirC.Date) >= dateDebut.Date && DbFunctions.TruncateTime(x.BonAvoirC.Date) <= dateFin.Date).Sum(x => x.Qte * x.Pu);
            }

            return chiffreAffaire - chiffreAffaireBAC;

        }
        public ActionResult SoldeByChiffreAffaireThisYearFournisseur()
        {
            DateTime dt = GetCurrentTime();
            float num = 0.0f;
            if (this.context.BonReceptionItems.Where(q => q.BonReception.Date.Year == dt.Year).Count() > 0)
            {
                num = this.context.BonReceptionItems.Where(q => q.BonReception.Date.Year == dt.Year).Sum(x => x.Qte * x.Pu);
                if (this.context.BonAvoirItems.Where(q => q.BonAvoir.Date.Year == dt.Year).Count() > 0)
                    num -= (float)(double)this.context.BonAvoirItems.Where(q => q.BonAvoir.Date.Year == dt.Year).Sum(x => x.Qte * x.Pu);
            }
            return this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SoldeByChiffreAffaireLastYear()
        {
            DateTime dt = GetCurrentTime();
            float num = 0.0f;
            if (this.context.BonLivraisonItems.Where(q => q.BonLivraison.Date.Year == dt.Year - 1).Count() > 0)
            {
                num = this.context.BonLivraisonItems.Where(q => q.BonLivraison.Date.Year == dt.Year - 1).Sum(x => x.Qte * x.Pu);
                if (this.context.BonAvoirCItems.Where(q => q.BonAvoirC.Date.Year == dt.Year - 1).Count() > 0)
                    num -= (float)(double)this.context.BonAvoirCItems.Where(q => q.BonAvoirC.Date.Year == dt.Year - 1).Sum(x => x.Qte * x.Pu);
            }
            return this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SoldeByChiffreAffaireLastYearFournisseur()
        {
            DateTime dt = GetCurrentTime();
            float num = 0.0f;
            if (this.context.BonReceptionItems.Where(q => q.BonReception.Date.Year == dt.Year - 1).Count() > 0)
            {
                num = this.context.BonReceptionItems.Where(q => q.BonReception.Date.Year == dt.Year - 1).Sum(x => x.Qte * x.Pu);
                if (this.context.BonAvoirItems.Where(q => q.BonAvoir.Date.Year == dt.Year - 1).Count() > 0)
                    num -= (float)(double)this.context.BonAvoirItems.Where(q => q.BonAvoir.Date.Year == dt.Year - 1).Sum(x => x.Qte * x.Pu);
            }
            return (ActionResult)this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public float SoldeByFournisseur(Guid IdFournisseur)
        {
            return
                this.context.PaiementFs.Where(q => q.IdFournisseur == IdFournisseur)
                    .GroupBy(q => q.IdFournisseur)
                    .Select(x => new
                    {
                        Sum = x.Sum(y => y.Debit) - x.Sum(i => i.Credit)
                    }).ToList()[0].Sum;
        }

        public ActionResult getSoldeByFournisseur(Guid IdFournisseur)
        {
            float solde =
                context.PaiementFs.Where(q => q.IdFournisseur == IdFournisseur)
                    .GroupBy(q => q.IdFournisseur)
                    .Select(x => new { Sum = (x.Sum(y => y.Debit) - x.Sum(i => i.Credit)) })
                    .ToList()[0].Sum;
            return Json(new { solde = solde }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSoldeTopClients()
        {
            var solde =
                context.Paiements.Where(q => q.IdClient != new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"))
                    .GroupBy(q => new { q.IdClient, q.Client.Name })
                    .Select(x => new { x.Key, Sum = (x.Sum(y => y.Debit) - x.Sum(i => i.Credit)) })
                    .OrderByDescending(x => x.Sum)
                    .Take(8)
                    .ToList();
            return Json(new { solde }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getSoldeAllClients()
        {
            return this.Json(this.context.Paiements.Where(q => q.IdClient != new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39")).GroupBy(q => new
            {
                IdClient = q.IdClient,
                Name = q.Client.Name
            }).Select(x => new
            {
                Key = x.Key,
                Sum = x.Sum(y => y.Debit) - x.Sum(i => i.Credit)
            }).OrderByDescending(x => x.Sum).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTotalCheques()
        {
            DateTime dt = GetCurrentTime();
            var totalCheques = context.Paiements.Where(q => DbFunctions.TruncateTime(q.Date) == DbFunctions.TruncateTime(dt) && (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") || q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4"))).GroupBy(q => new { q.Date.Day, q.Date.Month, q.Date.Year }).Select(x => new { total_ = x.Sum(q => q.Credit) }).ToList();//.Select(g => new { g.Key, TotalSales = g.Sum(x => x.Sales) })
            float total = 0;
            if (totalCheques.Count > 0)
                total += totalCheques[0].total_;
            return Json(total, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTotalChequesToFournisseurs()
        {
            DateTime dt = GetCurrentTime();
            var list1 =
                this.context.PaiementFs.Where(
                    q =>
                        q.MonCheque == true &&
                        (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") ||
                         q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4")) &&
                        DbFunctions.TruncateTime(q.DateEcheance) > DbFunctions.TruncateTime(dt)).ToList();


            /* List<PaiementF> list1 = this.context.PaiementFs.Where<PaiementF>((Expression<Func<PaiementF, bool>>)(q => 
             q.MonCheque == (bool?)true && (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") || 
             q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4")) && 
             DbFunctions.TruncateTime(q.DateEcheance) > DbFunctions.TruncateTime((DateTime?)dt))).ToList<PaiementF>();*/
            //List<PaiementF> list2 = this.context.PaiementFs.Where<PaiementF>((Expression<Func<PaiementF, bool>>) (q => q.MonCheque == (bool?) true && q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeece1"))).ToList<PaiementF>();
            var list2 =
              this.context.PaiementFs.Where(
                  q =>
                      q.MonCheque == true &&
                      q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeece1") &&
                        DbFunctions.TruncateTime(q.DateEcheance) > DbFunctions.TruncateTime(dt)).ToList();
            float num = 0.0f;
            if (list1.Count > 0)
                num += list1.Sum(x => x.Credit);
            if (list2.Count > 0)
                num -= list2.Sum(x => x.Debit);
            return this.Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMargeByBL(Guid IdBonLivraison)
        {
            DateTime now = GetCurrentTime();
            return
                (ActionResult)
                this.Json(

                    this.context.BonLivraisonItems.Where(q => q.IdBonLivraison == IdBonLivraison)
                        .Select(x => x.BonLivraison.Marge ?? 0.0f)
                        .ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMargesBL(DateTime? dateDebut, DateTime? dateFin, Guid IdClient)
        {
            DateTime now = GetCurrentTime();
            if (!dateDebut.HasValue || !dateFin.HasValue)
            {
                float num1 = 0.0f;
                float num2 = 0.0f;
                float num3 = 0.0f;
                var list = this.context.BonLivraisonItems.Where<BonLivraisonItem>((x => x.BonLivraison.IdClient == IdClient)).GroupBy(x => new
                {
                    NumBon = x.BonLivraison.NumBon,
                    Date = new
                    {
                        date = x.BonLivraison.Date.Day + "/" + x.BonLivraison.Date.Month + "/" + x.BonLivraison.Date.Year
                    },
                    Name = x.BonLivraison.Client.Name,
                    Marge = x.BonLivraison.Marge
                }).Select(x => new
                {
                    Key = x.Key,
                    somme = x.Sum<BonLivraisonItem>((q => q.Qte * q.Pu)),
                    margeBL = x.Key.Marge
                }).OrderByDescending(x => x.Key.Date).ToList();
                if (list.ToList().Count > 0)
                    num2 = list.Sum(x => x.margeBL ?? 0.0f);
                if (this.context.BonAvoirCs.Where<BonAvoirC>((x => x.IdClient == IdClient && x.Marge != new float?())).ToList<BonAvoirC>().Count > 0)
                    num1 = this.context.BonAvoirCs.Where<BonAvoirC>((x => x.IdClient == IdClient)).Sum<BonAvoirC>((x => x.Marge ?? 0.0f));
                if (this.context.Paiements.Where<Paiement>((x => x.IdClient == IdClient && x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb5"))).ToList<Paiement>().Count > 0)
                    num3 = this.context.Paiements.Where<Paiement>((x => x.IdClient == IdClient && x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb5"))).Sum<Paiement>((x => x.Credit));
                return this.Json(new
                {
                    marge = list,
                    margeAvoirC = num1,
                    Remises = num3,
                    totalMarge = num2,
                    margeNet = (num2 - num3 - num1)
                }, JsonRequestBehavior.AllowGet);
            }
            float num4 = 0.0f;
            float num5 = 0.0f;
            float num6 = 0.0f;
            var list1 = this.context.BonLivraisonItems.Where<BonLivraisonItem>((Expression<Func<BonLivraisonItem, bool>>)(x => (DateTime?)x.BonLivraison.Date >= dateDebut && (DateTime?)x.BonLivraison.Date <= dateFin && x.BonLivraison.IdClient == IdClient)).GroupBy(x => new
            {
                NumBon = x.BonLivraison.NumBon,
                Date = new
                {
                    date = x.BonLivraison.Date.Day + "/" + x.BonLivraison.Date.Month + "/" + x.BonLivraison.Date.Year
                },
                Name = x.BonLivraison.Client.Name,
                Marge = x.BonLivraison.Marge
            }).Select(x => new
            {
                Key = x.Key,
                somme = x.Sum(q => q.Qte * q.Pu),
                margeBL = x.Key.Marge
            }).OrderByDescending(x => x.Key.Date).ToList();
            if (list1.ToList().Count > 0)
                num5 = list1.Sum(x => x.margeBL ?? 0.0f);
            if (this.context.BonAvoirCs.Where<BonAvoirC>((x => (DateTime?)x.Date >= dateDebut && (DateTime?)x.Date <= dateFin && x.IdClient == IdClient)).ToList<BonAvoirC>().Count > 0)
                num4 = this.context.BonAvoirCs.Where<BonAvoirC>((x => (DateTime?)x.Date >= dateDebut && (DateTime?)x.Date <= dateFin && x.IdClient == IdClient)).Sum<BonAvoirC>((Expression<Func<BonAvoirC, float>>)(x => x.Marge ?? 0.0f));
            if (this.context.Paiements.Where<Paiement>((x => (DateTime?)x.Date >= dateDebut && (DateTime?)x.Date <= dateFin && x.IdClient == IdClient && x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb5"))).ToList<Paiement>().Count > 0)
                num6 = this.context.Paiements.Where<Paiement>((x => (DateTime?)x.Date >= dateDebut && (DateTime?)x.Date <= dateFin && x.IdClient == IdClient && x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb5"))).Sum<Paiement>((x => x.Credit));
            return (ActionResult)this.Json(new
            {
                marge = list1,
                margeAvoirC = num4,
                Remises = num6,
                totalMarge = num5,
                margeNet = (num5 - num6 - num4)
            }, JsonRequestBehavior.AllowGet);
        }

        public float getMargeByRevendeur(Guid IdRevendeur, DateTime dateDebut,DateTime dateFin)
        {
            if(context.BonLivraisons.Where(x => x.Client.IdRevendeur == IdRevendeur && DbFunctions.TruncateTime(x.Date) <= dateFin.Date && DbFunctions.TruncateTime(x.Date) >= dateDebut.Date).Count() > 0)
            return context.BonLivraisons.Where(x => x.Client.IdRevendeur == IdRevendeur && DbFunctions.TruncateTime(x.Date) <= dateFin.Date && DbFunctions.TruncateTime(x.Date) >= dateDebut.Date).Sum(x => x.Marge) ?? 0;

            return 0;
        }

        public ActionResult getQte(DateTime? dateDebut, DateTime? dateFin, Guid? IdArticle, Guid? IdClient)
        {
            DateTime now = GetCurrentTime();
            if (IdArticle.HasValue && IdClient.HasValue)
            {
                if (!dateDebut.HasValue || !dateFin.HasValue)
                    return
                        (ActionResult)
                        this.Json(

                            this.context.BonLivraisonItems.Where<BonLivraisonItem>(

                                    (x => (Guid?)x.BonLivraison.IdClient == IdClient && (Guid?)x.IdArticle == IdArticle))
                                .Sum<BonLivraisonItem>((Expression<Func<BonLivraisonItem, float>>)(x => x.Qte)),
                            JsonRequestBehavior.AllowGet);
                return
                    (ActionResult)
                    this.Json(

                        this.context.BonLivraisonItems.Where<BonLivraisonItem>(

                                (x =>
                                    (DateTime?)x.BonLivraison.Date >= dateDebut && (DateTime?)x.BonLivraison.Date <= dateFin &&
                                    (Guid?)x.BonLivraison.IdClient == IdClient && (Guid?)x.IdArticle == IdArticle))
                            .Sum<BonLivraisonItem>((Expression<Func<BonLivraisonItem, float>>)(x => x.Qte)),
                        JsonRequestBehavior.AllowGet);
            }
            if (!IdArticle.HasValue || IdClient.HasValue)
                return (ActionResult)this.Json(0, JsonRequestBehavior.AllowGet);
            if (!dateDebut.HasValue || !dateFin.HasValue)
                return
                    (ActionResult)
                    this.Json(

                        this.context.BonLivraisonItems.Where<BonLivraisonItem>(
                                 (x => (Guid?)x.IdArticle == IdArticle))
                            .Sum<BonLivraisonItem>((Expression<Func<BonLivraisonItem, float>>)(x => x.Qte)),
                        JsonRequestBehavior.AllowGet);
            return
                (ActionResult)
                this.Json(

                    this.context.BonLivraisonItems.Where<BonLivraisonItem>(

                            (x =>
                                (DateTime?)x.BonLivraison.Date >= dateDebut && (DateTime?)x.BonLivraison.Date <= dateFin &&
                                (Guid?)x.IdArticle == IdArticle))
                        .Sum<BonLivraisonItem>((Expression<Func<BonLivraisonItem, float>>)(x => x.Qte)),
                    JsonRequestBehavior.AllowGet);
        }

        public ActionResult getQteF(DateTime? dateDebut, DateTime? dateFin, Guid? IdArticle, Guid? IdFournisseur)
        {
            DateTime now = GetCurrentTime();
            if (IdArticle.HasValue && IdFournisseur.HasValue)
            {
                if (!dateDebut.HasValue || !dateFin.HasValue)
                    return (ActionResult)this.Json(this.context.BonReceptionItems.Where<BonReceptionItem>((Expression<Func<BonReceptionItem, bool>>)(x => (Guid?)x.BonReception.IdFournisseur == IdFournisseur && (Guid?)x.IdArticle == IdArticle)).Sum<BonReceptionItem>((Expression<Func<BonReceptionItem, float>>)(x => x.Qte)), JsonRequestBehavior.AllowGet);
                return (ActionResult)this.Json(this.context.BonReceptionItems.Where<BonReceptionItem>((Expression<Func<BonReceptionItem, bool>>)(x => (DateTime?)x.BonReception.Date >= dateDebut && (DateTime?)x.BonReception.Date <= dateFin && (Guid?)x.BonReception.IdFournisseur == IdFournisseur && (Guid?)x.IdArticle == IdArticle)).Sum<BonReceptionItem>((Expression<Func<BonReceptionItem, float>>)(x => x.Qte)), JsonRequestBehavior.AllowGet);
            }
            if (!IdArticle.HasValue || IdFournisseur.HasValue)
                return (ActionResult)this.Json(0, JsonRequestBehavior.AllowGet);
            if (!dateDebut.HasValue || !dateFin.HasValue)
                return (ActionResult)this.Json(this.context.BonReceptionItems.Where<BonReceptionItem>((Expression<Func<BonReceptionItem, bool>>)(x => (Guid?)x.IdArticle == IdArticle)).Sum<BonReceptionItem>((Expression<Func<BonReceptionItem, float>>)(x => x.Qte)), JsonRequestBehavior.AllowGet);
            return (ActionResult)this.Json(this.context.BonReceptionItems.Where<BonReceptionItem>((Expression<Func<BonReceptionItem, bool>>)(x => (DateTime?)x.BonReception.Date >= dateDebut && (DateTime?)x.BonReception.Date <= dateFin && (Guid?)x.IdArticle == IdArticle)).Sum<BonReceptionItem>((Expression<Func<BonReceptionItem, float>>)(x => x.Qte)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getProduitsMoinsVendus()
        {
            DateTime dt = GetCurrentTime();
            List<Guid> list = this.context.BonLivraisonItems.Where<BonLivraisonItem>((x => DbFunctions.DiffMonths((DateTime?)x.BonLivraison.Date, (DateTime?)dt) <= (int?)6)).Select<BonLivraisonItem, Guid>((Expression<Func<BonLivraisonItem, Guid>>)(x => x.Article.Id)).Distinct<Guid>().ToList<Guid>();
            return (ActionResult)this.Json(this.context.Articles.Select(x => new
            {
                Id = x.Id,
                Designation = x.Designation,
                QteStock = x.QteStock,
                Unite = x.Unite,
                PA = x.PA,
                PVD = x.PVD
            }).Where(x => !list.Contains(x.Id) && x.QteStock > 0.0f), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMesChequeEffetFournisseur()
        {
            DateTime dt = GetCurrentTime();
           

            return Json(context.PaiementFs.Where(x => (x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") || x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4")) &&  x.MonCheque == true && x.DateEcheance > dt && x.DateEcheance.Value.Month >= dt.Month).GroupBy(x => new
            {
                Date = x.DateEcheance.Value.Month
            }).Select(x => new
            {
                Mois = x.Key.Date,
                Montant = x.Sum(y => y.Credit)
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMesChequeEffetFournisseurThisMonth()
        {
            DateTime dt = GetCurrentTime();

            var Impaye = context.PaiementFs.Where(x => (x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeece1")) && x.MonCheque == true && x.DateEcheance.Value.Month == dt.Month && x.DateEcheance.Value.Year == dt.Year).GroupBy(x => new
            {
                Date = x.DateEcheance.Value.Month
            }).Select(x => x.Sum(y => y.Credit)).FirstOrDefault();

            return Json(context.PaiementFs.Where(x => (x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") || x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4")) && x.MonCheque == true && x.DateEcheance.Value.Month == dt.Month && x.DateEcheance.Value.Year == dt.Year).GroupBy(x => new
            {
                Date = x.DateEcheance.Value.Month
            }).Select(x=>x.Sum(y=>y.Credit)).FirstOrDefault() - Impaye, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSituationByDate(DateTime dt)
        {
            return this.Json(this.context.Paiements.Select(x => new
            {
                Date = x.Date,
                Debit = x.Debit,
                Credit = x.Credit,
                TypePaiement = x.TypePaiement.Name,
                Client = x.Client.Name,
                NumBon = x.BonLivraison.NumBon,
                Comment = x.Comment,
                DateEcheance = x.DateEcheance
            }).Where(x=>x.Date.Year == dt.Year && x.Date.Month == dt.Month && x.Date.Day == dt.Day), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getProduitsInAlert()
        {
            DateTime now = GetCurrentTime();
            return (ActionResult)this.Json(this.context.Articles.Select(x => new
            {
                Id = x.Id,
                Designation = x.Designation,
                QteStock = x.QteStock,
                Unite = x.Unite,
                PA = x.PA,
                PVD = x.PVD,
                MinStock = x.MinStock
            }).Where(x => (float?)x.QteStock < x.MinStock).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMargesThisYear()
        {
            DateTime dt = GetCurrentTime();
            int[] numArray = new int[12]
            {
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12
            };
            List<MargeSomme> margeSommeList = new List<MargeSomme>();
            var list1 = this.context.BonLivraisonItems.Where((x => x.BonLivraison.Date.Year == dt.Year)).GroupBy(x => new
            {
                Date = x.BonLivraison.Date.Month
            }).Select(x => new
            {
                Key = x.Key,
                somme = x.Sum((q => q.Pu * q.Qte))
            }).OrderByDescending(x => x.Key.Date).ToList();
            var list2 = this.context.BonLivraisons.Where(x => x.Date.Year == dt.Year).GroupBy(x => new
            {
                Date = x.Date.Month
            }).Select(x => new
            {
                Key = x.Key,
                margeBL = x.Sum(q => q.Marge)
            }).OrderByDescending(x => x.Key.Date).ToList();


            for (int index = 0; index < numArray.Length; ++index)
            {
                int m = numArray[index];
                float somme = 0.0f;
                float margeBL = 0.0f;
                if (list1.Where(x => x.Key.Date == m).FirstOrDefault() != null)
                {
                    somme = list1.Where(x => x.Key.Date == m).FirstOrDefault().somme;
                    if (this.context.BonAvoirCItems.Where((x => x.BonAvoirC.Date.Year == dt.Year && x.BonAvoirC.Date.Month == m)).FirstOrDefault() != null)
                        somme -= (float)context.BonAvoirCItems.Where((x => x.BonAvoirC.Date.Year == dt.Year && x.BonAvoirC.Date.Month == m)).Sum((x => x.Pu * x.Qte));
                }
                if (list2.Where(x => x.Key.Date == m).FirstOrDefault() != null)
                {
                    float? margeBl = list2.Where(x => x.Key.Date == m).FirstOrDefault().margeBL;
                    margeBL = margeBl.HasValue ? margeBl.GetValueOrDefault() : 0.0f;
                    if (this.context.Paiements.Where<Paiement>((x => x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb5") && x.Date.Year == dt.Year && x.Date.Month == m)).ToList<Paiement>().Count > 0)
                        margeBL -= (float)this.context.Paiements.Where<Paiement>((x => x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb5") && x.Date.Year == dt.Year && x.Date.Month == m)).Sum<Paiement>((x => x.Credit));
                    if (this.context.BonAvoirCItems.Where<BonAvoirCItem>((x => x.BonAvoirC.Date.Year == dt.Year && x.BonAvoirC.Date.Month == m)).ToList<BonAvoirCItem>().Count > 0)
                        margeBL -= (float)this.context.BonAvoirCs.Where<BonAvoirC>((x => x.Date.Year == dt.Year && x.Date.Month == m)).Sum<BonAvoirC>((x => x.Marge ?? 0.0f));
                    if (this.context.Depences.Where<Depence>((x => x.Date.Year == dt.Year && x.Date.Month == m)).ToList<Depence>().Count > 0)
                        margeBL -= (float)this.context.Depences.Where<Depence>((x => x.Date.Year == dt.Year && x.Date.Month == m)).Sum<Depence>((x => x.Montant));
                }
                margeSommeList.Add(new MargeSomme(m, somme, margeBL));
            }
            return Json(margeSommeList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMargesByDates(DateTime dateDebut, DateTime dateFin)
        {
            List<MargeSomme> margeSommeList = new List<MargeSomme>();
            var list1 = this.context.BonLivraisonItems.Where((x => DbFunctions.TruncateTime(x.BonLivraison.Date) > dateDebut && DbFunctions.TruncateTime(x.BonLivraison.Date) < dateFin)).GroupBy(x => new
            {
                Date = x.BonLivraison.Date.Month
            }).Select(x => new
            {
                Key = x.Key,
                somme = x.Sum((q => q.Pu * q.Qte))
            }).OrderByDescending(x => x.Key.Date).ToList();
            var list2 = this.context.BonLivraisons.Where(x => DbFunctions.TruncateTime(x.Date) > dateDebut && DbFunctions.TruncateTime(x.Date) < dateFin).GroupBy(x => new
            {
                Date = x.Date.Month
            }).Select(x => new
            {
                Key = x.Key,
                margeBL = x.Sum(q => q.Marge)
            }).OrderByDescending(x => x.Key.Date).ToList();


            
            return Json(margeSommeList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult estimationCaisseToday()
        {
            DateTime dt = GetCurrentTime();
            var list1 = this.context.Paiements.Where(q => DbFunctions.TruncateTime((DateTime?)q.Date) == DbFunctions.TruncateTime((DateTime?)dt) && q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2")).GroupBy(q => new
            {
                Day = q.Date.Day,
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                total_ = x.Sum<Paiement>((q => q.Credit))
            }).ToList();
            var list2 = this.context.Paiements.Where<Paiement>((q => DbFunctions.TruncateTime((DateTime?)q.Date) == DbFunctions.TruncateTime((DateTime?)dt) && (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") && q.EnCaisse == (bool?)true || q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4") && q.EnCaisse == (bool?)true))).GroupBy(q => new
            {
                Day = q.Date.Day,
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                total_ = x.Sum<Paiement>((q => q.Credit))
            }).ToList();
            var list3 = this.context.Paiements.Where(q => DbFunctions.TruncateTime((DateTime?)q.Date) == DbFunctions.TruncateTime((DateTime?)dt) && q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4")).GroupBy(q => new
            {
                Day = q.Date.Day,
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                total_ = x.Sum<Paiement>((q => q.Debit))
            }).ToList();
            float num = 0.0f;
            if (list1.Count > 0)
                num += list1[0].total_;
            if (list2.Count > 0)
                num += list2[0].total_;
            if (list3.Count > 0)
                num -= list3[0].total_;
            return (ActionResult)this.Json(new
            {
                caisse = num
            }, JsonRequestBehavior.AllowGet);
        }

        public float MontantALaCaisse(DateTime dt)
        {
            var list1 = this.context.Paiements.Where(q => DbFunctions.TruncateTime((DateTime?)q.Date) == DbFunctions.TruncateTime((DateTime?)dt) && q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2")).GroupBy(q => new
            {
                Day = q.Date.Day,
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                total_ = x.Sum<Paiement>((q => q.Credit))
            }).ToList();
            var list2 = this.context.Paiements.Where<Paiement>((q => DbFunctions.TruncateTime((DateTime?)q.Date) == DbFunctions.TruncateTime((DateTime?)dt) && (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") && q.EnCaisse == (bool?)true || q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4") && q.EnCaisse == (bool?)true))).GroupBy(q => new
            {
                Day = q.Date.Day,
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                total_ = x.Sum<Paiement>((q => q.Credit))
            }).ToList();
            var list3 = this.context.Paiements.Where(q => DbFunctions.TruncateTime((DateTime?)q.Date) == DbFunctions.TruncateTime((DateTime?)dt) && q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4")).GroupBy(q => new
            {
                Day = q.Date.Day,
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                total_ = x.Sum<Paiement>((q => q.Debit))
            }).ToList();
            float num = 0.0f;
            if (list1.Count > 0)
                num += list1[0].total_;
            if (list2.Count > 0)
                num += list2[0].total_;
            if (list3.Count > 0)
                num -= list3[0].total_;

            num -= getTotalDepenseByDate(dt);
            return num;
        }

        public float getTotalAchatByClient(Guid IdClient,DateTime DateDebut,DateTime DateFin)
        {

            List<Paiement> list =  context.Paiements.Where(x => x.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb6") && x.IdClient == IdClient && DbFunctions.TruncateTime(x.Date) >= DateDebut.Date && DbFunctions.TruncateTime(x.Date) <= DateFin.Date).ToList();

            if(list.Count > 0)
            {
                return list.Sum(x => x.Debit);
            }

            return 0;
        }

        public float getTotalDebitByClient(Guid IdClient)
        {
            List<Paiement> list = context.Paiements.Where(x => x.IdClient == IdClient).ToList();

            if (list.Count > 0)
            {
                return list.Sum(x => x.Debit);
            }

            return 0;
        }

        public float getTotalCreditByClient(Guid IdClient)
        {
            List<Paiement> list = context.Paiements.Where(x => x.IdClient == IdClient).ToList();

            if (list.Count > 0)
            {
                return list.Sum(x => x.Credit);
            }

            return 0;
        }

        public ActionResult estimationEspeceCaisse()
        {
            DateTime dt = GetCurrentTime();
            List<DateTime> dateTimeList = new List<DateTime>();
            List<StatistiqueEspece> source1 = new List<StatistiqueEspece>();
            var list1 = this.context.Paiements.Where(q => DbFunctions.DiffDays((DateTime?)q.Date, (DateTime?)dt) <= (int?)7 && (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2") || (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") && q.EnCaisse == true) || (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4") && q.EnCaisse == true) || q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4"))).GroupBy(q => new
            {
                Day = q.Date.Day,
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                Date = x.Key,
                total_ = x.Sum<Paiement>((q => q.Credit)) - x.Sum<Paiement>((q => q.Debit))
            }).OrderBy(x => new
            {
                Year = x.Date.Year,
                Month = x.Date.Month,
                Day = x.Date.Day
            }).ToList();
            var list2 = this.context.PaiementFs.Where(q => DbFunctions.DiffDays((DateTime?)q.Date, (DateTime?)dt) <= (int?)7 && q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2")).GroupBy(q => new
            {
                Day = q.Date.Day,
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                Date = x.Key,
                total_ = x.Sum<PaiementF>((q => q.Credit))
            }).OrderBy(x => new
            {
                Year = x.Date.Year,
                Month = x.Date.Month,
                Day = x.Date.Day
            }).ToList();
            foreach (var data in list1)
            {
                if (!dateTimeList.Contains(new DateTime(data.Date.Year, data.Date.Month, data.Date.Day)))
                    dateTimeList.Add(new DateTime(data.Date.Year, data.Date.Month, data.Date.Day));
            }
            var list3 = this.context.Depences.Where(q => DbFunctions.DiffDays((DateTime?)q.Date, (DateTime?)dt) <= (int?)7).GroupBy(q => new
            {
                Day = q.Date.Day,
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                Date = x.Key,
                total_ = x.Sum<Depence>((q => q.Montant))
            }).OrderBy(x => new
            {
                Year = x.Date.Year,
                Month = x.Date.Month,
                Day = x.Date.Day
            }).ToList();
            list2.AddRange(list3);
            var source2 = list2.GroupBy(x => x.Date).Select(x => new
            {
                Key = x.Key,
                total_ = x.Sum(q => q.total_)
            });
            foreach (var data in list2)
            {
                if (!dateTimeList.Contains(new DateTime(data.Date.Year, data.Date.Month, data.Date.Day)))
                    dateTimeList.Add(new DateTime(data.Date.Year, data.Date.Month, data.Date.Day));
            }
            dateTimeList.Sort();
            foreach (DateTime dateTime in dateTimeList)
            {
                DateTime date = dateTime;
                float totalDepense = 0.0f;
                float totalCredit = 0.0f;
                if (source2.Where(x => date == new DateTime(x.Key.Year, x.Key.Month, x.Key.Day)).FirstOrDefault() != null)
                    totalDepense = source2.Where(x => date == new DateTime(x.Key.Year, x.Key.Month, x.Key.Day)).FirstOrDefault().total_;
                if (list1.Where(x => date == new DateTime(x.Date.Year, x.Date.Month, x.Date.Day)).FirstOrDefault() != null)
                    totalCredit = list1.Where(x => date == new DateTime(x.Date.Year, x.Date.Month, x.Date.Day)).FirstOrDefault().total_;
                source1.Add(new StatistiqueEspece(date, totalDepense, totalCredit));
            }
            return (ActionResult)this.Json(new
            {
                liste = source1.Select(x => new
                {
                    Date = x.Date.Day.ToString() + "/" + x.Date.Month + "/" + x.Date.Year,
                    totalCredit = x.totalCredit,
                    totalDepense = x.totalDepense,
                    difference = x.totalCredit - x.totalDepense
                }).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult estimationEspeceCaisseMois()
        {
            DateTime dt = GetCurrentTime();
            List<string> stringList1 = new List<string>();
            List<StatistiqueEspeceParMois> source1 = new List<StatistiqueEspeceParMois>();
            var list1 = this.context.Paiements.Where(q => q.Date.Year == dt.Year && (q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2") || q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3") && q.EnCaisse == (bool?)true || q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4") && q.EnCaisse == (bool?)true || q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4"))).GroupBy(q => new
            {
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                Date = x.Key,
                total_ = x.Sum<Paiement>((q => q.Credit)) - x.Sum<Paiement>((q => q.Debit))
            }).OrderBy(x => new
            {
                Year = x.Date.Year,
                Month = x.Date.Month
            }).ToList();
            var list2 = this.context.PaiementFs.Where<PaiementF>((q => q.Date.Year == dt.Year && q.IdTypePaiement == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2"))).GroupBy(q => new
            {
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                Date = x.Key,
                total_ = x.Sum<PaiementF>((q => q.Credit))
            }).OrderBy(x => new
            {
                Year = x.Date.Year,
                Month = x.Date.Month
            }).ToList();
            foreach (var data in list1)
            {
                List<string> stringList2 = stringList1;
                int num = data.Date.Month;
                string str1 = num.ToString();
                string str2 = "/";
                num = data.Date.Year;
                string str3 = num.ToString();
                string str4 = str1 + str2 + str3;
                if (!stringList2.Contains(str4))
                {
                    List<string> stringList3 = stringList1;
                    num = data.Date.Month;
                    string str5 = num.ToString();
                    string str6 = "/";
                    num = data.Date.Year;
                    string str7 = num.ToString();
                    string str8 = str5 + str6 + str7;
                    stringList3.Add(str8);
                }
            }
            var list3 = this.context.Depences.Where(q => q.Date.Year == dt.Year).GroupBy(q => new
            {
                Month = q.Date.Month,
                Year = q.Date.Year
            }).Select(x => new
            {
                Date = x.Key,
                total_ = x.Sum<Depence>((q => q.Montant))
            }).OrderBy(x => new
            {
                Year = x.Date.Year,
                Month = x.Date.Month
            }).ToList();
            list2.AddRange(list3);
            var source2 = list2.GroupBy(x => x.Date).Select(x => new
            {
                Key = x.Key,
                total_ = x.Sum(q => q.total_)
            });
            foreach (var data in list2)
            {
                List<string> stringList2 = stringList1;
                int num = data.Date.Month;
                string str1 = num.ToString();
                string str2 = "/";
                num = data.Date.Year;
                string str3 = num.ToString();
                string str4 = str1 + str2 + str3;
                if (!stringList2.Contains(str4))
                {
                    List<string> stringList3 = stringList1;
                    num = data.Date.Month;
                    string str5 = num.ToString();
                    string str6 = "/";
                    num = data.Date.Year;
                    string str7 = num.ToString();
                    string str8 = str5 + str6 + str7;
                    stringList3.Add(str8);
                }
            }
            stringList1.Sort();
            foreach (string str1_ in stringList1)
            {
                string date = str1_;
                float totalDepense = 0.0f;
                float totalCredit = 0.0f;
                if (source2.Where(x =>
                {
                    string str1 = date;
                    int num = x.Key.Month;
                    string str2 = num.ToString();
                    string str3 = "/";
                    num = x.Key.Year;
                    string str4 = num.ToString();
                    string str5 = str2 + str3 + str4;
                    return str1 == str5;
                }).FirstOrDefault() != null)
                    totalDepense = source2.Where(x =>
                    {
                        string str1 = date;
                        int num = x.Key.Month;
                        string str2 = num.ToString();
                        string str3 = "/";
                        num = x.Key.Year;
                        string str4 = num.ToString();
                        string str5 = str2 + str3 + str4;
                        return str1 == str5;
                    }).FirstOrDefault().total_;
                if (list1.Where(x =>
                {
                    string str1 = date;
                    int num = x.Date.Month;
                    string str2 = num.ToString();
                    string str3 = "/";
                    num = x.Date.Year;
                    string str4 = num.ToString();
                    string str5 = str2 + str3 + str4;
                    return str1 == str5;
                }).FirstOrDefault() != null)
                    totalCredit = list1.Where(x =>
                    {
                        string str1 = date;
                        int num = x.Date.Month;
                        string str2 = num.ToString();
                        string str3 = "/";
                        num = x.Date.Year;
                        string str4 = num.ToString();
                        string str5 = str2 + str3 + str4;
                        return str1 == str5;
                    }).FirstOrDefault().total_;
                source1.Add(new StatistiqueEspeceParMois(date, totalDepense, totalCredit));
            }
            return (ActionResult)this.Json(new
            {
                liste = source1.Select(x => new
                {
                    Date = x.Date,
                    totalCredit = x.totalCredit,
                    totalDepense = x.totalDepense,
                    difference = x.totalCredit - x.totalDepense
                }).OrderBy(x => x.Date).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DecimalToWordsJSON(Decimal nbr)
        {
            return (ActionResult)this.Json(this.DecimalToWords(nbr), JsonRequestBehavior.AllowGet);
        }

        public string DecimalToWords(Decimal number)
        {
            if (number == Decimal.Zero)
                return "zero";
            if (number < Decimal.Zero)
                return "moins " + this.DecimalToWords(Math.Abs(number));
            int chiffre = (int)number;
            int num = (int)((number - chiffre) * 100);
            string str = ConvertisseurChiffresLettres.converti(chiffre) + "dhs TTC";
            if (num > 0) {
                str = str.Replace("TTC", "");
                str = str + " " + num.ToString() + "cts TTC";
            }
            str = str.ToUpper();
            string prefix = "UN";
            return str.StartsWith(prefix) ? str.Substring(prefix.Length) : str;
        }

        //for suiv

        public ActionResult BouteillesFournisseurs()
        {
            var ListBouteillesFournisseurs = new List<BouteillesFournisseursClass>();
            var resultDgbF = context.DgbFItems.GroupBy(x => new { x.DgbF.Fournisseur, x.Article }).Select(x => new { Fournisseur = x.Key.Fournisseur.Name, Article = x.Key.Article.Designation, QteEmballagePleine = x.Sum(y => y.Qte) });
            var resultRdbF = context.RdbFItems.GroupBy(x => new { x.RdbF.Fournisseur, x.Article }).Select(x => new { Fournisseur = x.Key.Fournisseur.Name, Article = x.Key.Article.Designation, QteEmballageVide = x.Sum(y => y.Qte) });

            //fill the list
            foreach (var item in resultDgbF)
            {
                if (ListBouteillesFournisseurs.Where(x => x.Article == item.Article && item.Fournisseur == x.Fournisseur).FirstOrDefault() == null)
                {
                    var bfc = new BouteillesFournisseursClass();
                    bfc.Fournisseur = item.Fournisseur;
                    bfc.Article = item.Article;
                    bfc.QteEmballagePleine = item.QteEmballagePleine;
                    bfc.QteEmballageVide = 0;
                    bfc.QteEmballageReste = item.QteEmballagePleine;
                    ListBouteillesFournisseurs.Add(bfc);
                }

            }

            foreach (var item in resultRdbF)
            {

                if (ListBouteillesFournisseurs.Where(x => x.Article == item.Article && item.Fournisseur == x.Fournisseur).FirstOrDefault() == null)
                {
                    var bfc = new BouteillesFournisseursClass();
                    bfc.Fournisseur = item.Fournisseur;
                    bfc.Article = item.Article;
                    bfc.QteEmballagePleine = 0;
                    bfc.QteEmballageVide = item.QteEmballageVide;
                    bfc.QteEmballageReste = 0;
                    ListBouteillesFournisseurs.Add(bfc);
                }

            }

            foreach (var item in resultRdbF)
            {
                if (ListBouteillesFournisseurs.Where(x => x.Article == item.Article && item.Fournisseur == x.Fournisseur).FirstOrDefault() != null)
                {
                    var bfc = ListBouteillesFournisseurs.Where(x => x.Article == item.Article && item.Fournisseur == x.Fournisseur).FirstOrDefault();
                    bfc.QteEmballageVide = item.QteEmballageVide;
                    bfc.QteEmballageReste = bfc.QteEmballagePleine - item.QteEmballageVide;
                }
            }


            return Json(ListBouteillesFournisseurs, JsonRequestBehavior.AllowGet);
        }


        //client

        public ActionResult BouteillesClients()
        {
            var ListBouteillesClients = new List<BouteillesClientsClass>();
            var resultDgb = context.DgbItems.GroupBy(x => new { x.Dgb.Client, x.Article }).Select(x => new { Client = x.Key.Client.Name, Article = x.Key.Article.Designation, QteEmballagePleine = x.Sum(y => y.Qte) });
            var resultRdb = context.RdbItems.GroupBy(x => new { x.Rdb.Client, x.Article }).Select(x => new { Client = x.Key.Client.Name, Article = x.Key.Article.Designation, QteEmballageVide = x.Sum(y => y.Qte) });

            //fill the list
            foreach (var item in resultDgb)
            {
                if (ListBouteillesClients.Where(x => x.Article == item.Article && item.Client == x.Client).FirstOrDefault() == null)
                {
                    var bfc = new BouteillesClientsClass();
                    bfc.Client = item.Client;
                    bfc.Article = item.Article;
                    bfc.QteEmballagePleine = item.QteEmballagePleine;
                    bfc.QteEmballageVide = 0;
                    bfc.QteEmballageReste = item.QteEmballagePleine;
                    ListBouteillesClients.Add(bfc);
                }

            }

            foreach (var item in resultRdb)
            {

                if (ListBouteillesClients.Where(x => x.Article == item.Article && item.Client == x.Client).FirstOrDefault() == null)
                {
                    var bfc = new BouteillesClientsClass();
                    bfc.Client = item.Client;
                    bfc.Article = item.Article;
                    bfc.QteEmballagePleine = 0;
                    bfc.QteEmballageVide = item.QteEmballageVide;
                    bfc.QteEmballageReste = 0;
                    ListBouteillesClients.Add(bfc);
                }

            }

            foreach (var item in resultRdb)
            {
                if (ListBouteillesClients.Where(x => x.Article == item.Article && item.Client == x.Client).FirstOrDefault() != null)
                {
                    var bfc = ListBouteillesClients.Where(x => x.Article == item.Article && item.Client == x.Client).FirstOrDefault();
                    bfc.QteEmballageVide = item.QteEmballageVide;
                    bfc.QteEmballageReste = bfc.QteEmballagePleine - item.QteEmballageVide;
                }
            }


            return Json(ListBouteillesClients, JsonRequestBehavior.AllowGet);
        }
    }
    public class BouteillesFournisseursClass
    {
        public string Fournisseur { get; set; }
        public string Article { get; set; }
        public float QteEmballagePleine { get; set; }
        public float QteEmballageVide { get; set; }
        public float QteEmballageReste { get; set; }

    }

    public class BouteillesClientsClass
    {
        public string Client { get; set; }
        public string Article { get; set; }
        public float QteEmballagePleine { get; set; }
        public float QteEmballageVide { get; set; }
        public float QteEmballageReste { get; set; }

    }
}
