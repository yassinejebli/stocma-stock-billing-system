namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Unique_site_name : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Sites", new[] { "Name" });
            CreateIndex("dbo.Sites", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sites", new[] { "Name" });
            CreateIndex("dbo.Sites", "Name");
        }
    }
}
