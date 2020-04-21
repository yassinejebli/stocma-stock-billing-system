namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bonlivraison_typeReglement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonLivraisons", "TypeReglement", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BonLivraisons", "TypeReglement");
        }
    }
}
