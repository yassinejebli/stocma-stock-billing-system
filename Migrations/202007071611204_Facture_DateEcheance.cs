namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Facture_DateEcheance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Factures", "DateEcheance", c => c.DateTime());
            AddColumn("dbo.FakeFactures", "DateEcheance", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FakeFactures", "DateEcheance");
            DropColumn("dbo.Factures", "DateEcheance");
        }
    }
}
