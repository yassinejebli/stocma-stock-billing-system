namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Devis_ClientName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devis", "ClientName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devis", "ClientName");
        }
    }
}
