using OnlineShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        IRepository<Category> CategoryRepository { get; }
        IRepository<Product> ProductRepository { get; }
    }
}
