namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rgb_rgbitem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DgbItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IdDgb = c.Guid(nullable: false),
                        Qte = c.Single(nullable: false),
                        IdArticle = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.IdArticle, cascadeDelete: true)
                .ForeignKey("dbo.Dgbs", t => t.IdDgb, cascadeDelete: true)
                .Index(t => t.IdDgb)
                .Index(t => t.IdArticle);
            
            CreateTable(
                "dbo.Dgbs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Ref = c.Int(nullable: false),
                        NumBon = c.String(),
                        Date = c.DateTime(nullable: false),
                        IdClient = c.Guid(nullable: false),
                        User = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.IdClient, cascadeDelete: true)
                .Index(t => t.IdClient);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DgbItems", "IdDgb", "dbo.Dgbs");
            DropForeignKey("dbo.Dgbs", "IdClient", "dbo.Clients");
            DropForeignKey("dbo.DgbItems", "IdArticle", "dbo.Articles");
            DropIndex("dbo.Dgbs", new[] { "IdClient" });
            DropIndex("dbo.DgbItems", new[] { "IdArticle" });
            DropIndex("dbo.DgbItems", new[] { "IdDgb" });
            DropTable("dbo.Dgbs");
            DropTable("dbo.DgbItems");
        }
    }
}
