namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class article_factueFItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FactureFItems", "Article_Id", "dbo.Articles");
            DropIndex("dbo.FactureFItems", new[] { "Article_Id" });
            DropColumn("dbo.FactureFItems", "IdArticle");
            RenameColumn(table: "dbo.FactureFItems", name: "Article_Id", newName: "IdArticle");
            AlterColumn("dbo.FactureFItems", "IdArticle", c => c.Guid(nullable: false));
            CreateIndex("dbo.FactureFItems", "IdArticle");
            AddForeignKey("dbo.FactureFItems", "IdArticle", "dbo.Articles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FactureFItems", "IdArticle", "dbo.Articles");
            DropIndex("dbo.FactureFItems", new[] { "IdArticle" });
            AlterColumn("dbo.FactureFItems", "IdArticle", c => c.Guid());
            RenameColumn(table: "dbo.FactureFItems", name: "IdArticle", newName: "Article_Id");
            AddColumn("dbo.FactureFItems", "IdArticle", c => c.Guid(nullable: false));
            CreateIndex("dbo.FactureFItems", "Article_Id");
            AddForeignKey("dbo.FactureFItems", "Article_Id", "dbo.Articles", "Id");
        }
    }
}
