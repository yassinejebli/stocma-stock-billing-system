namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Devis_Site : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devis", "IdSite", c => c.Int());
            CreateIndex("dbo.Devis", "IdSite");
            AddForeignKey("dbo.Devis", "IdSite", "dbo.Sites", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devis", "IdSite", "dbo.Sites");
            DropIndex("dbo.Devis", new[] { "IdSite" });
            DropColumn("dbo.Devis", "IdSite");
        }
    }
}
