namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaiementFacture_BonAvoirC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaiementFactures", "IdBonAvoirC", c => c.Guid());
            CreateIndex("dbo.PaiementFactures", "IdBonAvoirC");
            AddForeignKey("dbo.PaiementFactures", "IdBonAvoirC", "dbo.BonAvoirCs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaiementFactures", "IdBonAvoirC", "dbo.BonAvoirCs");
            DropIndex("dbo.PaiementFactures", new[] { "IdBonAvoirC" });
            DropColumn("dbo.PaiementFactures", "IdBonAvoirC");
        }
    }
}
