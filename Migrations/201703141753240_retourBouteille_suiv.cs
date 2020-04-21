namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class retourBouteille_suiv : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RetourBouteilleItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IdRetourBouteille = c.Guid(nullable: false),
                        Qte = c.Single(nullable: false),
                        IdArticle = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.IdArticle, cascadeDelete: false)
                .ForeignKey("dbo.RetourBouteilles", t => t.IdRetourBouteille, cascadeDelete: false)
                .Index(t => t.IdRetourBouteille)
                .Index(t => t.IdArticle);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.IdClient, cascadeDelete: false)
                .Index(t => t.IdClient);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RetourBouteilleItems", "IdRetourBouteille", "dbo.RetourBouteilles");
            DropForeignKey("dbo.RetourBouteilles", "IdClient", "dbo.Clients");
            DropForeignKey("dbo.RetourBouteilleItems", "IdArticle", "dbo.Articles");
            DropIndex("dbo.RetourBouteilles", new[] { "IdClient" });
            DropIndex("dbo.RetourBouteilleItems", new[] { "IdArticle" });
            DropIndex("dbo.RetourBouteilleItems", new[] { "IdRetourBouteille" });
            DropTable("dbo.RetourBouteilles");
            DropTable("dbo.RetourBouteilleItems");
        }
    }
}
