namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FA_TypePaiement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Factures", "IdTypePaiement", c => c.Guid());
            CreateIndex("dbo.Factures", "IdTypePaiement");
            AddForeignKey("dbo.Factures", "IdTypePaiement", "dbo.TypePaiements", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Factures", "IdTypePaiement", "dbo.TypePaiements");
            DropIndex("dbo.Factures", new[] { "IdTypePaiement" });
            DropColumn("dbo.Factures", "IdTypePaiement");
        }
    }
}
