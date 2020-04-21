namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Datetime_null : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BonLivraisons", "ModificationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BonLivraisons", "ModificationDate", c => c.DateTime(nullable: false));
        }
    }
}
