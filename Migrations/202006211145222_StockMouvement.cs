namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StockMouvement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockMouvements",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IdSiteFrom = c.Int(nullable: false),
                        IdSiteTo = c.Int(nullable: false),
                        Comment = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sites", t => t.IdSiteFrom)
                .ForeignKey("dbo.Sites", t => t.IdSiteTo)
                .Index(t => t.IdSiteFrom)
                .Index(t => t.IdSiteTo);
            
            CreateTable(
                "dbo.StockMouvementItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IdStockMouvement = c.Guid(nullable: false),
                        Qte = c.Single(nullable: false),
                        IdArticle = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.IdArticle, cascadeDelete: true)
                .ForeignKey("dbo.StockMouvements", t => t.IdStockMouvement, cascadeDelete: true)
                .Index(t => t.IdStockMouvement)
                .Index(t => t.IdArticle);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockMouvementItems", "IdStockMouvement", "dbo.StockMouvements");
            DropForeignKey("dbo.StockMouvementItems", "IdArticle", "dbo.Articles");
            DropForeignKey("dbo.StockMouvements", "IdSiteTo", "dbo.Sites");
            DropForeignKey("dbo.StockMouvements", "IdSiteFrom", "dbo.Sites");
            DropIndex("dbo.StockMouvementItems", new[] { "IdArticle" });
            DropIndex("dbo.StockMouvementItems", new[] { "IdStockMouvement" });
            DropIndex("dbo.StockMouvements", new[] { "IdSiteTo" });
            DropIndex("dbo.StockMouvements", new[] { "IdSiteFrom" });
            DropTable("dbo.StockMouvementItems");
            DropTable("dbo.StockMouvements");
        }
    }
}
