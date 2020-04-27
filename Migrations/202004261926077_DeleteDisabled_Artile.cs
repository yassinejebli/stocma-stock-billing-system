namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDisabled_Artile : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Articles", "Disabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "Disabled", c => c.Boolean(nullable: false));
        }
    }
}
