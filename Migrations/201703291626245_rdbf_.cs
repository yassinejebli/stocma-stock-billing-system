namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rdbf_ : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RdbFs",
                c => new
                    {
                        Id = c.Guid(nullable: false, defaultValue: Guid.NewGuid(), defaultValueSql: "newid()"),
                        Ref = c.Int(nullable: false),
                        NumBon = c.String(),
                        Date = c.DateTime(nullable: false),
                        IdFournisseur = c.Guid(nullable: false),
                        User = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fournisseurs", t => t.IdFournisseur, cascadeDelete: false)
                .Index(t => t.IdFournisseur);
            
            CreateTable(
                "dbo.RdbFItems",
                c => new
                    {
                        Id = c.Guid(nullable: false, defaultValue: Guid.NewGuid(), defaultValueSql: "newid()"),
                        IdRdbF = c.Guid(nullable: false),
                        Qte = c.Single(nullable: false),
                        IdArticle = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.IdArticle, cascadeDelete: false)
                .ForeignKey("dbo.RdbFs", t => t.IdRdbF, cascadeDelete: false)
                .Index(t => t.IdRdbF)
                .Index(t => t.IdArticle);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RdbFItems", "IdRdbF", "dbo.RdbFs");
            DropForeignKey("dbo.RdbFItems", "IdArticle", "dbo.Articles");
            DropForeignKey("dbo.RdbFs", "IdFournisseur", "dbo.Fournisseurs");
            DropIndex("dbo.RdbFItems", new[] { "IdArticle" });
            DropIndex("dbo.RdbFItems", new[] { "IdRdbF" });
            DropIndex("dbo.RdbFs", new[] { "IdFournisseur" });
            DropTable("dbo.RdbFItems");
            DropTable("dbo.RdbFs");
        }
    }
}
