namespace iFinco.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Branch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "BranchId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "BranchId");
        }
    }
}
