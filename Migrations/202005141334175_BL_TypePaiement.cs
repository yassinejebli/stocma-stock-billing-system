namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BL_TypePaiement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonLivraisons", "IdTypePaiement", c => c.Guid());
            CreateIndex("dbo.BonLivraisons", "IdTypePaiement");
            AddForeignKey("dbo.BonLivraisons", "IdTypePaiement", "dbo.TypePaiements", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BonLivraisons", "IdTypePaiement", "dbo.TypePaiements");
            DropIndex("dbo.BonLivraisons", new[] { "IdTypePaiement" });
            DropColumn("dbo.BonLivraisons", "IdTypePaiement");
        }
    }
}
