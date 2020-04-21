namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class disabled_client_fourn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fournisseurs", "Disabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Clients", "Disabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "Disabled");
            DropColumn("dbo.Fournisseurs", "Disabled");
        }
    }
}
