namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WillCascadeOnDelete_Test_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DepenseItems", "IdTypeDepense", "dbo.TypeDepenses");
            AddForeignKey("dbo.DepenseItems", "IdTypeDepense", "dbo.TypeDepenses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DepenseItems", "IdTypeDepense", "dbo.TypeDepenses");
            AddForeignKey("dbo.DepenseItems", "IdTypeDepense", "dbo.TypeDepenses", "Id", cascadeDelete: true);
        }
    }
}
