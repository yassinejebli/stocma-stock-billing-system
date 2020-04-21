namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class avoir_casse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonAvoirCItems", "Casse", c => c.Boolean(nullable: false,defaultValue:false,defaultValueSql:"0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BonAvoirCItems", "Casse");
        }
    }
}
