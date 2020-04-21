namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class factureitem_numBC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FactureItems", "NumBC", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FactureItems", "NumBC");
        }
    }
}
