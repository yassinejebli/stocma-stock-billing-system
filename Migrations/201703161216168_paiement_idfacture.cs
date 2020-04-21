namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paiement_idfacture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Paiements", "IdFacture", c => c.Guid());
            CreateIndex("dbo.Paiements", "IdFacture");
            AddForeignKey("dbo.Paiements", "IdFacture", "dbo.Factures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Paiements", "IdFacture", "dbo.Factures");
            DropIndex("dbo.Paiements", new[] { "IdFacture" });
            DropColumn("dbo.Paiements", "IdFacture");
        }
    }
}
