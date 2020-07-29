namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventaire : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InventaireItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IdArticle = c.Guid(nullable: false),
                        IdInvetaire = c.Guid(nullable: false),
                        QteStock = c.Single(nullable: false),
                        QteStockReel = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.IdArticle, cascadeDelete: true)
                .ForeignKey("dbo.Inventaires", t => t.IdInvetaire, cascadeDelete: true)
                .Index(t => t.IdArticle)
                .Index(t => t.IdInvetaire);
            
            CreateTable(
                "dbo.Inventaires",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Titre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InventaireItems", "IdInvetaire", "dbo.Inventaires");
            DropForeignKey("dbo.InventaireItems", "IdArticle", "dbo.Articles");
            DropIndex("dbo.InventaireItems", new[] { "IdInvetaire" });
            DropIndex("dbo.InventaireItems", new[] { "IdArticle" });
            DropTable("dbo.Inventaires");
            DropTable("dbo.InventaireItems");
        }
    }
}
