namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bonavoircItem_numBL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonAvoirCItems", "NumBL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BonAvoirCItems", "NumBL");
        }
    }
}
