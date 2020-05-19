// Decompiled with JetBrains decompiler
// Type: WebApplication1.DATA.MySaniSoftContext
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using WebApplication1.Migrations;

namespace WebApplication1.DATA
{
    public class MySaniSoftContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }

        public DbSet<Revendeur> Revendeurs { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Fournisseur> Fournisseurs { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<BonReception> BonReceptions { get; set; }

        public DbSet<BonReceptionItem> BonReceptionItems { get; set; }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleSite> ArticleSites { get; set; }
        public DbSet<Site> Sites { get; set; }

        public DbSet<Famille> Familles { get; set; }

        public DbSet<BonLivraison> BonLivraisons { get; set; }

        public DbSet<Devis> Devises { get; set; }

        public DbSet<BonLivraisonItem> BonLivraisonItems { get; set; }

        public DbSet<DevisItem> DevisItems { get; set; }

        public DbSet<BonAvoir> BonAvoirs { get; set; }

        public DbSet<BonAvoirItem> BonAvoirItems { get; set; }

        public DbSet<BonAvoirC> BonAvoirCs { get; set; }

        public DbSet<BonAvoirCItem> BonAvoirCItems { get; set; }

        public DbSet<BonCommande> BonCommandes { get; set; }

        public DbSet<BonCommandeItem> BonCommandeItems { get; set; }

        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<PaiementFacture> PaiementFactures { get; set; }

        public DbSet<TypePaiement> TypePaiements { get; set; }

        public DbSet<PaiementF> PaiementFs { get; set; }

        public DbSet<Facture> Factures { get; set; }

        public DbSet<FactureItem> FactureItems { get; set; }

        public DbSet<Depence> Depences { get; set; }

        public DbSet<TypeDepence> TypeDepences { get; set; }

        public DbSet<JournalConnexion> JournalConnexions { get; set; }

        public DbSet<ArticleFacture> ArticleFactures { get; set; }

        public DbSet<FakeFacture> FakeFactures { get; set; }

        public DbSet<FakeFactureItem> FakeFactureItems { get; set; }

        public DbSet<FakeFactureF> FakeFacturesF { get; set; }

        public DbSet<FakeFactureFItem> FakeFactureFItems { get; set; }

        public DbSet<Tarif> Tarifs { get; set; }

        public DbSet<TarifItem> TarifItems { get; set; }
        public DbSet<FactureF> FactureFs { get; set; }
        public DbSet<FactureFItem> FactureFItems { get; set; }


        static MySaniSoftContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MySaniSoftContext, Configuration>());
        }

        public MySaniSoftContext()
          : base("MySaniSoftContext")
        {

            //var ensureDLLIsCopied =
            //   System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            ////////Article sites
            modelBuilder.Entity<Site>().HasKey(t => t.Id);
            modelBuilder.Entity<ArticleSite>().HasKey(t => new { t.IdSite, t.IdArticle });
            modelBuilder.Entity<ArticleSite>().HasRequired(x => x.Site).WithMany(x => x.ArticleSites).HasForeignKey(x => x.IdSite).WillCascadeOnDelete(true);
            modelBuilder.Entity<ArticleSite>().HasRequired(x => x.Article).WithMany(x => x.ArticleSites).HasForeignKey(x => x.IdArticle).WillCascadeOnDelete(true);
            
            //modelBuilder.Entity<Article>().HasMany(x => x.ArticleSites).WithRequired().HasForeignKey(x => x.IdArticle).WillCascadeOnDelete(true);
            //modelBuilder.Entity<Site>().HasMany(x => x.ArticleSites).WithRequired().HasForeignKey(x => x.IdSite).WillCascadeOnDelete(true);
            ////////

            modelBuilder.Entity<Article>().HasKey(t => t.Id);
            modelBuilder.Entity<Article>()
                .HasOptional(t => t.Categorie)
                .WithMany(t => t.Articles)
                .HasForeignKey(d => d.IdCategorie);

            modelBuilder.Entity<Categorie>().HasKey(t => t.Id);
            modelBuilder.Entity<Categorie>()
                .HasOptional(t => t.Famille)
                .WithMany(t => t.Categories)
                .HasForeignKey(d => d.IdFamille);

            modelBuilder.Entity<Rdb>().HasKey(t => t.Id);
            modelBuilder.Entity<RdbItem>().HasKey(t => t.Id);
            modelBuilder.Entity<RdbItem>()
                .HasRequired(t => t.Rdb)
                .WithMany(t => t.RdbItems)
                .HasForeignKey(d => d.IdRdb);

            modelBuilder.Entity<Revendeur>().HasKey(t => t.Id);
            modelBuilder.Entity<Client>().HasKey(t => t.Id);
            modelBuilder.Entity<Client>()
                .HasOptional(t => t.Revendeur)
                .WithMany(t => t.Clients)
                .HasForeignKey(d => d.IdRevendeur);

            modelBuilder.Entity<RdbItem>()
                .HasRequired(t => t.Article)
                .WithMany(t => t.RdbItems)
                .HasForeignKey(d => d.IdArticle);

            modelBuilder.Entity<Rdb>()
               .HasRequired(t => t.Client)
               .WithMany(t => t.Rdbs)
               .HasForeignKey(d => d.IdClient);

            modelBuilder.Entity<Paiement>()
              .HasOptional(t => t.Facture)
              .WithMany(t => t.Paiements)
              .HasForeignKey(d => d.IdFacture);


            modelBuilder.Entity<Dgb>().HasKey(t => t.Id);
            modelBuilder.Entity<DgbItem>().HasKey(t => t.Id);
            modelBuilder.Entity<Dgb>()
               .HasRequired(t => t.Client)
               .WithMany(t => t.Dgbs)
               .HasForeignKey(d => d.IdClient);

            modelBuilder.Entity<DgbItem>()
               .HasRequired(t => t.Dgb)
               .WithMany(t => t.DgbItems)
               .HasForeignKey(d => d.IdDgb);

            modelBuilder.Entity<DgbItem>()
              .HasRequired(t => t.Article)
              .WithMany(t => t.DgbItems)
              .HasForeignKey(d => d.IdArticle);


            //DGBF


            modelBuilder.Entity<DgbF>().HasKey(t => t.Id);
            modelBuilder.Entity<DgbFItem>().HasKey(t => t.Id);
            modelBuilder.Entity<DgbF>()
               .HasRequired(t => t.Fournisseur)
               .WithMany(t => t.DgbFs)
               .HasForeignKey(d => d.IdFournisseur);

            modelBuilder.Entity<DgbFItem>()
               .HasRequired(t => t.DgbF)
               .WithMany(t => t.DgbFItems)
               .HasForeignKey(d => d.IdDgbF);

            modelBuilder.Entity<DgbFItem>()
              .HasRequired(t => t.Article)
              .WithMany(t => t.DgbFItems)
              .HasForeignKey(d => d.IdArticle);
            //RDBF

            modelBuilder.Entity<RdbF>().HasKey(t => t.Id);
            modelBuilder.Entity<RdbFItem>().HasKey(t => t.Id);
            modelBuilder.Entity<RdbFItem>()
                .HasRequired(t => t.RdbF)
                .WithMany(t => t.RdbFItems)
                .HasForeignKey(d => d.IdRdbF);

            modelBuilder.Entity<RdbFItem>()
                .HasRequired(t => t.Article)
                .WithMany(t => t.RdbFItems)
                .HasForeignKey(d => d.IdArticle);

            modelBuilder.Entity<RdbF>()
               .HasRequired(t => t.Fournisseur)
               .WithMany(t => t.RdbFs)
               .HasForeignKey(d => d.IdFournisseur);

            modelBuilder.Entity<BonReceptionItem>()
              .HasRequired<Article>((Expression<Func<BonReceptionItem, Article>>)(t => t.Article))
              .WithMany((Expression<Func<Article, ICollection<BonReceptionItem>>>)(t => t.BonReceptionItems))
              .HasForeignKey<Guid>((Expression<Func<BonReceptionItem, Guid>>)(d => d.IdArticle));
            modelBuilder.Entity<BonReceptionItem>().HasKey<Guid>((Expression<Func<BonReceptionItem, Guid>>)(t => t.Id));
            modelBuilder.Entity<BonReceptionItem>()
                .HasRequired<BonReception>((Expression<Func<BonReceptionItem, BonReception>>)(t => t.BonReception))
                .WithMany((Expression<Func<BonReception, ICollection<BonReceptionItem>>>)(t => t.BonReceptionItems))
                .HasForeignKey<Guid>((Expression<Func<BonReceptionItem, Guid>>)(d => d.IdBonReception));
            modelBuilder.Entity<BonReception>().HasKey<Guid>((Expression<Func<BonReception, Guid>>)(t => t.Id));
            modelBuilder.Entity<BonReception>()
                .HasRequired<Fournisseur>((Expression<Func<BonReception, Fournisseur>>)(t => t.Fournisseur))
                .WithMany((Expression<Func<Fournisseur, ICollection<BonReception>>>)(t => t.BonReceptions))
                .HasForeignKey<Guid>((Expression<Func<BonReception, Guid>>)(d => d.IdFournisseur));

            //Facture fournisseur
            modelBuilder.Entity<FactureF>().HasKey<Guid>((Expression<Func<FactureF, Guid>>)(t => t.Id));

            modelBuilder.Entity<FactureF>()
                .HasRequired<Fournisseur>((Expression<Func<FactureF, Fournisseur>>)(t => t.Fournisseur))
                .WithMany((Expression<Func<Fournisseur, ICollection<FactureF>>>)(t => t.FactureFs))
                .HasForeignKey<Guid>((Expression<Func<FactureF, Guid>>)(d => d.IdFournisseur));

            modelBuilder.Entity<BonReception>()
                .HasOptional<FactureF>((Expression<Func<BonReception, FactureF>>)(t => t.FactureF))
                .WithMany((Expression<Func<FactureF, ICollection<BonReception>>>)(t => t.BonReceptions))
                .HasForeignKey(d => d.IdFactureF);



            ///--------------------------------------site - bl
            modelBuilder.Entity<BonLivraison>()
                .HasOptional(t => t.Site)
                .WithMany(t => t.BonLivraisons)
                .HasForeignKey(d => d.IdSite);
            ///

            ///--------------------------------------site - br
            modelBuilder.Entity<BonReception>()
                .HasOptional(t => t.Site)
                .WithMany(t => t.BonReceptions)
                .HasForeignKey(d => d.IdSite);
            ///

            ///--------------------------------------site - devis
            modelBuilder.Entity<Devis>()
                .HasOptional(t => t.Site)
                .WithMany(t => t.Devises)
                .HasForeignKey(d => d.IdSite);
            ///


            modelBuilder.Entity<FactureFItem>().HasKey<Guid>((Expression<Func<FactureFItem, Guid>>)(t => t.Id));

            modelBuilder.Entity<FactureFItem>()
                .HasRequired<FactureF>((Expression<Func<FactureFItem, FactureF>>)(t => t.FactureF))
                .WithMany((Expression<Func<FactureF, ICollection<FactureFItem>>>)(t => t.FactureFItems))
                .HasForeignKey<Guid>((Expression<Func<FactureFItem, Guid>>)(d => d.IdFactureF));

            modelBuilder.Entity<FactureFItem>()
              .HasRequired<Article>((Expression<Func<FactureFItem, Article>>)(t => t.Article))
              .WithMany((Expression<Func<Article, ICollection<FactureFItem>>>)(t => t.FactureFItems))
              .HasForeignKey<Guid>((Expression<Func<FactureFItem, Guid>>)(d => d.IdArticle));
            /////////

            modelBuilder.Entity<BonLivraison>().HasKey<Guid>((Expression<Func<BonLivraison, Guid>>)(t => t.Id));
            modelBuilder.Entity<BonLivraison>()
                .HasRequired<Client>((Expression<Func<BonLivraison, Client>>)(t => t.Client))
                .WithMany((Expression<Func<Client, ICollection<BonLivraison>>>)(t => t.BonLivraisons))
                .HasForeignKey<Guid>((Expression<Func<BonLivraison, Guid>>)(d => d.IdClient));


            modelBuilder.Entity<BonLivraison>()
               .HasOptional(t => t.TypePaiement)
               .WithMany(t => t.BonLivraisons)
               .HasForeignKey(d => d.IdTypePaiement);


            modelBuilder.Entity<BonLivraisonItem>().HasKey<Guid>((Expression<Func<BonLivraisonItem, Guid>>)(t => t.Id));
            modelBuilder.Entity<BonLivraisonItem>()
                .HasRequired<BonLivraison>((Expression<Func<BonLivraisonItem, BonLivraison>>)(t => t.BonLivraison))
                .WithMany((Expression<Func<BonLivraison, ICollection<BonLivraisonItem>>>)(t => t.BonLivraisonItems))
                .HasForeignKey<Guid>((Expression<Func<BonLivraisonItem, Guid>>)(d => d.IdBonLivraison));
            modelBuilder.Entity<BonLivraisonItem>()
                .HasRequired<Article>((Expression<Func<BonLivraisonItem, Article>>)(t => t.Article))
                .WithMany((Expression<Func<Article, ICollection<BonLivraisonItem>>>)(t => t.BonLivraisonItems))
                .HasForeignKey<Guid>((Expression<Func<BonLivraisonItem, Guid>>)(d => d.IdArticle));

            //-------------------------Facture
            modelBuilder.Entity<Facture>().HasKey<Guid>((Expression<Func<Facture, Guid>>)(t => t.Id));

            modelBuilder.Entity<Facture>()
               .HasOptional(t => t.TypePaiement)
               .WithMany(t => t.Factures)
               .HasForeignKey(d => d.IdTypePaiement);

            modelBuilder.Entity<Facture>()
                .HasOptional(t => t.Site)
                .WithMany(t => t.Factures)
                .HasForeignKey(d => d.IdSite);

            modelBuilder.Entity<BonLivraison>()
                .HasOptional(t => t.Facture)
                .WithMany(t => t.BonLivraisons)
                .HasForeignKey(d => d.IdFacture);

            modelBuilder.Entity<Facture>()
                .HasRequired<Client>((Expression<Func<Facture, Client>>)(t => t.Client))
                .WithMany((Expression<Func<Client, ICollection<Facture>>>)(t => t.Factures))
                .HasForeignKey<Guid>((Expression<Func<Facture, Guid>>)(d => d.IdClient));
            modelBuilder.Entity<Facture>()
                .HasOptional<BonLivraison>((Expression<Func<Facture, BonLivraison>>)(t => t.BonLivraison))
                .WithMany((Expression<Func<BonLivraison, ICollection<Facture>>>)(t => t.Factures))
                .HasForeignKey<Guid?>((Expression<Func<Facture, Guid?>>)(d => d.IdBonLivraison));
            modelBuilder.Entity<FactureItem>().HasKey<Guid>((Expression<Func<FactureItem, Guid>>)(t => t.Id));
            modelBuilder.Entity<FactureItem>()
                .HasRequired<Facture>((Expression<Func<FactureItem, Facture>>)(t => t.Facture))
                .WithMany((Expression<Func<Facture, ICollection<FactureItem>>>)(t => t.FactureItems))
                .HasForeignKey<Guid>((Expression<Func<FactureItem, Guid>>)(d => d.IdFacture));
            modelBuilder.Entity<FactureItem>()
                .HasRequired<Article>((Expression<Func<FactureItem, Article>>)(t => t.Article))
                .WithMany((Expression<Func<Article, ICollection<FactureItem>>>)(t => t.FactureItems))
                .HasForeignKey<Guid>((Expression<Func<FactureItem, Guid>>)(d => d.IdArticle));


            modelBuilder.Entity<FakeFactureF>().HasKey<Guid>((Expression<Func<FakeFactureF, Guid>>)(t => t.Id));

            modelBuilder.Entity<FakeFacture>()
              .HasOptional(t => t.TypePaiement)
              .WithMany(t => t.FakeFactures)
              .HasForeignKey(d => d.IdTypePaiement);

            modelBuilder.Entity<FakeFactureF>()
                .HasRequired<Fournisseur>((Expression<Func<FakeFactureF, Fournisseur>>)(t => t.Fournisseur))
                .WithMany((Expression<Func<Fournisseur, ICollection<FakeFactureF>>>)(t => t.FakeFactureFs))
                .HasForeignKey<Guid>((Expression<Func<FakeFactureF, Guid>>)(d => d.IdFournisseur));
            modelBuilder.Entity<FakeFactureFItem>().HasKey<Guid>((Expression<Func<FakeFactureFItem, Guid>>)(t => t.Id));
            modelBuilder.Entity<FakeFactureFItem>()
                .HasRequired<FakeFactureF>((Expression<Func<FakeFactureFItem, FakeFactureF>>)(t => t.FakeFactureF))
                .WithMany((Expression<Func<FakeFactureF, ICollection<FakeFactureFItem>>>)(t => t.FakeFactureFItems))
                .HasForeignKey<Guid>((Expression<Func<FakeFactureFItem, Guid>>)(d => d.IdFakeFactureF));
            modelBuilder.Entity<FakeFactureFItem>()
                .HasRequired<ArticleFacture>((Expression<Func<FakeFactureFItem, ArticleFacture>>)(t => t.ArticleFacture))
                .WithMany((Expression<Func<ArticleFacture, ICollection<FakeFactureFItem>>>)(t => t.FakeFactureFItems))
                .HasForeignKey<Guid>((Expression<Func<FakeFactureFItem, Guid>>)(d => d.IdArticleFacture));



            modelBuilder.Entity<FakeFacture>().HasKey<Guid>((Expression<Func<FakeFacture, Guid>>)(t => t.Id));
            modelBuilder.Entity<FakeFacture>()
                .HasRequired<Client>((Expression<Func<FakeFacture, Client>>)(t => t.Client))
                .WithMany((Expression<Func<Client, ICollection<FakeFacture>>>)(t => t.FakeFactures))
                .HasForeignKey<Guid>((Expression<Func<FakeFacture, Guid>>)(d => d.IdClient));
            modelBuilder.Entity<FakeFactureItem>().HasKey<Guid>((Expression<Func<FakeFactureItem, Guid>>)(t => t.Id));
            modelBuilder.Entity<FakeFactureItem>()
                .HasRequired<FakeFacture>((Expression<Func<FakeFactureItem, FakeFacture>>)(t => t.FakeFacture))
                .WithMany((Expression<Func<FakeFacture, ICollection<FakeFactureItem>>>)(t => t.FakeFactureItems))
                .HasForeignKey<Guid>((Expression<Func<FakeFactureItem, Guid>>)(d => d.IdFakeFacture));
            modelBuilder.Entity<FakeFactureItem>()
                .HasRequired<ArticleFacture>((Expression<Func<FakeFactureItem, ArticleFacture>>)(t => t.ArticleFacture))
                .WithMany((Expression<Func<ArticleFacture, ICollection<FakeFactureItem>>>)(t => t.FakeFactureItems))
                .HasForeignKey<Guid>((Expression<Func<FakeFactureItem, Guid>>)(d => d.IdArticleFacture));



            modelBuilder.Entity<DevisItem>().HasKey<Guid>((Expression<Func<DevisItem, Guid>>)(t => t.Id));
            modelBuilder.Entity<DevisItem>()
                .HasRequired<Devis>((Expression<Func<DevisItem, Devis>>)(t => t.Devis))
                .WithMany((Expression<Func<Devis, ICollection<DevisItem>>>)(t => t.DevisItems))
                .HasForeignKey<Guid>((Expression<Func<DevisItem, Guid>>)(d => d.IdDevis));
            modelBuilder.Entity<DevisItem>()
                .HasRequired<Article>((Expression<Func<DevisItem, Article>>)(t => t.Article))
                .WithMany((Expression<Func<Article, ICollection<DevisItem>>>)(t => t.DevisItems))
                .HasForeignKey<Guid>((Expression<Func<DevisItem, Guid>>)(d => d.IdArticle));
            modelBuilder.Entity<Devis>().HasKey<Guid>((Expression<Func<Devis, Guid>>)(t => t.Id));

            modelBuilder.Entity<Devis>()
               .HasOptional(t => t.TypePaiement)
               .WithMany(t => t.Devises)
               .HasForeignKey(d => d.IdTypePaiement);

            modelBuilder.Entity<Devis>()
                .HasRequired<Client>((Expression<Func<Devis, Client>>)(t => t.Client))
                .WithMany((Expression<Func<Client, ICollection<Devis>>>)(t => t.Devises))
                .HasForeignKey<Guid>((Expression<Func<Devis, Guid>>)(d => d.IdClient));
            modelBuilder.Entity<BonAvoirItem>().HasKey<Guid>((Expression<Func<BonAvoirItem, Guid>>)(t => t.Id));
            modelBuilder.Entity<BonAvoirItem>()
                .HasRequired<BonAvoir>((Expression<Func<BonAvoirItem, BonAvoir>>)(t => t.BonAvoir))
                .WithMany((Expression<Func<BonAvoir, ICollection<BonAvoirItem>>>)(t => t.BonAvoirItems))
                .HasForeignKey<Guid>((Expression<Func<BonAvoirItem, Guid>>)(d => d.IdBonAvoir));
            modelBuilder.Entity<BonAvoirItem>()
                .HasRequired<Article>((Expression<Func<BonAvoirItem, Article>>)(t => t.Article))
                .WithMany((Expression<Func<Article, ICollection<BonAvoirItem>>>)(t => t.BonAvoirItems))
                .HasForeignKey<Guid>((Expression<Func<BonAvoirItem, Guid>>)(d => d.IdArticle));
            modelBuilder.Entity<BonAvoir>().HasKey<Guid>((Expression<Func<BonAvoir, Guid>>)(t => t.Id));
            modelBuilder.Entity<BonAvoir>()
                .HasRequired<Fournisseur>((Expression<Func<BonAvoir, Fournisseur>>)(t => t.Fournisseur))
                .WithMany((Expression<Func<Fournisseur, ICollection<BonAvoir>>>)(t => t.BonAvoirs))
                .HasForeignKey<Guid>((Expression<Func<BonAvoir, Guid>>)(d => d.IdFournisseur));
            modelBuilder.Entity<BonAvoir>()
                .HasRequired<BonReception>((Expression<Func<BonAvoir, BonReception>>)(t => t.BonReception))
                .WithMany((Expression<Func<BonReception, ICollection<BonAvoir>>>)(t => t.BonAvoirs))
                .HasForeignKey<Guid>((Expression<Func<BonAvoir, Guid>>)(d => d.IdBonReception));
            modelBuilder.Entity<BonAvoirCItem>().HasKey<Guid>((Expression<Func<BonAvoirCItem, Guid>>)(t => t.Id));
            modelBuilder.Entity<BonAvoirCItem>()
                .HasRequired<BonAvoirC>((Expression<Func<BonAvoirCItem, BonAvoirC>>)(t => t.BonAvoirC))
                .WithMany((Expression<Func<BonAvoirC, ICollection<BonAvoirCItem>>>)(t => t.BonAvoirCItems))
                .HasForeignKey<Guid>((Expression<Func<BonAvoirCItem, Guid>>)(d => d.IdBonAvoirC));
            modelBuilder.Entity<BonAvoirCItem>()
                .HasRequired<Article>((Expression<Func<BonAvoirCItem, Article>>)(t => t.Article))
                .WithMany((Expression<Func<Article, ICollection<BonAvoirCItem>>>)(t => t.BonAvoirCItems))
                .HasForeignKey<Guid>((Expression<Func<BonAvoirCItem, Guid>>)(d => d.IdArticle));
            modelBuilder.Entity<BonAvoirC>().HasKey<Guid>((Expression<Func<BonAvoirC, Guid>>)(t => t.Id));
            modelBuilder.Entity<BonAvoirC>()
                .HasRequired<Client>((Expression<Func<BonAvoirC, Client>>)(t => t.Client))
                .WithMany((Expression<Func<Client, ICollection<BonAvoirC>>>)(t => t.BonAvoirCs))
                .HasForeignKey<Guid>((Expression<Func<BonAvoirC, Guid>>)(d => d.IdClient));
            modelBuilder.Entity<BonAvoirC>()
                .HasOptional<BonLivraison>((Expression<Func<BonAvoirC, BonLivraison>>)(t => t.BonLivraison))
                .WithMany((Expression<Func<BonLivraison, ICollection<BonAvoirC>>>)(t => t.BonAvoirCs))
                .HasForeignKey<Guid?>((Expression<Func<BonAvoirC, Guid?>>)(d => d.IdBonLivraison));
            modelBuilder.Entity<BonCommandeItem>().HasKey<Guid>((Expression<Func<BonCommandeItem, Guid>>)(t => t.Id));
            modelBuilder.Entity<BonCommandeItem>()
                .HasRequired<BonCommande>((Expression<Func<BonCommandeItem, BonCommande>>)(t => t.BonCommande))
                .WithMany((Expression<Func<BonCommande, ICollection<BonCommandeItem>>>)(t => t.BonCommandeItems))
                .HasForeignKey<Guid>((Expression<Func<BonCommandeItem, Guid>>)(d => d.IdBonCommande));
            modelBuilder.Entity<BonCommandeItem>()
                .HasRequired<Article>((Expression<Func<BonCommandeItem, Article>>)(t => t.Article))
                .WithMany((Expression<Func<Article, ICollection<BonCommandeItem>>>)(t => t.BonCommandeItems))
                .HasForeignKey<Guid>((Expression<Func<BonCommandeItem, Guid>>)(d => d.IdArticle));
            modelBuilder.Entity<BonCommande>().HasKey<Guid>((Expression<Func<BonCommande, Guid>>)(t => t.Id));
            modelBuilder.Entity<BonCommande>()
                .HasRequired<Fournisseur>((Expression<Func<BonCommande, Fournisseur>>)(t => t.Fournisseur))
                .WithMany((Expression<Func<Fournisseur, ICollection<BonCommande>>>)(t => t.BonCommandes))
                .HasForeignKey<Guid>((Expression<Func<BonCommande, Guid>>)(d => d.IdFournisseur));
            modelBuilder.Entity<Paiement>().HasKey<Guid>((Expression<Func<Paiement, Guid>>)(t => t.Id));
            modelBuilder.Entity<TypePaiement>().HasKey<Guid>((Expression<Func<TypePaiement, Guid>>)(t => t.Id));
            modelBuilder.Entity<PaiementFacture>().HasKey(t => t.Id);


            modelBuilder.Entity<Paiement>()
                .HasRequired<Client>((Expression<Func<Paiement, Client>>)(t => t.Client))
                .WithMany((Expression<Func<Client, ICollection<Paiement>>>)(t => t.Paiements))
                .HasForeignKey<Guid>((Expression<Func<Paiement, Guid>>)(d => d.IdClient));



            modelBuilder.Entity<PaiementFacture>()
                .HasRequired(t => t.Client)
                .WithMany(t => t.PaiementFactures)
                .HasForeignKey(d => d.IdClient);

            modelBuilder.Entity<PaiementFacture>()
                .HasRequired(t => t.TypePaiement)
                .WithMany(t => t.PaiementFactures)
                .HasForeignKey(d => d.IdTypePaiement);

            modelBuilder.Entity<PaiementFacture>()
               .HasOptional(t => t.Facture)
               .WithMany(t => t.PaiementFactures)
               .HasForeignKey(d => d.IdFacture);

            modelBuilder.Entity<Paiement>()
                .HasOptional<BonLivraison>((Expression<Func<Paiement, BonLivraison>>)(t => t.BonLivraison))
                .WithMany((Expression<Func<BonLivraison, ICollection<Paiement>>>)(t => t.Paiements))
                .HasForeignKey<Guid?>((Expression<Func<Paiement, Guid?>>)(d => d.IdBonLivraison));
            modelBuilder.Entity<Paiement>()
                .HasRequired<TypePaiement>((Expression<Func<Paiement, TypePaiement>>)(t => t.TypePaiement))
                .WithMany((Expression<Func<TypePaiement, ICollection<Paiement>>>)(t => t.Paiements))
                .HasForeignKey<Guid>((Expression<Func<Paiement, Guid>>)(d => d.IdTypePaiement));
            modelBuilder.Entity<PaiementF>().HasKey<Guid>((Expression<Func<PaiementF, Guid>>)(t => t.Id));
            modelBuilder.Entity<PaiementF>()
                .HasRequired<Fournisseur>((Expression<Func<PaiementF, Fournisseur>>)(t => t.Fournisseur))
                .WithMany((Expression<Func<Fournisseur, ICollection<PaiementF>>>)(t => t.PaiementFs))
                .HasForeignKey<Guid>((Expression<Func<PaiementF, Guid>>)(d => d.IdFournisseur));
            modelBuilder.Entity<PaiementF>()
                .HasOptional<BonReception>((Expression<Func<PaiementF, BonReception>>)(t => t.BonReception))
                .WithMany((Expression<Func<BonReception, ICollection<PaiementF>>>)(t => t.PaiementFs))
                .HasForeignKey<Guid?>((Expression<Func<PaiementF, Guid?>>)(d => d.IdBonReception));
            modelBuilder.Entity<PaiementF>()
              .HasOptional<FactureF>((Expression<Func<PaiementF, FactureF>>)(t => t.FactureF))
              .WithMany((Expression<Func<FactureF, ICollection<PaiementF>>>)(t => t.PaiementFs))
              .HasForeignKey<Guid?>((Expression<Func<PaiementF, Guid?>>)(d => d.IdFactureF));
            modelBuilder.Entity<PaiementF>()
              .HasRequired<TypePaiement>((Expression<Func<PaiementF, TypePaiement>>)(t => t.TypePaiement))
              .WithMany((Expression<Func<TypePaiement, ICollection<PaiementF>>>)(t => t.PaiementFs))
              .HasForeignKey<Guid>((Expression<Func<PaiementF, Guid>>)(d => d.IdTypePaiement));
            modelBuilder.Entity<Depence>().HasKey<Guid>((Expression<Func<Depence, Guid>>)(t => t.Id));
            modelBuilder.Entity<TypeDepence>().HasKey<Guid>((Expression<Func<TypeDepence, Guid>>)(t => t.Id));
            modelBuilder.Entity<Depence>()
                .HasRequired<TypeDepence>((Expression<Func<Depence, TypeDepence>>)(t => t.TypeDepence))
                .WithMany((Expression<Func<TypeDepence, ICollection<Depence>>>)(t => t.Depences))
                .HasForeignKey<Guid>((Expression<Func<Depence, Guid>>)(d => d.IdTypeDepence));
            modelBuilder.Entity<Tarif>().HasKey<Guid>((Expression<Func<Tarif, Guid>>)(t => t.Id));
            modelBuilder.Entity<TarifItem>().HasKey<Guid>((Expression<Func<TarifItem, Guid>>)(t => t.Id));
            modelBuilder.Entity<TarifItem>()
                .HasRequired<Tarif>((Expression<Func<TarifItem, Tarif>>)(t => t.Tarif))
                .WithMany((Expression<Func<Tarif, ICollection<TarifItem>>>)(t => t.TarifItems))
                .HasForeignKey<Guid>((Expression<Func<TarifItem, Guid>>)(d => d.IdTarif));
            modelBuilder.Entity<TarifItem>()
                .HasRequired<Article>((Expression<Func<TarifItem, Article>>)(t => t.Article))
                .WithMany((Expression<Func<Article, ICollection<TarifItem>>>)(t => t.TarifItems))
                .HasForeignKey<Guid>((Expression<Func<TarifItem, Guid>>)(d => d.IdArticle));
        }

        public System.Data.Entity.DbSet<WebApplication1.DATA.Categorie> Categories { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.DATA.Rdb> Rdbs { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.DATA.RdbItem> RdbItems { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.DATA.Dgb> Dgbs { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.DATA.DgbItem> DgbItems { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.DATA.DgbF> DgbFs { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.DATA.DgbFItem> DgbFItems { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.DATA.RdbF> RdbFs { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.DATA.RdbFItem> RdbFItems { get; set; }
    }
}
