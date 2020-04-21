namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qteemballage_pleineetvide : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "QteEmballageVide", c => c.Single(defaultValue:0,defaultValueSql:"0"));
            AddColumn("dbo.Articles", "QteEmballagePleine", c => c.Single(defaultValue: 0, defaultValueSql: "0"));
            DropColumn("dbo.Articles", "QteStock2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "QteStock2", c => c.Single());
            DropColumn("dbo.Articles", "QteEmballagePleine");
            DropColumn("dbo.Articles", "QteEmballageVide");
        }
    }
}
