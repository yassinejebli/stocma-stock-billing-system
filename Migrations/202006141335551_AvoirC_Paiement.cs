namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AvoirC_Paiement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Paiements", "IdBonAvoirC", c => c.Guid());
            CreateIndex("dbo.Paiements", "IdBonAvoirC");
            AddForeignKey("dbo.Paiements", "IdBonAvoirC", "dbo.BonAvoirCs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Paiements", "IdBonAvoirC", "dbo.BonAvoirCs");
            DropIndex("dbo.Paiements", new[] { "IdBonAvoirC" });
            DropColumn("dbo.Paiements", "IdBonAvoirC");
        }
    }
}
