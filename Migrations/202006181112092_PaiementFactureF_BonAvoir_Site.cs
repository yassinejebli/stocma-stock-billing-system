namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaiementFactureF_BonAvoir_Site : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonAvoirs", "IdSite", c => c.Int());
            AddColumn("dbo.PaiementFs", "IdBonAvoir", c => c.Guid());
            AddColumn("dbo.PaiementFactureFs", "IdBonAvoir", c => c.Guid());
            AlterColumn("dbo.BonAvoirs", "NumBon", c => c.String());
            CreateIndex("dbo.BonAvoirs", "IdSite");
            CreateIndex("dbo.PaiementFs", "IdBonAvoir");
            CreateIndex("dbo.PaiementFactureFs", "IdBonAvoir");
            AddForeignKey("dbo.PaiementFs", "IdBonAvoir", "dbo.BonAvoirs", "Id");
            AddForeignKey("dbo.PaiementFactureFs", "IdBonAvoir", "dbo.BonAvoirs", "Id");
            AddForeignKey("dbo.BonAvoirs", "IdSite", "dbo.Sites", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BonAvoirs", "IdSite", "dbo.Sites");
            DropForeignKey("dbo.PaiementFactureFs", "IdBonAvoir", "dbo.BonAvoirs");
            DropForeignKey("dbo.PaiementFs", "IdBonAvoir", "dbo.BonAvoirs");
            DropIndex("dbo.PaiementFactureFs", new[] { "IdBonAvoir" });
            DropIndex("dbo.PaiementFs", new[] { "IdBonAvoir" });
            DropIndex("dbo.BonAvoirs", new[] { "IdSite" });
            AlterColumn("dbo.BonAvoirs", "NumBon", c => c.String(nullable: false));
            DropColumn("dbo.PaiementFactureFs", "IdBonAvoir");
            DropColumn("dbo.PaiementFs", "IdBonAvoir");
            DropColumn("dbo.BonAvoirs", "IdSite");
        }
    }
}
