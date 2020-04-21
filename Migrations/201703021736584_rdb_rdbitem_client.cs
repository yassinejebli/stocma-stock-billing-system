namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rdb_rdbitem_client : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rdbs", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.RdbItems", "Rdb_Id", "dbo.Rdbs");
            DropIndex("dbo.Rdbs", new[] { "Client_Id" });
            DropIndex("dbo.RdbItems", new[] { "Rdb_Id" });
            DropColumn("dbo.Rdbs", "IdClient");
            DropColumn("dbo.RdbItems", "IdRdb");
            RenameColumn(table: "dbo.Rdbs", name: "Client_Id", newName: "IdClient");
            RenameColumn(table: "dbo.RdbItems", name: "Rdb_Id", newName: "IdRdb");
            AlterColumn("dbo.Rdbs", "IdClient", c => c.Guid(nullable: false));
            AlterColumn("dbo.RdbItems", "IdRdb", c => c.Guid(nullable: false));
            CreateIndex("dbo.Rdbs", "IdClient");
            CreateIndex("dbo.RdbItems", "IdRdb");
            AddForeignKey("dbo.Rdbs", "IdClient", "dbo.Clients", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RdbItems", "IdRdb", "dbo.Rdbs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RdbItems", "IdRdb", "dbo.Rdbs");
            DropForeignKey("dbo.Rdbs", "IdClient", "dbo.Clients");
            DropIndex("dbo.RdbItems", new[] { "IdRdb" });
            DropIndex("dbo.Rdbs", new[] { "IdClient" });
            AlterColumn("dbo.RdbItems", "IdRdb", c => c.Guid());
            AlterColumn("dbo.Rdbs", "IdClient", c => c.Guid());
            RenameColumn(table: "dbo.RdbItems", name: "IdRdb", newName: "Rdb_Id");
            RenameColumn(table: "dbo.Rdbs", name: "IdClient", newName: "Client_Id");
            AddColumn("dbo.RdbItems", "IdRdb", c => c.Guid(nullable: false));
            AddColumn("dbo.Rdbs", "IdClient", c => c.Guid(nullable: false));
            CreateIndex("dbo.RdbItems", "Rdb_Id");
            CreateIndex("dbo.Rdbs", "Client_Id");
            AddForeignKey("dbo.RdbItems", "Rdb_Id", "dbo.Rdbs", "Id");
            AddForeignKey("dbo.Rdbs", "Client_Id", "dbo.Clients", "Id");
        }
    }
}
