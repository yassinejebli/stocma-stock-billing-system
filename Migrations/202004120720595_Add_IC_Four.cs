namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IC_Four : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fournisseurs", "ICE", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fournisseurs", "ICE");
        }
    }
}
