namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Optional_Re : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FakeFactureFs", "Ref", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FakeFactureFs", "Ref", c => c.Int(nullable: false));
        }
    }
}
