namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaiementFactureF_FactureF : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaiementFactureFs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(),
                        IdFournisseur = c.Guid(nullable: false),
                        IdTypePaiement = c.Guid(nullable: false),
                        IdFactureF = c.Guid(),
                        Debit = c.Single(nullable: false),
                        Credit = c.Single(nullable: false),
                        Comment = c.String(),
                        DateEcheance = c.DateTime(),
                        EnCaisse = c.Boolean(),
                        Hide = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FactureFs", t => t.IdFactureF)
                .ForeignKey("dbo.Fournisseurs", t => t.IdFournisseur, cascadeDelete: true)
                .ForeignKey("dbo.TypePaiements", t => t.IdTypePaiement, cascadeDelete: true)
                .Index(t => t.IdFournisseur)
                .Index(t => t.IdTypePaiement)
                .Index(t => t.IdFactureF);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaiementFactureFs", "IdTypePaiement", "dbo.TypePaiements");
            DropForeignKey("dbo.PaiementFactureFs", "IdFournisseur", "dbo.Fournisseurs");
            DropForeignKey("dbo.PaiementFactureFs", "IdFactureF", "dbo.FactureFs");
            DropIndex("dbo.PaiementFactureFs", new[] { "IdFactureF" });
            DropIndex("dbo.PaiementFactureFs", new[] { "IdTypePaiement" });
            DropIndex("dbo.PaiementFactureFs", new[] { "IdFournisseur" });
            DropTable("dbo.PaiementFactureFs");
        }
    }
}
