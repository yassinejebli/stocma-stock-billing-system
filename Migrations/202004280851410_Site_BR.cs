namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Site_BR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonReceptions", "IdSite", c => c.Int());
            CreateIndex("dbo.BonReceptions", "IdSite");
            AddForeignKey("dbo.BonReceptions", "IdSite", "dbo.Sites", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BonReceptions", "IdSite", "dbo.Sites");
            DropIndex("dbo.BonReceptions", new[] { "IdSite" });
            DropColumn("dbo.BonReceptions", "IdSite");
        }
    }
}
