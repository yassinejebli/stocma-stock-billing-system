namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Article_BarCode_MaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "BarCode", c => c.String(maxLength: 14));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articles", "BarCode", c => c.String());
        }
    }
}
