namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categorie_famille_article_bosh : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Articles", "IdFamille", "dbo.Familles");
            DropIndex("dbo.Articles", new[] { "IdFamille" });
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false,defaultValue:Guid.NewGuid(),defaultValueSql:"newid()"),
                        Name = c.String(nullable: false, maxLength: 200),
                        IdFamille = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Familles", t => t.IdFamille)
                .Index(t => t.Name)
                .Index(t => t.IdFamille);
            
            AddColumn("dbo.Articles", "IdCategorie", c => c.Guid(defaultValue: Guid.NewGuid(), defaultValueSql: "newid()"));
            CreateIndex("dbo.Articles", "IdCategorie");
            AddForeignKey("dbo.Articles", "IdCategorie", "dbo.Categories", "Id");
            DropColumn("dbo.Articles", "IdFamille");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "IdFamille", c => c.Guid());
            DropForeignKey("dbo.Articles", "IdCategorie", "dbo.Categories");
            DropForeignKey("dbo.Categories", "IdFamille", "dbo.Familles");
            DropIndex("dbo.Categories", new[] { "IdFamille" });
            DropIndex("dbo.Categories", new[] { "Name" });
            DropIndex("dbo.Articles", new[] { "IdCategorie" });
            DropColumn("dbo.Articles", "IdCategorie");
            DropTable("dbo.Categories");
            CreateIndex("dbo.Articles", "IdFamille");
            AddForeignKey("dbo.Articles", "IdFamille", "dbo.Familles", "Id");
        }
    }
}
