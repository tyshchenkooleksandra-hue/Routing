using System.Collections.Generic;
using ProductsWithRouting.Models;

namespace ProductsWithRouting.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll(int? filterId, string filterName);

        Product GetById(int id);

        int GetNextId();

        void Add(Product product);

        bool Update(Product product);

        bool Delete(int id);
    }
}
