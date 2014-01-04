namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PublisherId = c.Int(nullable: false),
                        ISBN13 = c.String(nullable: false),
                        ISBN10 = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        PublishDate = c.String(),
                        ImageSmall = c.String(),
                        ImageLarge = c.String(),
                        PageCount = c.Int(nullable: false),
                        ListPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyCodeLP = c.String(),
                        RetailPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyCodeRP = c.String(),
                        AvailableAsEPUB = c.Boolean(nullable: false),
                        AvailableAsPDF = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Publisher", t => t.PublisherId, cascadeDelete: true)
                .Index(t => t.PublisherId);
            
            CreateTable(
                "dbo.BookAuthor",
                c => new
                    {
                        BookId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookId, t.AuthorId })
                .ForeignKey("dbo.Author", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Book", t => t.BookId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Publisher",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CBFUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppUserName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Paid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserCollege",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CollegeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CollegeId })
                .ForeignKey("dbo.College", t => t.CollegeId, cascadeDelete: true)
                .ForeignKey("dbo.CBFUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CollegeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.College",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        State = c.String(nullable: false),
                        City = c.String(nullable: false),
                        ZipCode = c.String(nullable: false, maxLength: 5),
                        Activated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserBook",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        SoldFor = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Book", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.CBFUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserBook", "UserId", "dbo.CBFUser");
            DropForeignKey("dbo.UserBook", "BookId", "dbo.Book");
            DropForeignKey("dbo.UserCollege", "UserId", "dbo.CBFUser");
            DropForeignKey("dbo.UserCollege", "CollegeId", "dbo.College");
            DropForeignKey("dbo.Book", "PublisherId", "dbo.Publisher");
            DropForeignKey("dbo.BookAuthor", "BookId", "dbo.Book");
            DropForeignKey("dbo.BookAuthor", "AuthorId", "dbo.Author");
            DropIndex("dbo.UserBook", new[] { "UserId" });
            DropIndex("dbo.UserBook", new[] { "BookId" });
            DropIndex("dbo.UserCollege", new[] { "UserId" });
            DropIndex("dbo.UserCollege", new[] { "CollegeId" });
            DropIndex("dbo.Book", new[] { "PublisherId" });
            DropIndex("dbo.BookAuthor", new[] { "BookId" });
            DropIndex("dbo.BookAuthor", new[] { "AuthorId" });
            DropTable("dbo.UserBook");
            DropTable("dbo.College");
            DropTable("dbo.UserCollege");
            DropTable("dbo.CBFUser");
            DropTable("dbo.Publisher");
            DropTable("dbo.BookAuthor");
            DropTable("dbo.Book");
            DropTable("dbo.Author");
        }
    }
}
