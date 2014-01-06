namespace CampusBookFlip.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CollegeDataAgain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Institution", "InstitutionId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Institution", "InstitutionId");
        }
    }
}
