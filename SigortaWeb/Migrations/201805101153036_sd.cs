namespace SigortaWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sd : DbMigration
    {
        public override void Up()
        {
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Data", "companyId", "dbo.Companies");
            DropForeignKey("dbo.Data", "branchId", "dbo.Branches");
            DropForeignKey("dbo.Data", "accountId", "dbo.Accounts");
            DropForeignKey("dbo.Companies", "companyTypeId", "dbo.CompanyTypes");
            DropForeignKey("dbo.Branches", "branchGroupId", "dbo.BranchGroups");
            DropForeignKey("dbo.AccountCalcRelations", "calcId", "dbo.Calcs");
            DropForeignKey("dbo.AccountCalcRelations", "accountId", "dbo.Accounts");
            DropIndex("dbo.Data", new[] { "accountId" });
            DropIndex("dbo.Data", new[] { "branchId" });
            DropIndex("dbo.Data", new[] { "companyId" });
            DropIndex("dbo.Companies", new[] { "companyTypeId" });
            DropIndex("dbo.Branches", new[] { "branchGroupId" });
            DropIndex("dbo.AccountCalcRelations", new[] { "calcId" });
            DropIndex("dbo.AccountCalcRelations", new[] { "accountId" });
            DropTable("dbo.Sayacs");
            DropTable("dbo.Data");
            DropTable("dbo.CompanyTypes");
            DropTable("dbo.Companies");
            DropTable("dbo.Branches");
            DropTable("dbo.BranchGroups");
            DropTable("dbo.Calcs");
            DropTable("dbo.Accounts");
            DropTable("dbo.AccountCalcRelations");
        }
    }
}
