namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Paiement_ModificationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Paiements", "ModificationDate", c => c.DateTime());
            AddColumn("dbo.Paiements", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Paiements", "CreationDate");
            DropColumn("dbo.Paiements", "ModificationDate");
        }
    }
}
