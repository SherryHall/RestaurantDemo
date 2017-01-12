namespace ResturantDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderTime = c.DateTime(nullable: false),
                        Complete = c.Boolean(nullable: false),
                        Location = c.String(),
                        CustomerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            AddColumn("dbo.MenuItems", "Order_Id", c => c.Int());
            CreateIndex("dbo.MenuItems", "Order_Id");
            AddForeignKey("dbo.MenuItems", "Order_Id", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MenuItems", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.MenuItems", new[] { "Order_Id" });
            DropColumn("dbo.MenuItems", "Order_Id");
            DropTable("dbo.Orders");
        }
    }
}
