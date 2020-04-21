namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class total_dgbitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DgbItems", "Pu", c => c.Single(nullable: false));
            AddColumn("dbo.DgbItems", "TotalHT", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DgbItems", "TotalHT");
            DropColumn("dbo.DgbItems", "Pu");
        }
    }
}
