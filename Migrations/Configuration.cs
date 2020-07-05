// Decompiled with JetBrains decompiler
// Type: WebApplication1.Migrations.Configuration
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using WebApplication1.Auth;
using WebApplication1.DATA;
using WebApplication1.Models;
using WebGrease.Css.Extensions;

namespace WebApplication1.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<MySaniSoftContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MySaniSoftContext context)
        {
            

            //TODO: remove asap
            var articlesTVANull = context.Articles.Where(x => x.TVA == null);
            if (articlesTVANull.Count() > 0)
            {
                articlesTVANull.ForEach(x => x.TVA = 20);
                context.SaveChanges();
            }

            var articlesFactureTVANull = context.ArticleFactures.Where(x => x.TVA == null);
            if (articlesFactureTVANull.Count() > 0)
            {
                articlesFactureTVANull.ForEach(x => x.TVA = 20);
                context.SaveChanges();
            }

            var bonLivraisonItems = context.BonLivraisonItems.Where(x => x.PA == 0);

            if (bonLivraisonItems.Count() > 0)
            {
                bonLivraisonItems.ForEach(x => x.PA = x.Article.PA);
                context.SaveChanges();
            }

            var bonAvoirCItems = context.BonAvoirCItems.Where(x => x.PA == 0);

            if (bonAvoirCItems.Count() > 0)
            {
                bonAvoirCItems.ForEach(x => x.PA = x.Article.PA);
                context.SaveChanges();
            }


            //article sites
            if (context.Sites.FirstOrDefault() == null)
            {
                var mainSite = new Site
                {
                    Id = 1,
                    Name = "Magasin 1",
                    ArticleSites = new List<ArticleSite>()
                };
                context.Sites.Add(mainSite);

                foreach (var article in context.Articles)
                {
                    mainSite.ArticleSites.Add(new ArticleSite
                    {
                        Article = article,
                        QteStock = article.QteStock,
                        Site = mainSite
                    });
                }

                context.SaveChanges();
            }

            var bonLivraisons = context.BonLivraisons.Where(x => x.IdSite == null);
            if (bonLivraisons.Count() > 0)
            {
                foreach (var b in bonLivraisons)
                {
                    b.IdSite = 1;
                }
                context.SaveChanges();
            }

            var bonAvoirVentes = context.BonAvoirCs.Where(x => x.IdSite == null);
            if (bonAvoirVentes.Count() > 0)
            {
                foreach (var b in bonAvoirVentes)
                {
                    b.IdSite = 1;
                }
                context.SaveChanges();
            }

            var devises = context.Devises.Where(x => x.IdSite == null);
            if (devises.Count() > 0)
            {
                foreach (var b in devises)
                {
                    b.IdSite = 1;
                }
                context.SaveChanges();
            }

            var bonReceptions = context.BonReceptions.Where(x => x.IdSite == null);
            if (bonReceptions.Count() > 0)
            {
                foreach (var b in bonReceptions)
                {
                    b.IdSite = 1;
                }
                context.SaveChanges();
            }

            var factures = context.Factures.Where(x => x.IdSite == null);
            if (factures.Count() > 0)
            {
                foreach (var f in factures)
                {
                    f.IdSite = 1;
                }
                context.SaveChanges();
            }

            if (!context.Familles.Any())
            {
                context.Familles.AddRange((IEnumerable<Famille>)new Famille[1]
                {
                    new Famille()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Autre famille"
                    }
                });
                context.SaveChanges();
            }
            if (!context.Companies.Any<Company>())
            {
                context.Companies.AddRange((IEnumerable<Company>)new Company[1]
                {
                    new Company() {Id = Guid.NewGuid()}
                });
                context.SaveChanges();
            }
            if (!context.Clients.Any<Client>())
            {
                context.Clients.AddRange((IEnumerable<Client>)new Client[1]
                {
                    new Client()
                    {
                        Id = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"),
                        Name = "Client Divers",
                        DateCreation = DateTime.Now
                    }
                });
                context.SaveChanges();
            }
            if (!context.TypeDepences.Any<TypeDepence>())
            {
                context.TypeDepences.AddRange((IEnumerable<TypeDepence>)new TypeDepence[4]
                {
                    new TypeDepence()
                    {
                        Id = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc39"),
                        Name = "Transport"
                    },
                    new TypeDepence()
                    {
                        Id = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc38"),
                        Name = "Paiement pour un employé"
                    },
                    new TypeDepence()
                    {
                        Id = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc37"),
                        Name = "Repas"
                    },
                    new TypeDepence()
                    {
                        Id = new Guid("45c8b294-3a63-487c-821e-70bf4f9bdc44"),
                        Name = "Gazoil"
                    }
                });
                context.SaveChanges();
            }

            //context.Database.ExecuteSqlCommand("DELETE DepenseItems");
            //context.Database.ExecuteSqlCommand("DELETE Depenses");
            //context.Database.ExecuteSqlCommand("DELETE TypeDepenses");
            //context.SaveChanges();
            if (!context.TypeDepenses.Any())
            {
                context.TypeDepences.ForEach(x =>
                {
                    var TypeDepense = new TypeDepense
                    {
                        Id = x.Id,
                        Name = x.Name
                    };

                    context.TypeDepenses.Add(TypeDepense);
                });
                context.SaveChanges();
            }


            if (!context.Depenses.Any())
            {
                context.Depences.ForEach(x =>
                {
                    var Depense = new Depense
                    {
                        Id = Guid.NewGuid(),
                        Date = x.Date,
                        Titre = x.Comment,
                    };
                    context.DepenseItems.Add(new DepenseItem
                    {

                        Id = Guid.NewGuid(),
                        IdDepense = Depense.Id,
                        Montant = x.Montant,
                        Name = x.Comment,
                        IdTypeDepense = x.IdTypeDepence
                    });
                    context.Depenses.Add(Depense);
                });
                context.SaveChanges();
            }


            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Settings");

            var devisDiscount = context.Settings.Where(x => x.Code == "devis_discount")
                    .FirstOrDefault();

            if (
                devisDiscount == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "devis_discount",
                    Name = "Remise"
                });
            }

            var devisValidity = context.Settings.Where(x => x.Code == "devis_validity")
                    .FirstOrDefault();

            if (
                devisValidity == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "devis_validity",
                    Name = "Validité de l'offre"
                });
            }

            var devisPayment = context.Settings.Where(x => x.Code == "devis_payment")
                    .FirstOrDefault();

            if (
                devisPayment == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "devis_payment",
                    Name = "Mode de paiement",
                    Enabled = true
                });
            }

            var devisTransport = context.Settings.Where(x => x.Code == "devis_transport")
                    .FirstOrDefault();

            if (
                devisTransport == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "devis_transport",
                    Name = "Transport / Expédition"
                });
            }

            var devisDeliveryTime = context.Settings.Where(x => x.Code == "devis_delivery_time")
                    .FirstOrDefault();

            if (
                devisDeliveryTime == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "devis_delivery_time",
                    Name = "Délai de livraision"
                });
            }

            ///BL
            var bonLivraisonDiscount = context.Settings.Where(x => x.Code == "bl_discount")
                    .FirstOrDefault();

            if (
                bonLivraisonDiscount == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "bl_discount",
                    Name = "Remise"
                });
            }

            var bonLivraisonPayment = context.Settings.Where(x => x.Code == "bl_payment")
                    .FirstOrDefault();

            if (
                bonLivraisonPayment == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "bl_payment",
                    Name = "Mode de paiement",
                    Enabled = true
                });
            }

            //FA client
            var factureDiscount = context.Settings.Where(x => x.Code == "fa_discount")
                    .FirstOrDefault();

            if (
                factureDiscount == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "fa_discount",
                    Name = "Remise"
                });
            }

            var facturePayment = context.Settings.Where(x => x.Code == "fa_payment")
                    .FirstOrDefault();

            if (
                facturePayment == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "fa_payment",
                    Name = "Mode de paiement",
                    Enabled = true
                });
            }

            var factureCheque = context.Settings.Where(x => x.Code == "fa_cheque")
                   .FirstOrDefault();

            if (
                facturePayment == null)
            {
                context.Settings.Add(new Setting
                {
                    Code = "fa_cheque",
                    Name = "Numéro de chèque/effet",
                    Enabled = true
                });
            }
            ///////////////////////////////////////////////// end Settings

            if (!context.TypePaiements.Any<TypePaiement>())
            {
                context.TypePaiements.AddRange((IEnumerable<TypePaiement>)new TypePaiement[11]
                {
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2"),
                        Name = "Espéce"
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3"),
                        Name = "Chéque"
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeece1"),
                        Name = "Impayé"
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4"),
                        Name = "Effet"
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb5"),
                        Name = "Remise"
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb6"),
                        Name = "Vente"
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb7"),
                        Name = "Achat"
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb8"),
                        Name = "Avoir"
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecc1"),
                        Name = "Versement"
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4"),
                        Name = "Remboursement",
                        IsDebit = true
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecc9"),
                        Name = "Ancien Solde",
                        IsDebit = true

                    }
                });
                context.SaveChanges();
            }
            else
            {
                var TypePaiementCheque = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb3"));
                if (TypePaiementCheque != null)
                    TypePaiementCheque.IsBankRelated = true;

                var TypePaiementEffet = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb4"));
                if (TypePaiementEffet != null)
                    TypePaiementEffet.IsBankRelated = true;

                var TypePaiementImpaye = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeece1"));
                if (TypePaiementImpaye != null)
                {
                    TypePaiementImpaye.IsBankRelated = true;
                    TypePaiementImpaye.IsDebit = true;
                }

                var TypePaiementRemboursement = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4"));
                if (TypePaiementRemboursement != null)
                {
                    TypePaiementRemboursement.IsDebit = true;
                }

                var TypePaiementAncien = context.TypePaiements.Find(new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecc9"));
                if (TypePaiementAncien != null)
                    TypePaiementAncien.IsDebit = true;

                context.SaveChanges();
            }


        }
    }
}
