namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig_heaing_status_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Headings", "HeadingContent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Headings", "HeadingContent");
        }
    }
}
