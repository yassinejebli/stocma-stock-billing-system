namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class factureitem_numBL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FactureItems", "NumBL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FactureItems", "NumBL");
        }
    }
}
