namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_Depense_structure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Depenses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Titre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepenseItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IdTypeDepense = c.Guid(nullable: false),
                        IdDepense = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depenses", t => t.IdDepense, cascadeDelete: true)
                .ForeignKey("dbo.TypeDepenses", t => t.IdTypeDepense, cascadeDelete: true)
                .Index(t => t.IdTypeDepense)
                .Index(t => t.IdDepense);
            
            CreateTable(
                "dbo.TypeDepenses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DepenseItems", "IdTypeDepense", "dbo.TypeDepenses");
            DropForeignKey("dbo.DepenseItems", "IdDepense", "dbo.Depenses");
            DropIndex("dbo.DepenseItems", new[] { "IdDepense" });
            DropIndex("dbo.DepenseItems", new[] { "IdTypeDepense" });
            DropTable("dbo.TypeDepenses");
            DropTable("dbo.DepenseItems");
            DropTable("dbo.Depenses");
        }
    }
}
