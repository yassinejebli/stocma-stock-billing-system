namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sold_test_rollback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Solde", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "Solde");
        }
    }
}
