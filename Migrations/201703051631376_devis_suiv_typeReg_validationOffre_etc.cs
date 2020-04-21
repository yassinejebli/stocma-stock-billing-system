namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class devis_suiv_typeReg_validationOffre_etc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devis", "TypeRegelement", c => c.String());
            AddColumn("dbo.Devis", "DelaiLivrasion", c => c.String());
            AddColumn("dbo.Devis", "TransportExpedition", c => c.String());
            AddColumn("dbo.Devis", "ValiditeOffre", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devis", "ValiditeOffre");
            DropColumn("dbo.Devis", "TransportExpedition");
            DropColumn("dbo.Devis", "DelaiLivrasion");
            DropColumn("dbo.Devis", "TypeRegelement");
        }
    }
}
