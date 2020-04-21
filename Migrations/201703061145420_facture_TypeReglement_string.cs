namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class facture_TypeReglement_string : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Factures", "TypeReglement", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Factures", "TypeReglement", c => c.Int());
        }
    }
}
