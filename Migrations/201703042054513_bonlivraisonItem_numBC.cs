namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bonlivraisonItem_numBC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonLivraisonItems", "NumBC", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BonLivraisonItems", "NumBC");
        }
    }
}
