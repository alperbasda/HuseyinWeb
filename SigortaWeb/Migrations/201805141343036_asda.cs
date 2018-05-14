namespace SigortaWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tarih1 = c.String(),
                        Tarih2 = c.String(),
                        Tarih3 = c.String(),
                        Tarih4 = c.String(),
                        Tarih5 = c.String(),
                        Sirket1 = c.String(),
                        Sirket2 = c.String(),
                        Sirket3 = c.String(),
                        Sirket4 = c.String(),
                        Sirket5 = c.String(),
                        Deger11 = c.String(),
                        Deger12 = c.String(),
                        Deger13 = c.String(),
                        Deger14 = c.String(),
                        Deger15 = c.String(),
                        Deger21 = c.String(),
                        Deger22 = c.String(),
                        Deger23 = c.String(),
                        Deger24 = c.String(),
                        Deger25 = c.String(),
                        Deger31 = c.String(),
                        Deger32 = c.String(),
                        Deger33 = c.String(),
                        Deger34 = c.String(),
                        Deger35 = c.String(),
                        Deger41 = c.String(),
                        Deger42 = c.String(),
                        Deger43 = c.String(),
                        Deger44 = c.String(),
                        Deger45 = c.String(),
                        Deger51 = c.String(),
                        Deger52 = c.String(),
                        Deger53 = c.String(),
                        Deger54 = c.String(),
                        Deger55 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tables");
        }
    }
}
