﻿namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Client_CodeClient_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "CodeClient", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "CodeClient");
        }
    }
}
