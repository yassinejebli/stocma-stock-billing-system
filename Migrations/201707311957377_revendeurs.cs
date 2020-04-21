namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revendeurs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Revendeurs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            AddColumn("dbo.Clients", "IdRevendeur", c => c.Guid());
            CreateIndex("dbo.Clients", "IdRevendeur");
            AddForeignKey("dbo.Clients", "IdRevendeur", "dbo.Revendeurs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "IdRevendeur", "dbo.Revendeurs");
            DropIndex("dbo.Revendeurs", new[] { "Name" });
            DropIndex("dbo.Clients", new[] { "IdRevendeur" });
            DropColumn("dbo.Clients", "IdRevendeur");
            DropTable("dbo.Revendeurs");
        }
    }
}
