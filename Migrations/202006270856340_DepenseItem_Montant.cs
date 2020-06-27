namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepenseItem_Montant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DepenseItems", "Montant", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DepenseItems", "Montant");
        }
    }
}
