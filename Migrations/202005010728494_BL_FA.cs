namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BL_FA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonLivraisons", "IdFacture", c => c.Guid());
            AddColumn("dbo.Factures", "IdSite", c => c.Int());
            AddColumn("dbo.Factures", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Factures", "ModificationDate", c => c.DateTime());
            CreateIndex("dbo.BonLivraisons", "IdFacture");
            CreateIndex("dbo.Factures", "IdSite");
            AddForeignKey("dbo.Factures", "IdSite", "dbo.Sites", "Id");
            AddForeignKey("dbo.BonLivraisons", "IdFacture", "dbo.Factures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BonLivraisons", "IdFacture", "dbo.Factures");
            DropForeignKey("dbo.Factures", "IdSite", "dbo.Sites");
            DropIndex("dbo.Factures", new[] { "IdSite" });
            DropIndex("dbo.BonLivraisons", new[] { "IdFacture" });
            DropColumn("dbo.Factures", "ModificationDate");
            DropColumn("dbo.Factures", "CreationDate");
            DropColumn("dbo.Factures", "IdSite");
            DropColumn("dbo.BonLivraisons", "IdFacture");
        }
    }
}
