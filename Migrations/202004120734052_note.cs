namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class note : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonLivraisons", "Note", c => c.String());
            AddColumn("dbo.Devis", "Note", c => c.String());
            AddColumn("dbo.Factures", "Note", c => c.String());
            AddColumn("dbo.FakeFactures", "Note", c => c.String());
            AddColumn("dbo.BonCommandes", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BonCommandes", "Note");
            DropColumn("dbo.FakeFactures", "Note");
            DropColumn("dbo.Factures", "Note");
            DropColumn("dbo.Devis", "Note");
            DropColumn("dbo.BonLivraisons", "Note");
        }
    }
}
