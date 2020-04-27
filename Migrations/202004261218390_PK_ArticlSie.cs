namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PK_ArticlSie : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ArticleSites");
            AddPrimaryKey("dbo.ArticleSites", new[] { "IdArticle", "IdSite" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ArticleSites");
            AddPrimaryKey("dbo.ArticleSites", new[] { "IdSite", "IdArticle" });
        }
    }
}
