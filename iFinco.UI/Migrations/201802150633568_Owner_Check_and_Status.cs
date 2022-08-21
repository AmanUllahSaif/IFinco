namespace iFinco.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Owner_Check_and_Status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsOwner", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Status");
            DropColumn("dbo.Users", "IsOwner");
        }
    }
}
