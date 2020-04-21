namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class factureitem_TypeReglement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Factures", "TypeReglement", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Factures", "TypeReglement");
        }
    }
}
