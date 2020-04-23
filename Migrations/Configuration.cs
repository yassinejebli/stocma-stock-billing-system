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
            if (!context.Settings.Any<Setting>())
            {
                context.Settings.AddRange((IEnumerable<Setting>)new Setting[2]
                {
                    new Setting()
                    {
                        Code = "1",
                        Name = "Remplissage manuel du numéro BL",
                        Afficher = 0
                    },
                    new Setting()
                    {
                        Code = "2",
                        Name = "Taille de la police",
                        Afficher = 14
                    }
                });
                context.SaveChanges();
            }
            else
            {
                if (
                    context.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "2"))
                        .FirstOrDefault<Setting>() == null)
                {
                    context.Settings.AddRange((IEnumerable<Setting>)new Setting[1]
                    {
                        new Setting()
                        {
                            Code = "2",
                            Name = "Taille de la police (Page)",
                            Afficher = 14
                        }
                    });
                    context.SaveChanges();
                }
                if (
                    context.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "3"))
                        .FirstOrDefault<Setting>() == null)
                {
                    context.Settings.AddRange((IEnumerable<Setting>)new Setting[1]
                    {
                        new Setting()
                        {
                            Code = "3",
                            Name = "Taille de la police (Impression)",
                            Afficher = 9
                        }
                    });
                    context.SaveChanges();
                }
                if (
                    context.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "5"))
                        .FirstOrDefault<Setting>() == null)
                {
                    context.Settings.AddRange((IEnumerable<Setting>)new Setting[1]
                    {
                        new Setting()
                        {
                            Code = "5",
                            Name = "Type Tableau (Impression)",
                            Afficher = 2
                        }
                    });
                    context.SaveChanges();
                }
                if (
                    context.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "6"))
                        .FirstOrDefault<Setting>() == null)
                    context.Settings.AddRange((IEnumerable<Setting>)new Setting[1]
                    {
                        new Setting()
                        {
                            Code = "6",
                            Name = "Coût unitaire moyen pondéré (C.U.M.P)",
                            Afficher = 0
                        }
                    });
                if (
                    context.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "7"))
                        .FirstOrDefault<Setting>() == null)
                {
                    context.Settings.AddRange((IEnumerable<Setting>)new Setting[1]
                    {
                        new Setting()
                        {
                            Code = "7",
                            Name = "Format BL Par défaut (0 : petit / 1 : grand)",
                            Afficher = 0
                        }
                    });
                    context.SaveChanges();
                }
                if (
                    context.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "8"))
                        .FirstOrDefault<Setting>() == null)
                {
                    context.Settings.AddRange((IEnumerable<Setting>)new Setting[1]
                    {
                        new Setting()
                        {
                            Code = "8",
                            Name = "Ordre Qte (0 : aprés désignation / 1 : avant designation)",
                            Afficher = 0
                        }
                    });
                    context.SaveChanges();
                }
                if (
                    context.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "9"))
                        .FirstOrDefault<Setting>() == null)
                {
                    context.Settings.AddRange((IEnumerable<Setting>)new Setting[1]
                    {
                        new Setting()
                        {
                            Code = "9",
                            Name = "Afficher Réference d'article",
                            Afficher = 0
                        }
                    });
                    context.SaveChanges();
                }
                if (
                    context.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "10"))
                        .FirstOrDefault<Setting>() == null)
                {
                    context.Settings.AddRange((IEnumerable<Setting>)new Setting[1]
                    {
                        new Setting()
                        {
                            Code = "10",
                            Name =
                                "0 : Scanner tous les articles puis entrer les quantitiés | 1 : Scanner un article par un et entrer la quantité",
                            Afficher = 0
                        }
                    });
                    context.SaveChanges();
                }
                if (
                    context.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(x => x.Code == "11"))
                        .FirstOrDefault<Setting>() == null)
                {
                    context.Settings.AddRange((IEnumerable<Setting>)new Setting[1]
                    {
                        new Setting()
                        {
                            Code = "11",
                            Name = "Afficher H.T (0:non | 1:oui)",
                            Afficher = 0
                        }
                    });
                    context.SaveChanges();
                }
            }
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
