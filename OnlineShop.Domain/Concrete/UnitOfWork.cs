using OnlineShop.Domain.Abstract;
using OnlineShop.Domain.Concrete;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private EFDbContext context = new EFDbContext();
        private Repository<Product> productRepository;
        private Repository<Category> categoryRepository;

        public IRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                    this.productRepository = new Repository<Product>(context);
                return productRepository;
            }
        }
        public IRepository<Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository == null)
                    this.categoryRepository = new Repository<Category>(context);
                return categoryRepository;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
