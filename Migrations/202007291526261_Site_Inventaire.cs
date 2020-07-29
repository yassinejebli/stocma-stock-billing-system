namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Site_Inventaire : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventaires", "IdSite", c => c.Int(nullable: false));
            CreateIndex("dbo.Inventaires", "IdSite");
            AddForeignKey("dbo.Inventaires", "IdSite", "dbo.Sites", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventaires", "IdSite", "dbo.Sites");
            DropIndex("dbo.Inventaires", new[] { "IdSite" });
            DropColumn("dbo.Inventaires", "IdSite");
        }
    }
}
