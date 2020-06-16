namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFactureF : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FactureFs", "IdTypePaiement", c => c.Guid());
            AlterColumn("dbo.FactureFs", "Ref", c => c.Int());
            CreateIndex("dbo.FactureFs", "IdTypePaiement");
            AddForeignKey("dbo.FactureFs", "IdTypePaiement", "dbo.TypePaiements", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FactureFs", "IdTypePaiement", "dbo.TypePaiements");
            DropIndex("dbo.FactureFs", new[] { "IdTypePaiement" });
            AlterColumn("dbo.FactureFs", "Ref", c => c.Int(nullable: false));
            DropColumn("dbo.FactureFs", "IdTypePaiement");
        }
    }
}
