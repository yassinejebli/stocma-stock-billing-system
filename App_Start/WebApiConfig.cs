// Decompiled with JetBrains decompiler
// Type: WebApplication1.WebApiConfig
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using Microsoft.Data.Edm;
using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData.Extensions;
using WebApplication1.DATA;
using Microsoft.AspNet.OData.Builder;

namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Select().Expand().Filter().OrderBy().MaxTop(null).Count();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new
            {
                id = RouteParameter.Optional
            });
            config.MapODataServiceRoute("odata", "odata", GetModel());
            config.SetTimeZoneInfo(TimeZoneInfo.Utc);

            /*config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new
            {
                id = RouteParameter.Optional
            });
            config.Routes.MapODataServiceRoute("odata", "odata", WebApiConfig.GetModel());*/
        }

        public static Microsoft.OData.Edm.IEdmModel GetModel()
        {
            var conventionModelBuilder = new ODataConventionModelBuilder();


            string str = "";
            conventionModelBuilder.Namespace = str;
            string name1 = "Tarifs";
            conventionModelBuilder.EntitySet<Tarif>(name1);
            string name2 = "TarifItems";
            conventionModelBuilder.EntitySet<TarifItem>(name2);
            string name3 = "Companies";
            conventionModelBuilder.EntitySet<Company>(name3);
            string name4 = "Settings";
            conventionModelBuilder.EntitySet<Setting>(name4);
            string name5 = "Fournisseurs";
            conventionModelBuilder.EntitySet<Fournisseur>(name5);
            string name6 = "Clients";
            conventionModelBuilder.EntitySet<Client>(name6);
            string name7 = "Articles";
            conventionModelBuilder.EntitySet<Article>(name7);
            string name8 = "BonReceptions";
            conventionModelBuilder.EntitySet<BonReception>(name8);
            string name9 = "BonLivraisons";
            conventionModelBuilder.EntitySet<BonLivraison>(name9);
            string name10 = "Devises";
            conventionModelBuilder.EntitySet<Devis>(name10);
            string name11 = "BonReceptionItems";
            conventionModelBuilder.EntitySet<BonReceptionItem>(name11);
            string name12 = "BonLivraisonItems";
            conventionModelBuilder.EntitySet<BonLivraisonItem>(name12);
            string name13 = "DevisItems";
            conventionModelBuilder.EntitySet<DevisItem>(name13);
            string name14 = "Familles";
            conventionModelBuilder.EntitySet<Famille>(name14);
            string name15 = "BonAvoirs";
            conventionModelBuilder.EntitySet<BonAvoir>(name15);
            string name16 = "BonAvoirItems";
            conventionModelBuilder.EntitySet<BonAvoirItem>(name16);
            string name17 = "BonAvoirCs";
            conventionModelBuilder.EntitySet<BonAvoirC>(name17);
            string name18 = "BonAvoirCItems";
            conventionModelBuilder.EntitySet<BonAvoirCItem>(name18);
            string name19 = "BonCommandes";
            conventionModelBuilder.EntitySet<BonCommande>(name19);
            string name20 = "BonCommandeItems";
            conventionModelBuilder.EntitySet<BonCommandeItem>(name20);
            string name21 = "TypePaiements";
            conventionModelBuilder.EntitySet<TypePaiement>(name21);
            string name22 = "Paiements";
            conventionModelBuilder.EntitySet<Paiement>(name22);
            string name23 = "PaiementFs";
            conventionModelBuilder.EntitySet<PaiementF>(name23);
            string name24 = "Factures";
            conventionModelBuilder.EntitySet<Facture>(name24);
            string name25 = "FactureItems";
            conventionModelBuilder.EntitySet<FactureItem>(name25);
            string name26 = "Depences";
            conventionModelBuilder.EntitySet<Depence>(name26);
            string name27 = "TypeDepences";
            conventionModelBuilder.EntitySet<TypeDepence>(name27);
            string name28 = "JournalConnexions";
            conventionModelBuilder.EntitySet<JournalConnexion>(name28);
            string name29 = "ArticleFactures";
            conventionModelBuilder.EntitySet<ArticleFacture>(name29);
            string name30 = "FakeFactures";
            conventionModelBuilder.EntitySet<FakeFacture>(name30);
            string name31 = "FakeFactureItems";
            conventionModelBuilder.EntitySet<FakeFactureItem>(name31);
            conventionModelBuilder.EntitySet<Categorie>("Categories");
            conventionModelBuilder.EntitySet<Rdb>("Rdbs");
            conventionModelBuilder.EntitySet<RdbItem>("RdbItems");
            conventionModelBuilder.EntitySet<Dgb>("Dgbs");
            conventionModelBuilder.EntitySet<DgbItem>("DgbItems");
            conventionModelBuilder.EntitySet<DgbF>("DgbFs");
            conventionModelBuilder.EntitySet<DgbFItem>("DgbFItems");
            conventionModelBuilder.EntitySet<RdbF>("RdbFs");
            conventionModelBuilder.EntitySet<RdbFItem>("RdbFItems");
            conventionModelBuilder.EntitySet<Revendeur>("Revendeurs");
            conventionModelBuilder.EntitySet<FactureF>("FactureFs");
            conventionModelBuilder.EntitySet<FactureFItem>("FactureFItems");
            conventionModelBuilder.EntitySet<Site>("Sites");
            conventionModelBuilder.EntitySet<ArticleSite>("ArticleSites");
            conventionModelBuilder.EntitySet<FakeFactureF>("FakeFactureFs");
            conventionModelBuilder.EntitySet<FakeFactureFItem>("FakeFactureFItems");
            conventionModelBuilder.EntitySet<StockMouvement>("StockMouvements");
            conventionModelBuilder.EntitySet<Depense>("Depenses");
            conventionModelBuilder.EntitySet<DepenseItem>("DepenseItems");
            conventionModelBuilder.EntitySet<TypeDepense>("TypeDepenses");
            
            //invoice & payment
            conventionModelBuilder.EntitySet<PaiementFacture>("PaiementFactures");
            conventionModelBuilder.EntitySet<PaiementFactureF>("PaiementFactureFs");

            /////////
            //conventionModelBuilder.Entity<Article>().Collection.Action("ArticlesGaz");


            /////////
            conventionModelBuilder.StructuralTypes
                .First(t => t.ClrType == typeof(Client))
                .AddProperty(typeof(Client).GetProperty("Solde"));
            conventionModelBuilder.StructuralTypes
                .First(t => t.ClrType == typeof(Client))
                .AddProperty(typeof(Client).GetProperty("SoldeFacture"));
            conventionModelBuilder.StructuralTypes
                .First(t => t.ClrType == typeof(Fournisseur))
                .AddProperty(typeof(Fournisseur).GetProperty("Solde"));
            conventionModelBuilder.StructuralTypes
               .First(t => t.ClrType == typeof(Fournisseur))
               .AddProperty(typeof(Fournisseur).GetProperty("SoldeFacture"));
            conventionModelBuilder.StructuralTypes
                .First(t => t.ClrType == typeof(Article))
                .AddProperty(typeof(Article).GetProperty("QteStockSum"));
            /////////


            return conventionModelBuilder.GetEdmModel();
        }
    }
}
