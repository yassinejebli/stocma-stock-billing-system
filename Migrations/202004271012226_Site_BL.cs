namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Site_BL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonLivraisons", "IdSite", c => c.Int());
            CreateIndex("dbo.BonLivraisons", "IdSite");
            AddForeignKey("dbo.BonLivraisons", "IdSite", "dbo.Sites", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BonLivraisons", "IdSite", "dbo.Sites");
            DropIndex("dbo.BonLivraisons", new[] { "IdSite" });
            DropColumn("dbo.BonLivraisons", "IdSite");
        }
    }
}
