namespace OnlineShop.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.CategoryID })
                .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductCategory", new[] { "CategoryID" });
            DropIndex("dbo.ProductCategory", new[] { "ProductID" });
            DropForeignKey("dbo.ProductCategory", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.ProductCategory", "ProductID", "dbo.Product");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Category");
            DropTable("dbo.Product");
        }
    }
}
