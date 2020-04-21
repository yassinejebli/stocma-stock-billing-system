namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class devis_TypeReglement_correct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devis", "TypeReglement", c => c.String());
            DropColumn("dbo.Devis", "TypeRegelement");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Devis", "TypeRegelement", c => c.String());
            DropColumn("dbo.Devis", "TypeReglement");
        }
    }
}
