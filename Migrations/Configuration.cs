// Decompiled with JetBrains decompiler
// Type: WebApplication1.Migrations.Configuration
// Assembly: WebApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C822783-F9C5-42E8-8CB3-732AAA2F6F0F
// Assembly location: D:\PROJECT\SANI SOFT\WebApplication1\WebApplication1\bin\WebApplication1.dll

using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using WebApplication1.DATA;

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
            //article sites
            if (context.Sites.FirstOrDefault() == null)
            {
                var mainSite = new Site
                {
                    Id = 1,
                    Name = "Magasin 1",
                    ArticleSites = new List<ArticleSite>()
                };

                foreach (var article in context.Articles)
                {
                    mainSite.ArticleSites.Add(new ArticleSite
                    {
                        Article = article,
                        QteStock = article.QteStock,
                        Site = mainSite
                    });
                }

                context.Sites.Add(mainSite);
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


            context.Database.ExecuteSqlCommand("TRUNCATE TABLE Settings");

            var devisDiscount = context.Settings.Where(x => x.Code == "devis_discount")
                    .FirstOrDefault();

            if (
                devisDiscount == null)
            {
                context.Settings.Add(new Setting
                {
                    Id = 1,
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
                    Enabled = false,
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
                    Enabled = false,
                    Name = "Mode de paiement"
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
                    Enabled = false,
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
                    Enabled = false,
                    Name = "Délai de livraision"
                });
            }
            var bonLivraisonDiscount = context.Settings.Where(x => x.Code == "bl_discount")
                    .FirstOrDefault();

            if (
                bonLivraisonDiscount == null)
            {
                context.Settings.Add(new Setting
                {
                    Id = 2,
                    Code = "bl_discount",
                    Disabled = true,
                    Name = "Remise"
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
                        Name = "Retour d'argent"
                    },
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecc9"),
                        Name = "ANCIEN"
                    }
                });
                context.SaveChanges();
            }
            else
            {
                if (
                    context.TypePaiements.Where<TypePaiement>(
                        (Expression<Func<TypePaiement, bool>>)
                        (x => x.Id == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeece1"))).FirstOrDefault<TypePaiement>() ==
                    null)
                {
                    context.TypePaiements.AddRange((IEnumerable<TypePaiement>)new TypePaiement[1]
                    {
                        new TypePaiement()
                        {
                            Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeece1"),
                            Name = "Impayé"
                        }
                    });
                    context.SaveChanges();
                }
                if (
                    context.TypePaiements.Where<TypePaiement>(
                        (Expression<Func<TypePaiement, bool>>)
                        (x => x.Id == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4"))).FirstOrDefault<TypePaiement>() !=
                    null)
                    return;
                context.TypePaiements.AddRange((IEnumerable<TypePaiement>)new TypePaiement[1]
                {
                    new TypePaiement()
                    {
                        Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeeca4"),
                        Name = "Retour d'argent"
                    }
                });



                //categories ss categories
                if (
                    context.Categories.Where(
                        x => x.Id == new Guid("399d159e-9ce0-4fcc-957a-08a65bbeec00")).FirstOrDefault() != null)
                {
                    context.Categories.AddRange(new Categorie[1]
                    {
                        new Categorie()
                        {
                            Id = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeec00"),
                            Name = "Gaz"
                        }
                    });
                }

                context.SaveChanges();
            }



        }
    }
}
