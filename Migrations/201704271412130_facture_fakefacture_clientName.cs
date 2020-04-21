namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class facture_fakefacture_clientName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FakeFactures", "ClientName", c => c.String());
            AddColumn("dbo.Factures", "ClientName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Factures", "ClientName");
            DropColumn("dbo.FakeFactures", "ClientName");
        }
    }
}
