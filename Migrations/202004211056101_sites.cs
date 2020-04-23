namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleSites",
                c => new
                    {
                        IdSite = c.Int(nullable: false),
                        IdArticle = c.Guid(nullable: false),
                        QteStock = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdSite, t.IdArticle })
                .ForeignKey("dbo.Articles", t => t.IdArticle, cascadeDelete: true)
                .ForeignKey("dbo.Sites", t => t.IdSite, cascadeDelete: true)
                .Index(t => t.IdSite)
                .Index(t => t.IdArticle);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Address = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleSites", "IdSite", "dbo.Sites");
            DropForeignKey("dbo.ArticleSites", "IdArticle", "dbo.Articles");
            DropIndex("dbo.Sites", new[] { "Name" });
            DropIndex("dbo.ArticleSites", new[] { "IdArticle" });
            DropIndex("dbo.ArticleSites", new[] { "IdSite" });
            DropTable("dbo.Sites");
            DropTable("dbo.ArticleSites");
        }
    }
}
