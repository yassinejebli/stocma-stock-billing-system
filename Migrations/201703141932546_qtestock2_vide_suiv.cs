namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qtestock2_vide_suiv : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "QteStock2", c => c.Single(defaultValue:0,defaultValueSql:"0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "QteStock2");
        }
    }
}
