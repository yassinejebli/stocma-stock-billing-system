namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clients", "Solde");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "Solde", c => c.Single(nullable: false));
        }
    }
}
