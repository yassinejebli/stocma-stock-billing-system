namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Client_IsClientDivers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "IsClientDivers", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "IsClientDivers");
        }
    }
}
