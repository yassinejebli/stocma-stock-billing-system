namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FakeFactureF_TypePaiement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FakeFactureFs", "IdTypePaiement", c => c.Guid());
            CreateIndex("dbo.FakeFactureFs", "IdTypePaiement");
            AddForeignKey("dbo.FakeFactureFs", "IdTypePaiement", "dbo.TypePaiements", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FakeFactureFs", "IdTypePaiement", "dbo.TypePaiements");
            DropIndex("dbo.FakeFactureFs", new[] { "IdTypePaiement" });
            DropColumn("dbo.FakeFactureFs", "IdTypePaiement");
        }
    }
}
