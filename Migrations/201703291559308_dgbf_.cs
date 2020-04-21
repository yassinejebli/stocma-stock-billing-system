namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dgbf_ : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RetourBouteilleItems", "IdArticle", "dbo.Articles");
            DropForeignKey("dbo.RetourBouteilles", "IdClient", "dbo.Clients");
            DropForeignKey("dbo.RetourBouteilleItems", "IdRetourBouteille", "dbo.RetourBouteilles");
            DropIndex("dbo.RetourBouteilleItems", new[] { "IdRetourBouteille" });
            DropIndex("dbo.RetourBouteilleItems", new[] { "IdArticle" });
            DropIndex("dbo.RetourBouteilles", new[] { "IdClient" });
            CreateTable(
                "dbo.DgbFs",
                c => new
                    {
                        Id = c.Guid(nullable: false, defaultValue:Guid.NewGuid(),defaultValueSql:"newid()"),
                        Ref = c.Int(nullable: false),
                        NumBon = c.String(),
                        Date = c.DateTime(nullable: false),
                        IdFournisseur = c.Guid(nullable: false),
                        User = c.String(),
                        CinRcn = c.String(),
                        DatePaiement = c.String(),
                        ModeConsignation = c.String(),
                        TypeReglement = c.String(),
                        Banque = c.String(),
                        NumCheque = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fournisseurs", t => t.IdFournisseur, cascadeDelete: false)
                .Index(t => t.IdFournisseur);
            
            CreateTable(
                "dbo.DgbFItems",
                c => new
                    {
                        Id = c.Guid(nullable: false, defaultValue: Guid.NewGuid(), defaultValueSql: "newid()"),
                        IdDgbF = c.Guid(nullable: false),
                        Qte = c.Single(nullable: false),
                        Pu = c.Single(nullable: false),
                        TotalHT = c.Single(nullable: false),
                        IdArticle = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.IdArticle, cascadeDelete: false)
                .ForeignKey("dbo.DgbFs", t => t.IdDgbF, cascadeDelete: false)
                .Index(t => t.IdDgbF)
                .Index(t => t.IdArticle);
            
            DropTable("dbo.RetourBouteilleItems");
            DropTable("dbo.RetourBouteilles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RetourBouteilles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Ref = c.Int(nullable: false),
                        NumBon = c.String(),
                        Date = c.DateTime(nullable: false),
                        IdClient = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RetourBouteilleItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IdRetourBouteille = c.Guid(nullable: false),
                        Qte = c.Single(nullable: false),
                        IdArticle = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.DgbFs", "IdFournisseur", "dbo.Fournisseurs");
            DropForeignKey("dbo.DgbFItems", "IdDgbF", "dbo.DgbFs");
            DropForeignKey("dbo.DgbFItems", "IdArticle", "dbo.Articles");
            DropIndex("dbo.DgbFItems", new[] { "IdArticle" });
            DropIndex("dbo.DgbFItems", new[] { "IdDgbF" });
            DropIndex("dbo.DgbFs", new[] { "IdFournisseur" });
            DropTable("dbo.DgbFItems");
            DropTable("dbo.DgbFs");
            CreateIndex("dbo.RetourBouteilles", "IdClient");
            CreateIndex("dbo.RetourBouteilleItems", "IdArticle");
            CreateIndex("dbo.RetourBouteilleItems", "IdRetourBouteille");
            AddForeignKey("dbo.RetourBouteilleItems", "IdRetourBouteille", "dbo.RetourBouteilles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RetourBouteilles", "IdClient", "dbo.Clients", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RetourBouteilleItems", "IdArticle", "dbo.Articles", "Id", cascadeDelete: true);
        }
    }
}
