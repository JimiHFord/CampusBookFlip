namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullablePageCount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Book", "PageCount", c => c.Int(nullable: true));
            AlterColumn("dbo.Book", "ListPrice", c => c.Decimal(nullable: true, precision: 18, scale: 2));
            AlterColumn("dbo.Book", "RetailPrice", c => c.Decimal(nullable: true, precision: 18, scale: 2));
            AlterColumn("dbo.Book", "AvailableAsEPUB", c => c.Boolean(nullable: true));
            AlterColumn("dbo.Book", "AvailableAsPDF", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Book", "PageCount", c => c.Int(nullable: false));
            AlterColumn("dbo.Book", "ListPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Book", "RetailPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Book", "AvailableAsEPUB", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Book", "AvailableAsPDF", c => c.Boolean(nullable: false));
        }
    }
}
