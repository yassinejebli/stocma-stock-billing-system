namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Datetime_null : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonLivraisons", "ModificationDate", c => c.DateTime());
            AddColumn("dbo.BonLivraisons", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.BonLivraisons", "ModificationDate", c => c.DateTime(nullable: false));
        }
    }
}
