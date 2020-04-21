namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class company_adresses_email : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "CompleteName", c => c.String());
            AddColumn("dbo.Companies", "AdresseSociete1", c => c.String());
            AddColumn("dbo.Companies", "AdresseSociete2", c => c.String());
            AddColumn("dbo.Companies", "AdresseSociete3", c => c.String());
            AddColumn("dbo.Companies", "AdresseSociete4", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "AdresseSociete4");
            DropColumn("dbo.Companies", "AdresseSociete3");
            DropColumn("dbo.Companies", "AdresseSociete2");
            DropColumn("dbo.Companies", "AdresseSociete1");
            DropColumn("dbo.Companies", "CompleteName");
        }
    }
}
