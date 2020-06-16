namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BonAvoirC_Site : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonAvoirCs", "IdSite", c => c.Int());
            CreateIndex("dbo.BonAvoirCs", "IdSite");
            AddForeignKey("dbo.BonAvoirCs", "IdSite", "dbo.Sites", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BonAvoirCs", "IdSite", "dbo.Sites");
            DropIndex("dbo.BonAvoirCs", new[] { "IdSite" });
            DropColumn("dbo.BonAvoirCs", "IdSite");
        }
    }
}
