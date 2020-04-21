namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paiementF_factureF : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PaiementFs", name: "FactureF_Id", newName: "IdFactureF");
            RenameIndex(table: "dbo.PaiementFs", name: "IX_FactureF_Id", newName: "IX_IdFactureF");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PaiementFs", name: "IX_IdFactureF", newName: "IX_FactureF_Id");
            RenameColumn(table: "dbo.PaiementFs", name: "IdFactureF", newName: "FactureF_Id");
        }
    }
}
