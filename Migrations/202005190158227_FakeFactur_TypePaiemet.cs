namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FakeFactur_TypePaiemet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FakeFactures", "IdTypePaiement", c => c.Guid());
            AddColumn("dbo.FakeFactures", "WithDiscount", c => c.Boolean(nullable: false));
            AddColumn("dbo.FakeFactureItems", "Discount", c => c.Single());
            AddColumn("dbo.FakeFactureItems", "PercentageDiscount", c => c.Boolean(nullable: false));
            CreateIndex("dbo.FakeFactures", "IdTypePaiement");
            AddForeignKey("dbo.FakeFactures", "IdTypePaiement", "dbo.TypePaiements", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FakeFactures", "IdTypePaiement", "dbo.TypePaiements");
            DropIndex("dbo.FakeFactures", new[] { "IdTypePaiement" });
            DropColumn("dbo.FakeFactureItems", "PercentageDiscount");
            DropColumn("dbo.FakeFactureItems", "Discount");
            DropColumn("dbo.FakeFactures", "WithDiscount");
            DropColumn("dbo.FakeFactures", "IdTypePaiement");
        }
    }
}
