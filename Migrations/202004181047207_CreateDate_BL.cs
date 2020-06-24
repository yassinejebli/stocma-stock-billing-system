namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDate_BL : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Clients", "Discriminator");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Clients", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
