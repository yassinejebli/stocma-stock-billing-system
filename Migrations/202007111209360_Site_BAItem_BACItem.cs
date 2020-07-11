namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Site_BAItem_BACItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonAvoirItems", "IdSite", c => c.Int());
            AddColumn("dbo.BonAvoirCItems", "IdSite", c => c.Int());
            CreateIndex("dbo.BonAvoirItems", "IdSite");
            CreateIndex("dbo.BonAvoirCItems", "IdSite");
            AddForeignKey("dbo.BonAvoirCItems", "IdSite", "dbo.Sites", "Id");
            AddForeignKey("dbo.BonAvoirItems", "IdSite", "dbo.Sites", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BonAvoirItems", "IdSite", "dbo.Sites");
            DropForeignKey("dbo.BonAvoirCItems", "IdSite", "dbo.Sites");
            DropIndex("dbo.BonAvoirCItems", new[] { "IdSite" });
            DropIndex("dbo.BonAvoirItems", new[] { "IdSite" });
            DropColumn("dbo.BonAvoirCItems", "IdSite");
            DropColumn("dbo.BonAvoirItems", "IdSite");
        }
    }
}
