namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Site_BLItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonLivraisonItems", "IdSite", c => c.Int());
            CreateIndex("dbo.BonLivraisonItems", "IdSite");
            AddForeignKey("dbo.BonLivraisonItems", "IdSite", "dbo.Sites", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BonLivraisonItems", "IdSite", "dbo.Sites");
            DropIndex("dbo.BonLivraisonItems", new[] { "IdSite" });
            DropColumn("dbo.BonLivraisonItems", "IdSite");
        }
    }
}
