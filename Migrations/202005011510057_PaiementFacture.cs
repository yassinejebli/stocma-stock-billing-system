namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaiementFacture : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaiementFactures",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        IdClient = c.Guid(nullable: false),
                        IdTypePaiement = c.Guid(nullable: false),
                        IdFacture = c.Guid(),
                        Debit = c.Single(nullable: false),
                        Credit = c.Single(nullable: false),
                        Comment = c.String(),
                        DateEcheance = c.DateTime(),
                        EnCaisse = c.Boolean(),
                        Hide = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.IdClient, cascadeDelete: true)
                .ForeignKey("dbo.Factures", t => t.IdFacture)
                .ForeignKey("dbo.TypePaiements", t => t.IdTypePaiement, cascadeDelete: true)
                .Index(t => t.IdClient)
                .Index(t => t.IdTypePaiement)
                .Index(t => t.IdFacture);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaiementFactures", "IdTypePaiement", "dbo.TypePaiements");
            DropForeignKey("dbo.PaiementFactures", "IdFacture", "dbo.Factures");
            DropForeignKey("dbo.PaiementFactures", "IdClient", "dbo.Clients");
            DropIndex("dbo.PaiementFactures", new[] { "IdFacture" });
            DropIndex("dbo.PaiementFactures", new[] { "IdTypePaiement" });
            DropIndex("dbo.PaiementFactures", new[] { "IdClient" });
            DropTable("dbo.PaiementFactures");
        }
    }
}
