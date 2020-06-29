namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WillCascadeOnDelete_TypeDepense_Required : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DepenseItems", new[] { "IdTypeDepense" });
            AlterColumn("dbo.DepenseItems", "IdTypeDepense", c => c.Guid(nullable: false));
            CreateIndex("dbo.DepenseItems", "IdTypeDepense");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DepenseItems", new[] { "IdTypeDepense" });
            AlterColumn("dbo.DepenseItems", "IdTypeDepense", c => c.Guid());
            CreateIndex("dbo.DepenseItems", "IdTypeDepense");
        }
    }
}
