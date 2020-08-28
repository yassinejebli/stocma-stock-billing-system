namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IventaireItem_Category : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventaireItems", "IdCategory", c => c.Guid());
            CreateIndex("dbo.InventaireItems", "IdCategory");
            AddForeignKey("dbo.InventaireItems", "IdCategory", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InventaireItems", "IdCategory", "dbo.Categories");
            DropIndex("dbo.InventaireItems", new[] { "IdCategory" });
            DropColumn("dbo.InventaireItems", "IdCategory");
        }
    }
}
