namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BonReception_Optional_BonAvoir : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BonAvoirs", "IdBonReception", "dbo.BonReceptions");
            DropIndex("dbo.BonAvoirs", new[] { "IdBonReception" });
            AlterColumn("dbo.BonAvoirs", "IdBonReception", c => c.Guid());
            CreateIndex("dbo.BonAvoirs", "IdBonReception");
            AddForeignKey("dbo.BonAvoirs", "IdBonReception", "dbo.BonReceptions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BonAvoirs", "IdBonReception", "dbo.BonReceptions");
            DropIndex("dbo.BonAvoirs", new[] { "IdBonReception" });
            AlterColumn("dbo.BonAvoirs", "IdBonReception", c => c.Guid(nullable: false));
            CreateIndex("dbo.BonAvoirs", "IdBonReception");
            AddForeignKey("dbo.BonAvoirs", "IdBonReception", "dbo.BonReceptions", "Id", cascadeDelete: true);
        }
    }
}
