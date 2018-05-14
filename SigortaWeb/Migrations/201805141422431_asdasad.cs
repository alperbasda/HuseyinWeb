namespace SigortaWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdasad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tables", "TableName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tables", "TableName");
        }
    }
}
