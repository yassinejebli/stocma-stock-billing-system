namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rdbitem_article_relation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RdbItems", "Article_Id", "dbo.Articles");
            DropIndex("dbo.RdbItems", new[] { "Article_Id" });
            DropColumn("dbo.RdbItems", "IdArticle");
            RenameColumn(table: "dbo.RdbItems", name: "Article_Id", newName: "IdArticle");
            AlterColumn("dbo.RdbItems", "IdArticle", c => c.Guid(nullable: false));
            CreateIndex("dbo.RdbItems", "IdArticle");
            AddForeignKey("dbo.RdbItems", "IdArticle", "dbo.Articles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RdbItems", "IdArticle", "dbo.Articles");
            DropIndex("dbo.RdbItems", new[] { "IdArticle" });
            AlterColumn("dbo.RdbItems", "IdArticle", c => c.Guid());
            RenameColumn(table: "dbo.RdbItems", name: "IdArticle", newName: "Article_Id");
            AddColumn("dbo.RdbItems", "IdArticle", c => c.Guid(nullable: false));
            CreateIndex("dbo.RdbItems", "Article_Id");
            AddForeignKey("dbo.RdbItems", "Article_Id", "dbo.Articles", "Id");
        }
    }
}
