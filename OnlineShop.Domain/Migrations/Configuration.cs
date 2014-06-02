namespace OnlineShop.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using OnlineShop.Domain.Concrete;
    using OnlineShop.Domain.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineShop.Domain.Concrete.EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EFDbContext context)
        {
            var products = new List<Product>
            {
                new Product { ProductId = 1, Name = "Apple", Price = 150, Categories = new List<Category>()},
                new Product { ProductId = 2, Name = "Samsung", Price = 10, Categories = new List<Category>()}
            };

            products.ForEach(s => context.Products.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var categories = new List<Category>
            {
                new Category { Name = "Food", Products = new List<Product>() },
                new Category { Name = "Electronics", Products = new List<Product>() },
                new Category { Name = "Transport", Products = new List<Product>() },
                new Category { Name = "CellPhones", Products = new List<Product>() },
                new Category { Name = "Games", Products = new List<Product>() }
            };
            categories.ForEach(s => context.Categories.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();


            AddOrUpdateProduct(context, "Apple", "Food");
            AddOrUpdateProduct(context, "Apple", "Electronics");
            AddOrUpdateProduct(context, "Samsung", "Electronics");
            AddOrUpdateProduct(context, "Samsung", "CellPhones");
            AddOrUpdateProduct(context, "Banana", "Food");
            AddOrUpdateProduct(context, "Cherry", "Food");
            AddOrUpdateProduct(context, "Mango", "Electronics");
            AddOrUpdateProduct(context, "Mango", "CellPhones");
            context.SaveChanges();
        }

        void AddOrUpdateProduct(EFDbContext context, string ProductName, string CategoryName)
        {
            var crs = context.Products.SingleOrDefault(c => c.Name == ProductName);
            var inst = crs.Categories.SingleOrDefault(i => i.Name == CategoryName);
            if (inst == null)
                crs.Categories.Add(context.Categories.Single(i => i.Name == CategoryName));
        }
    }
}
