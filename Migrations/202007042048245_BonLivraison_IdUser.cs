namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BonLivraison_IdUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonLivraisons", "IdUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BonLivraisons", "IdUser");
        }
    }
}
